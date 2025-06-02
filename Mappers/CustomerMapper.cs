namespace CarWorkshopProjekt.Mappers
{
    using Riok.Mapperly.Abstractions;
    using CarWorkshopProjekt.DTOs;
    using CarWorkshopProjekt.Data;
    [Mapper]
    public partial class CustomerMapper
    {
        //[HttpGet("getCustomers")]
        public partial ReturnCustomer ToReturnDto(Customer customer);
        public partial List<ReturnCustomer> ToReturnDtoList(List<Customer> customers);

        //[HttpPost("addCustomer")]
        public partial Customer MapToEntity(AddCustomer customerDto);

        //[HttpPut("update")]
        public partial void UpdateCustomer(UpdateCustomer customerDto, [MappingTarget] Customer customerEntity);
    }
}
