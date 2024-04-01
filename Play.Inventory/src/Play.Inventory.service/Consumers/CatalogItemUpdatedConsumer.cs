using MassTransit;
using Play.Catalog.Contracts;
using Play.common;
using Play.Inventory.service.Entities;

namespace Play.Inventory.service.Consumers
{
    public class CatalogItemUpdatedConsumer : IConsumer<CatalogItemUpdated>
    {
        private readonly IRepository<CatalogItem> repository;
        public CatalogItemUpdatedConsumer(IRepository<CatalogItem> repository)
        {
            this.repository = repository;
        }
        public async Task Consume(ConsumeContext<CatalogItemUpdated> context)
        {
            var message = context.Message;
            var item = await repository.GetAsync(message.ItemId);
            if (item is null)
            {
                item = new CatalogItem()
                {
                    Id = message.ItemId,
                    Description = message.Description,
                    Name = message.Name
                };
                await repository.CreateAsync(item);
            }
            else
            {
                item.Name = message.Name;
                item.Description = message.Description;
                await repository.UpdateAsync(item);
            }


        }
    }
}