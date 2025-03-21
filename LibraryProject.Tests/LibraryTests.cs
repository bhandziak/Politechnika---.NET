namespace LibraryProject.Tests
{
    public class Tests
    {
        [Test]
        public void BorrowNonExistentBook_ReturnsFalse()
        {
            Library library = new Library();
            Book book1 = new Book("C# Programming", "John Doe", "12345");
            library.AddBook(book1);

            Reader reader = new Reader(1, "Alice", "alice@example.com");
            library.RegisterReader(reader);

            bool res = library.BorrowBook("444", reader);
            Assert.IsFalse(res);
        }

        [Test]
        public void BorrowByNotRegisteredReader_ReturnsFalse()
        {
            Library library = new Library();
            Book book1 = new Book("C# Programming", "John Doe", "12345");

            library.AddBook(book1);

            Reader reader = new Reader(1, "Alice", "alice@example.com");

            bool res = library.BorrowBook("12345", reader);
            Assert.IsFalse(res);
        }
        [Test]
        public void BorrowAlreadyBorrowedBook_ReturnsFalse()
        {
            Library library = new Library();
            Book book1 = new Book("C# Programming", "John Doe", "12345");

            library.AddBook(book1);

            Reader reader = new Reader(1, "Alice", "alice@example.com");
            Reader reader2 = new Reader(2, "John", "john@example.com");
            library.RegisterReader(reader);
            library.RegisterReader(reader2);


            library.BorrowBook("12345", reader);

            bool res = library.BorrowBook("12345", reader2);
            Assert.IsFalse(res);
        }
        [Test]
        public void BorrowAvailableBookByRegisteredReader_ReturnsTrue()
        {
            Library library = new Library();
            Book book1 = new Book("C# Programming", "John Doe", "12345");

            library.AddBook(book1);

            Reader reader = new Reader(1, "Alice", "alice@example.com");
            library.RegisterReader(reader);

            bool res = library.BorrowBook("12345", reader);

            Assert.IsTrue(res);
        }

        [Test]
        public void BorrowAvailableEBook_ReturnsTrue()
        {
            Library library = new Library();
            EBook ebook1 = new EBook("C++ Tutorial", "John Right", "33214", "PDF");

            library.AddBook(ebook1);

            Reader reader = new Reader(1, "Alice", "alice@example.com");
            library.RegisterReader(reader);

            bool res = library.BorrowBook("33214", reader);

            Assert.IsTrue(res);
        }


        [Test]
        public void ReturnNonExistentBook_ReturnsFalse()
        {
            Library library = new Library();
            Book book1 = new Book("C# Programming", "John Doe", "12345");
            library.AddBook(book1);

            Reader reader = new Reader(1, "Alice", "alice@example.com");
            library.RegisterReader(reader);

            bool res = library.ReturnBook("444", reader);
            Assert.IsFalse(res);
        }

        [Test]
        public void ReturnByNotRegisteredReader_ReturnsFalse()
        {
            Library library = new Library();
            Book book1 = new Book("C# Programming", "John Doe", "12345");

            library.AddBook(book1);

            Reader reader = new Reader(1, "Alice", "alice@example.com");

            bool res = library.ReturnBook("12345", reader);
            Assert.IsFalse(res);
        }
        [Test]
        public void ReturnNotBorrowedBook_ReturnsFalse()
        {
            Library library = new Library();
            Book book1 = new Book("C# Programming", "John Doe", "12345");

            library.AddBook(book1);

            Reader reader = new Reader(1, "Alice", "alice@example.com");

            bool res = library.ReturnBook("12345", reader);
            Assert.IsFalse(res);
        }
        [Test]
        public void ReturnBookByRegisteredReader_ReturnsTrue()
        {
            Library library = new Library();
            Book book1 = new Book("C# Programming", "John Doe", "12345");

            library.AddBook(book1);

            Reader reader = new Reader(1, "Alice", "alice@example.com");
            library.RegisterReader(reader);

            library.BorrowBook("12345", reader);

            bool res = library.ReturnBook("12345", reader);

            Assert.IsTrue(res);
        }
        [Test]
        public void ReturnEBook_ReturnsTrue()
        {
            Library library = new Library();
            EBook ebook1 = new EBook("C++ Tutorial", "John Right", "33214", "PDF");

            library.AddBook(ebook1);

            Reader reader = new Reader(1, "Alice", "alice@example.com");
            library.RegisterReader(reader);

            library.BorrowBook("33214", reader);
            bool res = library.ReturnBook("33214", reader);

            Assert.IsTrue(res);
        }
    }
}