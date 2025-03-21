using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LibraryProject
{
    public class Reader
    {
        private int Id;
        private string Name;
        private string Email;
        public Reader(int id, string name, string email) {
            this.Id = id;
            this.Name = name;
            if (ValidateEmail(email)) {
                this.Email = email;
            }
            else
            {
                throw new ArgumentException("Podany email jest nieprawidłowy");
            }
            
        }

        private bool ValidateEmail(string email)
        {
            string emailRegex = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            return Regex.IsMatch(email, emailRegex);   
        }

        public void displayInfo()
        {
            Console.WriteLine("Id: " + Id + ", Imie: " + Name + ", Email: " + Email);
        }
    }
}
