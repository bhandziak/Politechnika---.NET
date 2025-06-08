using Xunit;
using CarWorkshopProjekt.Controllers;
using CarWorkshopProjekt.Data;
using CarWorkshopProjekt.DTOs;
using CarWorkshopProjekt.Mappers;
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
    public class PartControllerTests
    {
        private AppDbContext GetInMemoryDb(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new AppDbContext(options);
        }

        // test dla api GET
        [Fact]
        public async Task GetAll_ReturnsMappedPartsList()
        {
            // Arrange
            var context = GetInMemoryDb("GetAllPartsDb");
            context.Parts.AddRange(new List<Part>
            {
                new Part { PartId = Guid.NewGuid(), NamePart = "Filtr", TypePart = "Olejowy", UnitPrice = 25.0m },
                new Part { PartId = Guid.NewGuid(), NamePart = "Pasek", TypePart = "Napędowy", UnitPrice = 50.0m }
            });
            context.SaveChanges();

            var controller = new PartController(context);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<List<PartDTO>>(okResult.Value);
            Assert.Equal(2, returned.Count);
            Assert.All(returned, dto => Assert.IsType<Guid>(dto.PartId));
        }

        // test dla api DELETE
        [Fact]
        public async Task DeletePart_ExistingPart_ReturnsOk()
        {
            // Arrange
            var context = GetInMemoryDb("DeletePart_ExistingPart");
            var part = new Part { PartId = Guid.NewGuid(), NamePart = "Filtr", TypePart = "Olejowy", UnitPrice = 30.0m };
            context.Parts.Add(part);
            context.SaveChanges();

            var controller = new PartController(context);

            // Act
            var result = await controller.DeletePart(part.PartId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Część została pomyślnie usunięta.", okResult.Value);
        }

    }
}
