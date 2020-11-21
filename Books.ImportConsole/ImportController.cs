using Books.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils;
using Books.Core.Validations;

namespace Books.ImportConsole
{
    public static class ImportController
    {
        public static IEnumerable<Book> ReadBooksFromCsv()
        {
            string inputFile = "books.csv";
            string[][] notValidMatrix = MyFile.ReadStringMatrixFromCsv(inputFile, false);
            IsbnValidation isbnValidation = new IsbnValidation();

            string[][] matrix = notValidMatrix.Where(b => isbnValidation.IsValid(b[3])).ToArray();
            string[] authorOfBook = notValidMatrix.Select(b => b[0]).ToArray();

            string[] authorsNames = notValidMatrix.SelectMany(n => n[0].Split('~')).ToArray();

            Author[] authors = notValidMatrix
                .Select(a => new Author
                {
                    Name = authorOfBook.Where(n => n.Contains(a[0])).FirstOrDefault()
                })
                .ToArray()
                ;

            Book[] books = notValidMatrix.Select(b => new Book
            {
                Title = b[1],
                Publishers = b[2],
                Isbn = b[3]
            })
            .ToArray()
            ;
            return books;
        }
    }
}
