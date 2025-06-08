using System.Text.RegularExpressions;

namespace CarWorkshopProjekt.Services
{
    public class CustomerService: ICustomerService
    {
        // [HttpPost("addCustomer")] validation
        private readonly Regex _nameRegex = new Regex(@"^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]{1,29}$");
        private readonly Regex _surnameRegex = new Regex(@"^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż\-]{1,49}$");
        private readonly Regex _phoneRegex = new Regex(@"^\+48\d{9}$");

        // [HttpPost("addVehicle/{customerID}")] validation
        private readonly Regex _brandRegex = new Regex(@"^[A-Z][a-zA-Z\s\-]{1,29}$");
        private readonly Regex _modelRegex = new Regex(@"^[A-Za-z0-9\s\-]{1,30}$");
        private readonly Regex _vinRegex = new Regex(@"^[A-HJ-NPR-Z0-9]{17}$");
        private readonly Regex _registralNumberRegex = new Regex(@"^[A-Z]{2,3}\s?\d{4,5}[A-Z]{0,2}$");

        // [HttpPost("getDetails/addVehicleImage/{vehicleID}")] validation and image save
        private readonly string[] _allowedExtensions = [".jpg", ".png"];
        private readonly string _uploadPath = Path.Combine("wwwroot", "uploads");

        // [HttpPost("addCustomer")] validation
        public bool IsValidFirstName(string firstName, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(firstName) || !_nameRegex.IsMatch(firstName))
            {
                errorMessage = "Imię musi zaczynać się wielką literą i zawierać tylko litery.";
                return false;
            }
            errorMessage = null;
            return true;
        }

        public bool IsValidLastName(string lastName, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(lastName) || !_surnameRegex.IsMatch(lastName))
            {
                errorMessage = "Nazwisko musi zaczynać się wielką literą i zawierać tylko litery lub myślnik.";
                return false;
            }
            errorMessage = null;
            return true;
        }

        public bool IsValidPhoneNumber(string phoneNumber, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber) || !_phoneRegex.IsMatch(phoneNumber))
            {
                errorMessage = "Numer telefonu musi być w formacie +48123456789.";
                return false;
            }
            errorMessage = null;
            return true;
        }

        // [HttpPost("addVehicle/{customerID}")] validation
        public bool IsValidBrand(string brand, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(brand) || !_brandRegex.IsMatch(brand))
            {
                errorMessage = "Marka musi zaczynać się wielką literą i zawierać tylko litery, spacje lub myślniki.";
                return false;
            }
            errorMessage = null;
            return true;
        }

        public bool IsValidModel(string model, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(model) || !_modelRegex.IsMatch(model))
            {
                errorMessage = "Model może zawierać litery, cyfry, spacje i myślniki.";
                return false;
            }
            errorMessage = null;
            return true;
        }

        public bool IsValidVIN(string vin, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(vin) || !_vinRegex.IsMatch(vin))
            {
                errorMessage = "VIN musi składać się z dokładnie 17 znaków (bez I, O, Q).";
                return false;
            }
            errorMessage = null;
            return true;
        }

        public bool IsValidRegistralNumber(string regNum, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(regNum) || !_registralNumberRegex.IsMatch(regNum))
            {
                errorMessage = "Numer rejestracyjny ma nieprawidłowy format.";
                return false;
            }
            errorMessage = null;
            return true;
        }

        public bool IsValidYear(int year, out string errorMessage)
        {
            if (year < 1850 || year > 2100)
            {
                errorMessage = "Rok musi być w zakresie 1850–2100.";
                return false;
            }
            errorMessage = null;
            return true;
        }


        // [HttpPost("getDetails/addVehicleImage/{vehicleID}")] validation and image save
        public bool IsValidImage(IFormFile file, out string error)
        {
            if (file == null || file.Length == 0)
            {
                error = "Plik jest pusty.";
                return false;
            }

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(ext))
            {
                error = "Dozwolone formaty: JPG, PNG.";
                return false;
            }

            error = null;
            return true;
        }

        public async Task<string> SaveImageAsync(IFormFile file)
        {
            Directory.CreateDirectory(_uploadPath);

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var fileName = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(_uploadPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/{fileName}";
        }
    }
}
