using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject
{
    interface IBookOperations
    {
        bool BorrowBook(string bookId, Reader reader);
        bool ReturnBook(string bookId, Reader reader);
    }
}
