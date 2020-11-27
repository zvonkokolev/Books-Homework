using Books.Core.DataTransferObjects;
using Books.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Books.Core.Contracts
{
    public interface IAuthorRepository
    {
        Task<Author[]> GetAllAuthorsAsync();
        Task<IEnumerable<AuthorDto>> GetAllDtosAsync();
        Task<AuthorDto> GetDtoByIdAsync(int authorId);
        void Add(Author author);
        bool IsDuplicateAuthor(Author author);
    }
}
