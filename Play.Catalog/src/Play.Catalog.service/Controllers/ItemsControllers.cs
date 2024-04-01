using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.service.Dtos;
using Play.Catalog.service.Entities;
using Play.common;
using Play.Catalog.Contracts;
namespace Play.Catalog.service.Controllers;

[ApiController]
[Route("Items")]
public class ItemsControllers : ControllerBase
{
    private readonly IRepository<Item> itemsRepository;
    private readonly IPublishEndpoint publishEndpoint;
    public ItemsControllers(IRepository<Item> itemsRepository, IPublishEndpoint publishEndpoint)
    {
        this.itemsRepository = itemsRepository;
        this.publishEndpoint = publishEndpoint;
    }
    [HttpGet]
    public async Task<IEnumerable<ItemDto>> GetAsync()
    {
        var items = (await itemsRepository.GetAllAsync()).Select(x => x.asDto());
        return items;
    }

    [HttpGet("id")]
    public async Task<ActionResult<ItemDto>> GetbyIdAsync(Guid Id)
    {
        var item = await itemsRepository.GetAsync(Id);
        if (item is null)
            return NotFound();
        return item.asDto();
    }
    [HttpPost]
    public async Task<ActionResult<ItemDto>> PostAsync(CreatedItemDTo createdItemDTo)
    {
        Item item = new Item()
        {
            Name = createdItemDTo.Name,
            Description = createdItemDTo.Description,
            Price = createdItemDTo.Price,
            CreatedDate = DateTimeOffset.UtcNow
        };
        await itemsRepository.CreateAsync(item);
        await publishEndpoint.Publish(new CatalogItemCreated(item.Id, item.Name, item.Description));
        return CreatedAtAction(nameof(GetbyIdAsync), new { id = item.Id }, item);

    }

    [HttpPut("id")]
    public async Task<IActionResult> Put(Guid id, UpdateItemDTo updateItemDTo)
    {
        var existingItem = await itemsRepository.GetAsync(id);
        if (existingItem is null)
            return NotFound();

        existingItem.Name = updateItemDTo.Name;
        existingItem.Description = updateItemDTo.Description;
        existingItem.Price = updateItemDTo.Price;
        await itemsRepository.UpdateAsync(existingItem);
        await publishEndpoint.Publish(new CatalogItemUpdated(existingItem.Id, existingItem.Name, existingItem.Description));

        return NoContent();
    }
    [HttpDelete("id")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var existingItem = await itemsRepository.GetAsync(id);
        if (existingItem is null)
            return NotFound();
        await itemsRepository.RemoveAsync(id);
        await publishEndpoint.Publish(new CatalogItemDeleted(id));

        return NoContent();
    }
}
