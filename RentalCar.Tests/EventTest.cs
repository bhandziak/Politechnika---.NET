using RentalCarProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalCar.Tests
{
    public class EventTest
    {
        [Test]
        public void OnNewReservation_ShouldTriggerEvent() 
        {
            var rentalCompany = new RentalCompany();

            rentalCompany.AddVehicle(new Car(1, "Toyota", "Corolla", 2020, "Sedan"));
            rentalCompany.AddVehicle(new Motorcycle(2, "Yamaha", "MT-07", 2021, 689));

            string? messageToCheck = null;

            rentalCompany.OnNewReservation += message => messageToCheck = message;

            Reservation reservation = rentalCompany.ReserveVehicle(1, "John Doe");

            Assert.AreEqual($"Nowa rezerwacja: {reservation}", messageToCheck);
        }
    }
}
