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
            => _dbContext = dbContext;

        public void Add(Author author)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<AuthorDto>> GetAllDtosAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<AuthorDto> GetDtoByIdAsync(int authorId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Author[]> GetAllAuthorsAsync()
            => await _dbContext.Authors
            .Include(ba => ba.BookAuthors)
            .OrderByDescending(ba => ba.Name)
            .ToArrayAsync()
            ;
        public bool IsDuplicateAuthor(Author author)
            => _dbContext.Authors
            .Any(a => a.Id == author.Id && a.Name.Equals(author.Name))
            ;
    }
}