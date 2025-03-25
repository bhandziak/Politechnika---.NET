using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalCarProject
{
    public class Car : Vehicle, IReservable
    {
        public string BodyType { get; }

        public Car(int id, string brand, string model, int year, string bodyType) : base(id, brand, model, year)
        {
            BodyType = bodyType;
        }

        public void CancelReservation()
        {
            if (!IsAvailable())
            {
                IsAvailableBool = true;
            }
            else
            {
                throw new ArgumentException($"Samochód {Id} nie został zarezerwowany!");
            }
        }

        public bool IsAvailable()
        {
            return IsAvailableBool;
        }

        public void Reserve(string customer)
        {
            if (IsAvailable())
            {
                IsAvailableBool = false;
            }
            else
            {
                throw new ArgumentException($"Samochód {Id} został już zarezerwowany!");
            }
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Typ: Samochód, id: {Id}, marka: {Brand}, model: {Model}" +
                $", rok produkcji: {Year}, klasa: {BodyType}");
        }
    }
}
