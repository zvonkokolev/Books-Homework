using Books.Core.Contracts;
using Books.Core.DataTransferObjects;
using Books.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Books.Web.ApiControllers
{
    /// <summary>
    /// API to managed the Books application.
    /// </summary>
    [Produces("application/json")]
    [Route("api/books")]
    public class BooksController : Controller
    {
        private readonly IUnitOfWork _uow;

        public BooksController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            return await _uow.Books.GetAllBooksDtoAsync();
        }

        /// <summary>
        /// Bind books by title.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        // GET: api/books/<title>
        [Route("{title}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooksByTitel(string title)
        {
            return await _uow.Books.GetFilteredBooksDtoAsync(title);
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBookById(int id)
        {
            var book = await _uow.Books.GetBookByIdAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/Books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }
            _uow.Books.UpdateBook(book);
            try
            {
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await BookExistsAsync(id)))
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

        // POST: api/Books
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            await _uow.Books.AddBookAsync(book);
            await _uow.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // DELETE: api/Book/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> DeleteBook(int id)
        {
            var book = await _uow.Books.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _uow.Books.DeleteBook(book);
            await _uow.SaveChangesAsync();

            return book;
        }
        private async Task<bool> BookExistsAsync(int id)
            => await _uow.Books.IsExistingBookAsync(id)
            ;
    }
}
