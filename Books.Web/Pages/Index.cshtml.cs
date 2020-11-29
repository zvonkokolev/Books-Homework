using Books.Core.Contracts;
using Books.Core.DataTransferObjects;
using Books.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _uow;

        public IndexModel(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IList<AuthorDto> Authors { get; set; }

        public async Task OnGetAsync()
        {
            Authors = await _uow.Authors.GetAllDtosAsync();
        }
    }
}
