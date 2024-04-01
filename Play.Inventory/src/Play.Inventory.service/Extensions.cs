

using Play.Inventory.service.Dtos;
using Play.Inventory.service.Entities;

namespace Play.Inventory.service
{
    public static class Extensions
    {
        public static InventoryItemDTo asDto(this InventoryItem item, string name, string description)
        {
            return new InventoryItemDTo(item.CatalogItemId, name ?? "", description ?? "", item.Quantity, item.AcquiredDate);
        }
    }

}