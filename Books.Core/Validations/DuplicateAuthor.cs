using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Books.Core.Contracts;
using Books.Core.Entities;
using Books.Core.Validations;

namespace Books.Core.Validations
{
    public class DuplicateAuthor : ValidationAttribute
    {
        private IUnitOfWork _uow;

        public DuplicateAuthor(IUnitOfWork uow) : base()
        {
            _uow = uow;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(!(value is Author author))
            {
                throw new ArgumentException("Der Objekt ist kein Author", nameof(value));
            }
            bool check =  _uow.Authors.IsDuplicateAuthor(author);
            if (check)
            {
                return new ValidationResult("Es existiert bereits ein Author mit dem Namen");
            }
            return ValidationResult.Success;
        }

    }
}
