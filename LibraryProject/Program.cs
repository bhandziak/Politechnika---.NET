namespace LibraryProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Library library = new Library();

            // Dodanie książek do biblioteki
            Book book1 = new Book("C# Programming", "John Doe", "12345");
            Book book2 = new Book("Design Patterns", "Gamma et al.", "67890");
            EBook ebook1 = new EBook("C++ Tutorial", "John Right", "33214", "PDF");

            library.AddBook(book1);
            library.AddBook(book2);
            library.AddBook(ebook1);

            library.ListAvailableBooks();

            // Rejestracja czytelnika
            Reader reader = new Reader(1, "Alice", "alice@example.com");
            library.RegisterReader(reader);
            library.ListAllReaders();

            // Wypożyczenie książki
            if (library.BorrowBook("12345", reader))
            {
                Console.WriteLine("Book borrowed successfully.");
            }
            else
            {
                Console.WriteLine("Book is not available.");
            }

            // Zwrot książki
            if (library.ReturnBook("12345", reader))
            {
                Console.WriteLine("Book returned successfully.");
            }
            else
            {
                Console.WriteLine("Failed to return book.");
            }
        }
    }
}
