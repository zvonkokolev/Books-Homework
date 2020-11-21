using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Books.Core.Validations
{
    public class IsbnValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string numbAsString = value as string;

            if (numbAsString.Length != 10)
            {
                return new ValidationResult($"Isbn {value} is invalid");
            }
            StringBuilder sb = new StringBuilder();
            int result = 0;
            if (numbAsString[9] == 'x' || numbAsString[9] == 'X')
            {
                for (int i = 0; i < 9; i++)
                {
                    sb.Append(numbAsString[i]);
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
                    sb.Append(numbAsString[i]);
                }
                int.TryParse(sb.ToString(), out int numberToValidate);
                for (int i = 1; i < 11; i++)
                {
                    result += numberToValidate % 10 * i;
                    numberToValidate /= 10;
                }
            }

            if(result % 11 != 0)
            {
                return new ValidationResult("Der IBAN ist nicht valid");
            }

            return ValidationResult.Success;
        }
    }
}
