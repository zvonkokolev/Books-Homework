using Books.Core.DataTransferObjects;
using Books.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Books.Core.Contracts
{
    public interface IBookRepository
    {
        Task AddBookAsync(Book book);
        Task AddRangeAsync(IEnumerable<Book> books);
        Task<Book[]> GetAllBooksAsync();
        Task<Book[]> GetFilteredBooksAsync(string searchText);
        Task<BookDto[]> GetAllBooksDtoAsync();
        Task<BookDto[]> GetFilteredBooksDtoAsync(string searchText);
        void DeleteBook(Book book);
        void UpdateBook(Book book);
        Task<Book> GetBookByIdAsync(int id);
        Task<bool> IsExistingBookAsync(int id);
    }
}