using CarWorkshopProjekt.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWorkshop.Tests
{
    public class CustomerServiceTests
    {
        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            _customerService = new CustomerService();
        }

        [Fact]
        public void IsValidFirstName_ReturnsTrue()
        {
            string firstName = "Jan";

            // Act
            var result = _customerService.IsValidFirstName(firstName, out var errorMessage);

            // Assert
            Assert.Equal(true, result);
            Assert.Equal(null, errorMessage);
        }

        [Fact]
        public void IsValidFirstName_ReturnsFalse()
        {
            string firstName = "jan";

            // Act
            var result = _customerService.IsValidFirstName(firstName, out var errorMessage);

            // Assert
            Assert.Equal(false, result);
            Assert.Equal("Imię musi zaczynać się wielką literą i zawierać tylko litery.", errorMessage);
        }
    }
}
