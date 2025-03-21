using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject
{
    public class Library : IBookOperations
    {
        private List<Book> ListOfBooks = new List<Book> { };
        private List<Reader> ListOfReaders = new List<Reader> { };
        public void ListAvailableBooks()
        {
            int availableBooksCount = 0;
            if(ListOfBooks.Count == 0)
            {
                Console.WriteLine("Brak książek!");
                return;
            }
            foreach (Book book in ListOfBooks){
                if (book.IsAvailable)
                {
                    book.DisplayInfo();
                    availableBooksCount++;
                }
            }
            if(availableBooksCount == 0)
            {
                Console.WriteLine("Brak dostępnych książek!");
            }
        }
        public void AddBook(Book book)
        {
            ListOfBooks.Add(book);
        }

        public void RegisterReader(Reader reader)
        {
            ListOfReaders.Add(reader);
        }
        public void ListAllReaders()
        {
            if (ListOfReaders.Count == 0)
            {
                Console.WriteLine("Brak czytelników!");
                return;
            }
            foreach (Reader reader in ListOfReaders)
            {
                reader.displayInfo();
            }
        }

        public bool BorrowBook(string bookId, Reader reader)
        {
            Book? book = ListOfBooks.Find(b => b.Id == bookId);
            try
            {
                if (!ListOfReaders.Contains(reader))
                {
                    throw new ArgumentException("Czytelnik nie jest zarejestrowany w bibliotece");
                }
                if (book == null)
                {
                    throw new ArgumentException("Książka o podanym ID nie istnieje");
                }

                if (!book.IsAvailable)
                {
                    throw new ArgumentException("Książka o podanym ID została już pożyczona");
                }
            }
            catch (Exception ex) {
                Console.Error.WriteLine(ex.Message);
                return false;
            }

            book.returnOrBorrowBook();
            return true;
        }

        public bool ReturnBook(string bookId, Reader reader)
        {
            Book? book = ListOfBooks.Find(b => b.Id == bookId);

            try
            {
                if (!ListOfReaders.Contains(reader))
                {
                    throw new ArgumentException("Czytelnik nie jest zarejestrowany w bibliotece");
                }
                if (book == null)
                {
                    throw new ArgumentException("Książka o podanym ID nie istnieje");
                }

                if (book.IsAvailable)
                {
                    throw new ArgumentException("Książka o podanym ID nie została pożyczona");
                }
            }
            catch (Exception ex) {
                Console.Error.WriteLine(ex.Message);
                return false;
            }


            book.returnOrBorrowBook();
            return true;
        }
    }
}
