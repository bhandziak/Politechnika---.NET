using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype_Project
{
    public class Owner
    {
        public string Name { get; set;}
        public string Surname { get; set;}

        public Owner(string name, string surname)
        {
            Name = name;
            Surname = surname;
        }
    }
}
