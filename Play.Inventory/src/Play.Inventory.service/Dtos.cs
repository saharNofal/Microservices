using System.ComponentModel.DataAnnotations;

namespace Play.Inventory.service.Dtos
{
    public record GrantItemDto(Guid UserId, Guid CatalogItemId, int Quantity);


    public record InventoryItemDTo(Guid CatalogItemId, string Name, string Description, int Quantity, DateTimeOffset AcquiredDate);
    // sync approach 
    public record CatalogItemDto(Guid Id, string Name, string Description);

}