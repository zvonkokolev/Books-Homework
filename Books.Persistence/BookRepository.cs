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
            => (await _dbContext.Books
            .Include(ba => ba.BookAuthors)
            .ToArrayAsync())
            .GroupBy(ba => ba.Title)
            .Select(d => new BookDto
            {
                Title = d.First().Title,
                BookAuthors = d.First().BookAuthors,
                Publishers = d.First().Publishers,
                Isbn = d.First().Isbn
            })
            .OrderByDescending(ba => ba.Title)
            .ToArray()
            ;
        public async Task<BookDto[]> GetFilteredBooksDtoAsync(string searchText)
            => (await _dbContext.Books
            .Include(ba => ba.BookAuthors)
            .Where(b => b.Title.StartsWith(searchText))
            .ToArrayAsync())
            .GroupBy(ba => ba.Title)
            .Select(d => new BookDto
            {
                Title = d.First().Title,
                BookAuthors = d.First().BookAuthors,
                Publishers = d.First().Publishers,
                Isbn = d.First().Isbn
            })
            .OrderByDescending(ba => ba.Title)
            .ToArray()
            ;
    }
}