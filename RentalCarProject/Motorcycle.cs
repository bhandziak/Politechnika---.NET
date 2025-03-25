using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalCarProject
{
    public class Motorcycle : Vehicle, IReservable
    {
        public int EngineCapacity {get; }

        public Motorcycle(int id, string brand, string model, int year, int engineCapacity) : base(id, brand, model, year)
        {
            EngineCapacity = engineCapacity;
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
            Console.WriteLine($"Typ: Motor, id: {Id}, marka: {Brand}, model: {Model}" +
                $", rok produkcji: {Year}, pojemność silnika: {EngineCapacity}");
        }
    }
}
