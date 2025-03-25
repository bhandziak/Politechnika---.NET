using RentalCarProject;

namespace RentalCar.Tests
{
    public class VehicleTests
    {

        [Test]
        public void Car_ShouldCreateCorrectObject()
        {
            int expectedId = 1;
            string expectedBrand = "Toyota";
            string expectedModel = "Corolla";
            int expectedYear = 2020;
            string expectedBodyType = "Sedan";
            
            Car car = new Car(expectedId, expectedBrand, expectedModel, expectedYear, expectedBodyType);

            Assert.AreEqual(expectedId, car.Id);
            Assert.AreEqual(expectedBrand, car.Brand);
            Assert.AreEqual(expectedModel, car.Model);   
            Assert.AreEqual(expectedYear, car.Year);   
            Assert.AreEqual(expectedBodyType, car.BodyType); 
        }

        [Test]
        public void Motorcycle_ShouldCreateCorrectObject() {
            int expectedId = 1;
            string expectedBrand = "Yamaha";
            string expectedModel = "MT-07";
            int expectedYear = 2022;
            int EngineCapacity = 8;

            Motorcycle motorcycle = new Motorcycle(expectedId, expectedBrand, expectedModel, expectedYear, EngineCapacity);

            Assert.AreEqual(expectedId, motorcycle.Id);
            Assert.AreEqual(expectedBrand, motorcycle.Brand);
            Assert.AreEqual(expectedModel, motorcycle.Model);
            Assert.AreEqual(expectedYear, motorcycle.Year);
            Assert.AreEqual(EngineCapacity, motorcycle.EngineCapacity);
        }
    }
}