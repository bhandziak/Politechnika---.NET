using CarWorkshopProjekt.Data;
using CarWorkshopProjekt.DTOs;
using Riok.Mapperly.Abstractions;

namespace CarWorkshopProjekt.Mappers
{
    [Mapper]
    public partial class PartMapper
    {
        public partial List<PartDTO> ToReturnDtoList(List<Part> parts);

    }
}
