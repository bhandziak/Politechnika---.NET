using CarWorkshopProjekt.Data;
using CarWorkshopProjekt.DTOs;
using Riok.Mapperly.Abstractions;

namespace CarWorkshopProjekt.Mappers
{
    [Mapper]
    public partial class PartMapper
    {
        public partial List<PartDTO> ToReturnDtoList(List<Part> parts);

        //[HttpPost("addPart")]
        public Part MapToEntity(AddPart partDto)
        {
            if (!decimal.TryParse(partDto.UnitPrice, out var unitPrice))
            {
                //nieudana konwersja
                return null;
            }
            return new Part
            {
                PartId = Guid.NewGuid(),
                NamePart =partDto.NamePart,
                TypePart = partDto.TypePart,
                UnitPrice = unitPrice,
            };
        }

        //[HttpPut("update/{partId}")]
        public partial void UpdatePart(AddPart partDto, [MappingTarget] Part partEntity);
    }
}
