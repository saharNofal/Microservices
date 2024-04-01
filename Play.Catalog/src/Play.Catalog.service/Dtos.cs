using System.ComponentModel.DataAnnotations;

namespace Play.Catalog.service.Dtos
{
    public record ItemDto(Guid Id, string Name, string Description, decimal Price,
     DateTimeOffset CreatedDate);

    public record CreatedItemDTo([Required] string Name, string Description, [Range(0, 1000)] decimal Price);
    public record UpdateItemDTo([Required] string Name, string Description, [Range(0, 1000)] decimal Price);

}