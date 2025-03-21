using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject
{
    public class EBook : Book
    {
        private string FileFormat;
        public EBook(string id, string title, string author, string fileFormat) : base(id, title, author)
        {
            FileFormat = fileFormat;
        }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine(", typ: Ebook, format pliku: " + FileFormat);
        }
    }
}
