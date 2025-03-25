using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalCarProject
{
    public class Reservation
    {
        public int ReservationId { get; }
        public Vehicle ReservedVehicle {get; }
        private string Customer;
        private DateTime ReservationDate;

        public Reservation(int reservationId, Vehicle reservedVehicle, string customer)
        {
            ReservationId = reservationId;
            ReservedVehicle = reservedVehicle;
            Customer = customer;
            ReservationDate = DateTime.Now;
        }

        override public string ToString()
        {
            return $"Id rezerwacji {ReservationId}, marka: {ReservedVehicle.Brand}, model: {ReservedVehicle.Model}" +
                $", imie i nazwisko: {Customer}, data wypożyczenia: {ReservationDate.ToString("dd.MM.yyyy")}";
        }
    }
}
