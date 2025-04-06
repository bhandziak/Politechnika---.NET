using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype_Project
{
    public class Cat : Animal
    {
        public string Breed { get; set; }

        public Cat(string name, int age, Owner owner, string breed) : base(name, age, owner)
        {
            Breed = breed;
        }

        public override Cat ShallowClone()
        {
            return (Cat)this.MemberwiseClone();
        }

        public override Cat DeepClone()
        {
            Cat clone = (Cat)this.MemberwiseClone();
            clone.Owner = new Owner(Owner.Name, Owner.Surname);
            return clone;
        }

        public override string ToString()
        {
            return "Name: " + Name + ", age: " + Age + ", owner: " + Owner.Name + " " + Owner.Surname
                + ", breed: " + Breed;
        }
    }
}
