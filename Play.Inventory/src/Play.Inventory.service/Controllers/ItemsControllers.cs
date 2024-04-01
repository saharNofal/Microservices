using Microsoft.AspNetCore.Mvc;
using Play.Inventory.service.Entities;
using Play.common;
using Play.Inventory.service.Dtos;
using Play.Inventory.service.Client;
using Play.Inventory.service;

namespace Play.Catalog.service.Controllers
{
    [ApiController]
    [Route("Items")]
    public class ItemsControllers : ControllerBase
    {
        private readonly IRepository<InventoryItem> InventoryItemsRepository;
        // private readonly CatalogClient catalogClient;
        private readonly IRepository<CatalogItem> catalogItemsRepository;
        public ItemsControllers(IRepository<InventoryItem> InventoryItemsRepository, IRepository<CatalogItem> catalogItemsRepository)
        {
            this.InventoryItemsRepository = InventoryItemsRepository;
            this.catalogItemsRepository = catalogItemsRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDTo>>> GetAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                return BadRequest();

            //var catalogItms = await catalogClient.GetCatalogItemAsync();
            var inventoryItemEntity = await InventoryItemsRepository.GetAllAsync(item => item.UserId == userId);
            var itemIds = inventoryItemEntity.Select(item => item.CatalogItemId);
            var catalogItmsEntities = await catalogItemsRepository.GetAllAsync(item => itemIds.Contains(item.Id));
            var inventoryItemDto = inventoryItemEntity.Select(inventoryItem =>
            {
                var categoryItem = catalogItmsEntities.Single(categoryItem => categoryItem.Id == inventoryItem.CatalogItemId);
                return inventoryItem.asDto(categoryItem.Name, categoryItem.Description);
            });
            return Ok(inventoryItemDto);
        }


        [HttpPost]
        public async Task<ActionResult> PostAsync(GrantItemDto grantItemDto)
        {
            var inventoryItem = await InventoryItemsRepository.GetAsync(x => x.UserId == grantItemDto.UserId
            && x.CatalogItemId == grantItemDto.CatalogItemId);
            if (inventoryItem == null)
            {
                inventoryItem = new InventoryItem()
                {
                    CatalogItemId = grantItemDto.CatalogItemId,
                    UserId = grantItemDto.UserId,
                    Quantity = grantItemDto.Quantity,
                    AcquiredDate = DateTimeOffset.UtcNow
                };
                await InventoryItemsRepository.CreateAsync(inventoryItem);
            }
            else
            {
                inventoryItem.Quantity += grantItemDto.Quantity;
                await InventoryItemsRepository.UpdateAsync(inventoryItem);
            }
            return Ok();
        }
    }
}


