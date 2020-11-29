using Books.Core.Contracts;
using Books.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Books.Web.Pages.Authors
{
    public class CreateModel : PageModel
    {
        private readonly IUnitOfWork _uow;

        [BindProperty]
        public Author Author { get; set; }

        public CreateModel(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        // POST: Authors/Create
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _uow.Authors.AddAuthorAsync(Author);
            await _uow.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
