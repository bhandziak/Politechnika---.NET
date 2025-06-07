using CarWorkshopProjekt.Controllers;
using CarWorkshopProjekt.Data;
using CarWorkshopProjekt.DTOs;
using CarWorkshopProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWorkshop.Tests
{
    public class CustomerControllerTests
    {
        private AppDbContext GetInMemoryDb(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new AppDbContext(options);
        }

        // test dla api PUT
        [Fact]
        public async Task UpdateCustomer_UpdatesCustomerCorrectly()
        {
            // Arrange
            var context = GetInMemoryDb("UpdateCustomerDb");

            var existingCustomer = new Customer
            {
                CustomerId = Guid.NewGuid(),
                NameCustomer = "Jan",
                SurnameCustomer = "Kowalski",
                PhoneNumber = "+48123456789"
            };

            context.Customers.Add(existingCustomer);
            await context.SaveChangesAsync();


            // Mock symuluje serwisy tak aby nie odpalać logiki serwisów
            var mockCustomerService = new Mock<ICustomerService>();
            var mockRaportService = new Mock<IRaportService>();
            var controller = new CustomerController(context, mockCustomerService.Object, mockRaportService.Object);

            var updateDto = new UpdateCustomer
            {
                CustomerId = existingCustomer.CustomerId,
                NameCustomer = "Janek",
                SurnameCustomer = "Nowak",
                PhoneNumber = "+48987654321"
            };

            // Act
            var result = await controller.UpdateCustomer(updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Klient został pomyślnie zaktualizowany.", okResult.Value);

            var updatedCustomer = await context.Customers.FindAsync(existingCustomer.CustomerId);
            Assert.Equal("Janek", updatedCustomer.NameCustomer);
            Assert.Equal("Nowak", updatedCustomer.SurnameCustomer);
            Assert.Equal("+48987654321", updatedCustomer.PhoneNumber);
        }

        // test dla api POST

        [Fact]
        public async Task AddCustomer_ReturnsOk_WhenDataIsValid()
        {
            // Arrange
            var context = GetInMemoryDb("AddCustomerDB");

            var mockCustomerService = new Mock<ICustomerService>();
            var mockRaportService = new Mock<IRaportService>();

            // Dane wejściowe
            var newCustomerDto = new AddCustomer
            {
                NameCustomer = "Jan",
                SurnameCustomer = "Kowalski",
                PhoneNumber = "+48123456789"
            };

            // Mockowanie walidacji
            // czyli nie testujemy logiki walidacji
            mockCustomerService.Setup(s => s.IsValidFirstName(It.IsAny<string>(), out It.Ref<string>.IsAny))
                .Returns(true);
            mockCustomerService.Setup(s => s.IsValidLastName(It.IsAny<string>(), out It.Ref<string>.IsAny))
                .Returns(true);
            mockCustomerService.Setup(s => s.IsValidPhoneNumber(It.IsAny<string>(), out It.Ref<string>.IsAny))
                .Returns(true);

            var controller = new CustomerController(
                context,
                mockCustomerService.Object,
                mockRaportService.Object
            );

            // Act
            var result = await controller.AddCustomer(newCustomerDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var messageProp = okResult.Value.GetType().GetProperty("message");
            Assert.NotNull(messageProp);

            var message = messageProp.GetValue(okResult.Value) as string;
            Assert.Equal("Klient zarejestrowany pomyślnie.", message);

            Assert.Single(context.Customers);
            var savedCustomer = await context.Customers.FirstAsync();
            Assert.Equal("Jan", savedCustomer.NameCustomer);
            Assert.Equal("Kowalski", savedCustomer.SurnameCustomer);
            Assert.Equal("+48123456789", savedCustomer.PhoneNumber);
        }
    }
}
