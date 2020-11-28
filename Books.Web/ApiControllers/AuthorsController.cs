using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Books.Core.Entities;
using Books.Persistence;
using Books.Core.Contracts;
using Books.Core.DataTransferObjects;

namespace Books.Web.ApiControllers
{
    /// <summary>
    /// API to manage Authors application.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public AuthorsController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
        {
            return await _uow.Authors.GetAllDtosAsync();
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var author = await _uow.Authors.GetAuthorByIdAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            return author;
        }

        // PUT: api/Authors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, Author author)
        {
            if (id != author.Id)
            {
                return BadRequest();
            }
            _uow.Authors.UpdateAuthor(author);
            try
            {
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await AuthorExistsAsync(id)))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // POST: api/Authors
        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor(Author author)
        {
            await _uow.Authors.AddAuthorAsync(author);
            await _uow.SaveChangesAsync();

            return CreatedAtAction("GetAuthor", new { id = author.Id }, author);
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Author>> DeleteAuthor(int id)
        {
            var author = await _uow.Authors.GetAuthorByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _uow.Authors.DeleteAuthor(author);
            await _uow.SaveChangesAsync();

            return author;
        }

        private async Task<bool> AuthorExistsAsync(int id)
            => await _uow.Authors.IsExistingAuthorAsync(id)
            ;
    }
}
