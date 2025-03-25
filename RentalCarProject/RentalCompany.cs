using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalCarProject
{
    public class RentalCompany
    {
        private List<Vehicle> vehicles = new List<Vehicle>();
        private List<Reservation> reservations = new List<Reservation>();
        public event Action<string> OnNewReservation;
        private int ResertationCounter = 0;

        public RentalCompany()
        {

        }

        public void AddVehicle(Vehicle vehicle)
        {
            vehicles.Add(vehicle);
        }


        public Reservation ReserveVehicle(int vehicleId, string customer)
        {
            Vehicle? vehicle = vehicles.Find(v => v.Id == vehicleId);
            if (vehicle == null)
            {
                throw new ArgumentException($"Nie ma takie samohodu o id {vehicleId}");
            }

            if(customer == null || customer == "")
            {
                throw new ArgumentException("Pole customer nie może być puste");
            }

            if (!vehicle.IsAvailableBool)
            {
                throw new ArgumentException($"Samochod o id {vehicleId} jest już zarezerwonany!");
            }

            Reservation reservation = new Reservation(ResertationCounter++, vehicle, customer);
            reservations.Add(reservation);

            if(vehicle is Car)
            {
                Car car = (Car)vehicle;
                car.Reserve(customer);
            }
            else if(vehicle is Motorcycle)
            {
                Motorcycle motorcycle = (Motorcycle)vehicle;
                motorcycle.Reserve(customer);
            }

            OnNewReservation?.Invoke($"Nowa rezerwacja: {reservation.ToString()}");

            return reservation;
        }

        public void CancelReservation(int reservationId) 
        {
            Reservation? reservation = reservations.Find(r => r.ReservationId == reservationId);

            if(reservation == null)
            {
                throw new ArgumentException($"Nie ma takiej rezerwacji o id {reservationId}");
            }
            if (reservation.ReservedVehicle is Car)
            {
                Car car = (Car)reservation.ReservedVehicle;
                car.CancelReservation();
            }
            else if (reservation.ReservedVehicle is Motorcycle)
            {
                Motorcycle motorcycle = (Motorcycle)reservation.ReservedVehicle;
                motorcycle.CancelReservation();
            }

            reservations.Remove(reservation);
        }

        public Reservation GetReservationById(int reservationId)
        {
            Reservation? reservation = reservations.Find(r => r.ReservationId == reservationId);
            if (reservation == null)
            {
                throw new ArgumentException($"Nie ma takiej rezerwacji o id {reservationId}");
            }

            return reservation;
        }

        public void ListAvailableVehicles()
        {
            List<Vehicle> listOfAvailableVehicles = VehicleExtensions.GetAvailableVehicles(vehicles);
            foreach (Vehicle vehicle in listOfAvailableVehicles)
            {
                vehicle.DisplayInfo();
            }
        }

        public List<Vehicle> GetAllVehicles()
        {
            return vehicles;
        }

        public List<Reservation> GetAllReservations()
        {
            return reservations;
        }
    }
}
