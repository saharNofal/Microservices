using MassTransit;
using Play.Catalog.Contracts;
using Play.common;
using Play.Inventory.service.Entities;

namespace Play.Inventory.service.Consumers
{
    public class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreated>
    {
        private readonly IRepository<CatalogItem> repository;
        public CatalogItemCreatedConsumer(IRepository<CatalogItem> repository)
        {
            this.repository = repository;
        }
        public async Task Consume(ConsumeContext<CatalogItemCreated> context)
        {
            var message = context.Message;
            var item = await repository.GetAsync(message.ItemId);
            if (item is not null)
                return;
            item = new CatalogItem()
            {
                Id = message.ItemId,
                Description = message.Description,
                Name = message.Name
            };
            await repository.CreateAsync(item);
        }
    }
}