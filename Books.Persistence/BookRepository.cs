using Books.Core.Contracts;
using Books.Core.DataTransferObjects;
using Books.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Persistence
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BookRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddBookAsync(Book book)
            => await _dbContext.Books.AddAsync(book)
            ;
        public async Task AddRangeAsync(IEnumerable<Book> books)
            => await _dbContext.AddRangeAsync(books)
            ;
        public async Task<Book[]> GetAllBooksAsync()
            => await _dbContext.Books
            .Include(ba => ba.BookAuthors)
            .OrderByDescending(ba => ba.Title)
            .ToArrayAsync()
            ;
        public async Task<Book[]> GetFilteredBooksAsync(string searchText)
            => await _dbContext.Books
            .Include(ba => ba.BookAuthors)
            .OrderByDescending(ba => ba.Title)
            .Where(b => b.Title.StartsWith(searchText))
            .ToArrayAsync()
            ;
        public async Task<BookDto[]> GetAllBooksDtoAsync()
            => await _dbContext.Books
              .Include(ba => ba.BookAuthors)
              .Select(d => new BookDto
              {
                  Id = d.Id,
                  Title = d.Title,
                  Authors = d.BookAuthors.Select(a => a.Author).ToList(),
                  Publishers = d.Publishers,
                  Isbn = d.Isbn
              })
              .OrderBy(ba => ba.Title)
              .ToArrayAsync()
              ;
        public async Task<BookDto[]> GetFilteredBooksDtoAsync(string searchText)
            => await _dbContext.Books
            .Include(ba => ba.BookAuthors)
            .Where(ba => ba.Title.StartsWith(searchText))
            .Select(d => new BookDto
            {
                Id = d.Id,
                Title = d.Title,
                Authors = d.BookAuthors.Select(a => a.Author).ToList(),
                Publishers = d.Publishers,
                Isbn = d.Isbn
            })
            .OrderBy(ba => ba.Title)
            .ToArrayAsync()
            ;
        public void DeleteBook(Book book)
            => _dbContext.Books.Remove(book)
            ;
        public void UpdateBook(Book book)
            => _dbContext.Update(book)
            ;
        public async Task<Book> GetBookByIdAsync(int id)
            => await _dbContext.Books
            .SingleOrDefaultAsync(b => b.Id == id)
            ;
        public async Task<bool> IsExistingBookAsync(int id)
            => await _dbContext.Books
            .AnyAsync(b => b.Id == id)
            ;
    }
}