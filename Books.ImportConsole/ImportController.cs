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

            string[][] matrix = notValidMatrix
                .Where(b => isbnValidation
                .IsValid(b[3]))
                .ToArray()
                ;
            Dictionary<Book, Author[]> keyValuePairs = new Dictionary<Book, Author[]>();
            foreach (var item in matrix)
            {
                keyValuePairs
                    .Add(new Book
                    {
                        Isbn = item[3],
                        Publishers = item[2],
                        Title = item[1]
                    }
                    ,
                     item[0]
                     .Split('~')
                    .Distinct()
                    .Select(a => new Author
                    {
                        Name = a
                    })
                    .ToArray()
                    );
            }
            Author[] authors = matrix
                .SelectMany(n => n[0].Split('~'))
                .Distinct()
                .Select(a => new Author
                {
                    Name = a
                })
                .ToArray()
                ;
            Book[] books = notValidMatrix
                .Select(b => new Book
                {
                    Title = b[1],
                    Publishers = b[2],
                    Isbn = b[3],
                    BookAuthors = keyValuePairs
                        .Select(ba => new BookAuthor
                        {
                            Book = ba.Key,
                            Author = ba.Value.FirstOrDefault()
                        })
                        .ToArray()
                })
                .ToArray()
                ;
            return books;
        }
    }
}
