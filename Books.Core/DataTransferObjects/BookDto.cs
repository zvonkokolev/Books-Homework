using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Books.Core.Entities;

namespace Books.Core.DataTransferObjects
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [DisplayName("Authoren")]
        public string Name { get; set; }
        public string Publishers { get; set; }
        public string Isbn { get; set; }
    }
}
