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

            List<Book> books = new List<Book>();
            List<BookAuthor> bookAuthors = new List<BookAuthor>();
            Dictionary<string, Author> authors = new Dictionary<string, Author>();

            foreach (var item in notValidMatrix)
            {
                string[] authorsAll = item[0].Trim(' ').Split('~');
                Book book = new Book
                {
                    Title = item[1].Trim(' '),
                    Publishers = item[2].Trim(' '),
                    Isbn = item[3].Trim(' ')
                };
                foreach (var an in authorsAll)
                {
                    if (!authors.TryGetValue(an, out Author author))
                    {
                        author = new Author
                        {
                            Name = an.Trim(' ')
                        };
                        authors[an] = author;
                    }
                    BookAuthor bookAuthor = new BookAuthor
                    {
                        Book = book,
                        Author = author
                    };
                    book.BookAuthors.Add(bookAuthor);
                }
                books.Add(book);
            }

            return books;
        }
    }
}
