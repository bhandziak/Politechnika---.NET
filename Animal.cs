using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype_Project
{
    public class Animal : IPrototype<Animal>
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Owner Owner { get; set; }

        public Animal(string name, int age, Owner owner)
        {
            Name = name;
            Age = age;
            Owner = owner;
        }

        public virtual Animal ShallowClone()
        {
            return (Animal) this.MemberwiseClone();
        }

        public virtual Animal DeepClone()
        {
            Animal clone = (Animal) this.MemberwiseClone();
            clone.Owner = new Owner(Owner.Name, Owner.Surname);
            return clone;
        }

        public override string ToString()
        {
            return "Name: " + Name + ", age: " + Age + ", owner: " + Owner.Name + " " + Owner.Surname;
        }
    }
}
