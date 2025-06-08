namespace CarWorkshopProjekt.Services
{
    public interface ICustomerService
    {

        // [HttpPost("addCustomer")] validation
        bool IsValidFirstName(string firstName, out string errorMessage);
        bool IsValidLastName(string lastName, out string errorMessage);
        bool IsValidPhoneNumber(string phoneNumber, out string errorMessage);


        // [HttpPost("addVehicle/{customerID}")] validation
        bool IsValidBrand(string brand, out string errorMessage);
        bool IsValidModel(string model, out string errorMessage);
        bool IsValidVIN(string vin, out string errorMessage);
        bool IsValidRegistralNumber(string regNum, out string errorMessage);
        bool IsValidYear(int year, out string errorMessage);

        // [HttpPost("getDetails/addVehicleImage/{vehicleID}")] validation and image save
        bool IsValidImage(IFormFile file, out string error);
        Task<string> SaveImageAsync(IFormFile file);
    }
}
