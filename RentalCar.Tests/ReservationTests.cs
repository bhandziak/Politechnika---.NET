using RentalCarProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalCar.Tests
{
    public class ReservationTests
    {
        [Test]
        public void ReserveVehicle_ShouldAppendReservation_ShouldChangeAvailablity()
        {
            var rentalCompany = new RentalCompany();

            Car car = new Car(1, "Toyota", "Corolla", 2020, "Sedan");

            rentalCompany.AddVehicle(car);

            Reservation reservation = rentalCompany.ReserveVehicle(1, "John Doe");

            Assert.IsTrue(
                rentalCompany.GetAllReservations().Contains(reservation)
            );

            Assert.IsFalse(car.IsAvailableBool);
        }

        [Test]
        public void ReserveVehicle_ReserveNotAvailableVehicle_ThrowsException()
        {
            var rentalCompany = new RentalCompany();

            Car car = new Car(1, "Toyota", "Corolla", 2020, "Sedan");

            rentalCompany.AddVehicle(car);
            rentalCompany.ReserveVehicle(1, "John Doe");

            Assert.Throws<ArgumentException>(() =>
                rentalCompany.ReserveVehicle(1, "Jan Kowalski")
            );
        }

        [Test]
        public void ReserveVehicle_ReserveNonExistentVehicle_ThrowsException()
        {
            var rentalCompany = new RentalCompany();

            Car car = new Car(1, "Toyota", "Corolla", 2020, "Sedan");

            rentalCompany.AddVehicle(car);
            Assert.Throws<ArgumentException>(() =>
                rentalCompany.ReserveVehicle(2, "John Doe")
            );
            
        }

        [Test]
        public void CancelReservation_ShouldRemoveReservation_ShouldChangeAvailablity()
        {
            var rentalCompany = new RentalCompany();

            Car car = new Car(1, "Toyota", "Corolla", 2020, "Sedan");

            rentalCompany.AddVehicle(car);

            Reservation reservation = rentalCompany.ReserveVehicle(1, "John Doe");
            rentalCompany.CancelReservation(reservation.ReservationId);

            Assert.IsFalse(
                rentalCompany.GetAllReservations().Contains(reservation)
            );

            Assert.IsTrue(car.IsAvailableBool);
        }

        [Test]
        public void CancelReservation_CancelNonExistentReservation_ThrowsException()
        {
            var rentalCompany = new RentalCompany();

            Car car = new Car(1, "Toyota", "Corolla", 2020, "Sedan");

            rentalCompany.AddVehicle(car);
            Reservation reservation = rentalCompany.ReserveVehicle(1, "John Doe");

            Assert.Throws<ArgumentException>(() =>
                rentalCompany.CancelReservation(5)
            );
        }

        [Test]
        public void GetReservationById_ShouldReturnCorrectReservation()
        {
            var rentalCompany = new RentalCompany();

            Car car = new Car(1, "Toyota", "Corolla", 2020, "Sedan");

            rentalCompany.AddVehicle(car);
            Reservation reservation = rentalCompany.ReserveVehicle(1, "John Doe");

            Assert.AreEqual(reservation, rentalCompany.GetReservationById(reservation.ReservationId));
        }


        [Test]
        public void GetReservationById_GetNonExistentReservation_ThrowsException()
        {
            var rentalCompany = new RentalCompany();

            Car car = new Car(1, "Toyota", "Corolla", 2020, "Sedan");

            rentalCompany.AddVehicle(car);
            Reservation reservation = rentalCompany.ReserveVehicle(1, "John Doe");

            Assert.Throws<ArgumentException>(() =>
                rentalCompany.GetReservationById(2)
            );
        }
    }
}
