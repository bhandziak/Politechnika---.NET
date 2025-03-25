using RentalCarProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalCar.Tests
{
    public class ExtensionTest
    {
        [Test]
        public void GetAvailableVehicles_ReturnsCorrectList()
        {
            var rentalCompany = new RentalCompany();

            Car car = new Car(1, "Toyota", "Corolla", 2020, "Sedan");
            Motorcycle motorcycle = new Motorcycle(2, "Yamaha", "MT-07", 2021, 689);

            rentalCompany.AddVehicle(car);
            rentalCompany.AddVehicle(motorcycle);

            rentalCompany.ReserveVehicle(1, "John Doe");

            List<Vehicle> availableVehicles = VehicleExtensions.GetAvailableVehicles(rentalCompany.GetAllVehicles());

            CollectionAssert.AreEqual(new List<Vehicle> { motorcycle }, availableVehicles);
        }
    }
}
