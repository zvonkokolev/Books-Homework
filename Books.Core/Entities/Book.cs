using Books.Core.Validations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Books.Core.Entities
{
    public class Book : EntityObject, IValidatableObject
    {

        public ICollection<BookAuthor> BookAuthors { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Titel muss definiert sein")]
        [MaxLength(200, ErrorMessage = "Titel darf maximal 200 Zeichen lang sein")]
        public string Title { get; set; }

        [Required, MaxLength(100)]
        public string Publishers { get; set; }

        [IsbnValidation]
        [Required, MaxLength(13)]
        public string Isbn { get; set; }

        public Book()
        {
            BookAuthors = new List<BookAuthor>();
        }

        /// <summary>
        /// Eine gültige ISBN-Nummer besteht aus den Ziffern 0, ... , 9,
        /// 'x' oder 'X' (nur an der letzten Stelle)
        /// Die Gesamtlänge der ISBN beträgt 10 Zeichen.
        /// Für die Ermittlung der Prüfsumme werden die Ziffern 
        /// von rechts nach links mit 1 - 10 multipliziert und die 
        /// Produkte aufsummiert. Ist das rechte Zeichen ein x oder X
        /// wird als Zahlenwert 10 verwendet.
        /// Die Prüfsumme muss modulo 11 0 ergeben.
        /// </summary>
        /// <returns>Prüfergebnis</returns>
        public static bool CheckIsbn(string isbn)
        {
            if (isbn.Length != 10 || 
                string.IsNullOrWhiteSpace(isbn) ||
                string.IsNullOrEmpty(isbn))
            {
                return false;
            }
            StringBuilder sb = new StringBuilder();
            int result = 0;
            if (isbn[9] == 'x' || isbn[9] == 'X')
            {
                for (int i = 0; i < 9; i++)
                {
                    sb.Append(isbn[i]);
                }
                int.TryParse(sb.ToString(), out int numberToValidate);
                result += 10;
                for (int i = 2; i < 11; i++)
                {
                    result += numberToValidate % 10 * i;
                    numberToValidate /= 10;
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    sb.Append(isbn[i]);
                }
                int.TryParse(sb.ToString(), out int numberToValidate);
                for (int i = 1; i < 11; i++)
                {
                    result += numberToValidate % 10 * i;
                    numberToValidate /= 10;
                }
            }

            return result % 11 != 0;
        }

        public override string ToString()
        {
            return $"{Title} {Isbn} mit {BookAuthors.Count()} Autoren";
        }

        /// <inheritdoc />
        /// <summary>
        /// Jedes Buch muss zumindest einen Autor haben.
        /// Weiters darf ein Autor einem Buch nicht mehrfach zugewiesen
        /// werden.
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(BookAuthors.Count < 1 || BookAuthors == null)
            {
                yield return new ValidationResult(
                    "Buch muss mindestens einen Author haben", 
                    new string[] { nameof(BookAuthors)});
            }
            //foreach (var item in BookAuthors)
            //{
                
            //}
        }
    }
}

