﻿using Books.Core.Contracts;
using Books.Core.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Books.Web.Pages.Authors
{
  public class DetailsModel : PageModel
  {
    private readonly IUnitOfWork _uow;

    public AuthorDto AuthorInfo { get; set; }

    public DetailsModel(IUnitOfWork uow)
    {
      _uow = uow;
    }

    public IActionResult OnGet(int? id)
    {
      return Page();
    }
  }
}
