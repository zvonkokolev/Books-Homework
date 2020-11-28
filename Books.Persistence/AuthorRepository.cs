using Books.Core.Contracts;
using Books.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.Core.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace Books.Persistence
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AuthorRepository(ApplicationDbContext dbContext)
            => _dbContext = dbContext
            ;
        public async Task AddAuthorAsync(Author author)
            => await _dbContext.Authors.AddAsync(author)
            ;
        public async Task<List<AuthorDto>> GetAllDtosAsync()
            => await _dbContext.Authors
            .Include(ba => ba.BookAuthors)
            .OrderBy(ba => ba.Name)
            .Select(a => new AuthorDto
            {
                Id = a.Id,
                Author = a.Name,
                BookCount = a.BookAuthors.Count(),
                Books = a.BookAuthors.Select(b => b.Book).ToList(),
                Publishers = a.BookAuthors
                    .Select(p => p.Book.Publishers)
                    .FirstOrDefault()
            })
            .ToListAsync()
            ;
        public async Task<Author> GetAuthorByIdAsync(int authorId)
            => await _dbContext.Authors
                .FindAsync(authorId)
            ;
        public async Task<AuthorDto> GetDtoByIdAsync(int authorId)
            => await _dbContext.Authors
            .Include(ba => ba.BookAuthors)
            .Where(ba => ba.Id == authorId)
            .Select(a => new AuthorDto
            {
                Id = authorId,
                Author = a.Name,
                BookCount = a.BookAuthors.Count(),
                Publishers = a.BookAuthors.Select(p => p.Book.Publishers).FirstOrDefault()
            })
            .FirstOrDefaultAsync()
            ;
        public async Task<Author[]> GetAllAuthorsAsync()
            => await _dbContext.Authors
            //.Include(ba => ba.BookAuthors)
            .OrderBy(ba => ba.Name)
            .ToArrayAsync()
            ;
        public bool IsDuplicateAuthor(Author author)
            => _dbContext.Authors
            .Any(a => a.Id == author.Id && a.Name.Equals(author.Name))
            ;
        public void DeleteAuthor(Author author)
            => _dbContext.Authors.Remove(author)
            ;
        public void UpdateAuthor(Author author)
            => _dbContext.Update(author)
            ;
        public async Task<bool> IsExistingAuthorAsync(int id)
            => await _dbContext.Authors
            .AnyAsync(a => a.Id == id)
            ;
    }
}