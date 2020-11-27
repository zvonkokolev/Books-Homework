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
        Task<List<AuthorDto>> GetAllDtosAsync();
        Task<AuthorDto> GetDtoByIdAsync(int authorId);
        Task AddAuthorAsync(Author author);
        bool IsDuplicateAuthor(Author author);
        Task<Author> GetAuthorByIdAsync(int id);
        void DeleteAuthor(Author author);
    }
}
