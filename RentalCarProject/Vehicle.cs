using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalCarProject
{
    public abstract class Vehicle
    {
        public int Id { get; }
        public string Brand { get; }
        public string Model { get; }
        public int Year { get; }
        public bool IsAvailableBool { get; set; }

        public Vehicle(int id, string brand, string model, int year)
        {
            Id = id;
            Brand = brand;
            Model = model;
            Year = year;
            IsAvailableBool = true;
        }

        public abstract void DisplayInfo();
    }
}
