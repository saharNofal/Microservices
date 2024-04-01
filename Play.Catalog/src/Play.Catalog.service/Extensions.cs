using Play.Catalog.service.Dtos;
using Play.Catalog.service.Entities;

namespace Play.Catalog.service
{
    public static class Extensions
    {
        public static ItemDto asDto(this Item item)
        {
            return new ItemDto(item.Id, item.Name ?? "", item.Description ?? "", item.Price, item.CreatedDate);
        }
    }

}