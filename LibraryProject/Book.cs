using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject
{
    public class Book
    {
        public string Id { get; }
        public string Title { get; }
        public string Author { get; }
        public bool IsAvailable { get; private set; }

        public Book(string title, string author, string id)
        {
            Id = id;
            Title = title;
            Author = author;
            IsAvailable = true;
        }

        public void returnOrBorrowBook()
        {
            IsAvailable = !IsAvailable;
        }

        public virtual void DisplayInfo()
        {
            Console.WriteLine("Id: " + Id + ", tytuł: " + Title + ", autor: " + Author);
        }
    }
}
