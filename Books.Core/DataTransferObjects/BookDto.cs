using System;
using System.Collections.Generic;
using System.Text;
using Books.Core.Entities;

namespace Books.Core.DataTransferObjects
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<BookAuthor> BookAuthors { get; set; }
        public string Publishers { get; set; }
        public string Isbn { get; set; }
    }
}
