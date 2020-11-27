using Books.Core.Contracts;
using Books.Core.Entities;
using Books.Core.Validations;
using Books.Persistence;
using Books.Wpf.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Wpf.ViewModels
{
    public class BookEditCreateViewModel : BaseViewModel
    {
        private Author _selectedAuthor;
        private string _title;
        private string _publishers;
        private string _isbn;

        public string WindowTitle { get; set; }
        public Book SelectedBook { get; set; }
        public ObservableCollection<Author> Authors { get; set; }

        public Author SelectedAuthor 
        {
            get => _selectedAuthor;
            set
            {
                _selectedAuthor = value;
                OnPropertyChanged(nameof(SelectedAuthor));
                //Validate();
            }
        }
        [MinLength(1, ErrorMessage = "Titel muss angegeben werden")]
        public string Title 
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
                //Validate();
            }
        }
        public string Publishers 
        {
            get => _publishers;
            set
            {
                _publishers = value;
                OnPropertyChanged(nameof(Publishers));
                //Validate();
            }
        }
        [IsbnValidation]
        public string Isbn 
        {
            get => _isbn;
            set
            {
                _isbn = value;
                OnPropertyChanged(nameof(Isbn));
                //Validate();
            }
        }
        public BookEditCreateViewModel() : base(null)
        {
        }

        public BookEditCreateViewModel(IWindowController windowController, Book book) : base(windowController)
        {
            SelectedBook = book;
            LoadCommands();
           
        }

        public static async Task<BaseViewModel> Create(WindowController controller, Book book)
        {
            var model = new BookEditCreateViewModel(controller, book);
            await model.LoadData();
            return model;
        }

        private async Task LoadData()
        {
            await using IUnitOfWork unitOfWork = new UnitOfWork();
            Author[] authors = await unitOfWork.Authors.GetAllAuthorsAsync();
            Authors = new ObservableCollection<Author>(authors);
            SelectedAuthor = Authors.FirstOrDefault();
        }

        private void LoadCommands()
        {
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return ValidationResult.Success;
        }

    }
}
