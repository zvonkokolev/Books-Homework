using Books.Core.Contracts;
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
            => await _dbContext.AddRangeAsync(books);

        public async Task<Book[]> GetAllBooksAsync()
            => await _dbContext.Books
            .Include(ba => ba.BookAuthors)
            .OrderByDescending(ba => ba.Title)
            .ToArrayAsync();

    }
}