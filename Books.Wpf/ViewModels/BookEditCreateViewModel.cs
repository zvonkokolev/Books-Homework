using Books.Core.Contracts;
using Books.Core.DataTransferObjects;
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
using System.Windows.Input;

namespace Books.Wpf.ViewModels
{
    public class BookEditCreateViewModel : BaseViewModel
    {
        private Author _selectedAuthor;
        private string _title;
        private string[] _publishers;
        private string _isbn;
        private ObservableCollection<Author> _authors;
        private ObservableCollection<AuthorDto> _authorsDtos;
        private AuthorDto _selectedAuthorDto;

        public string WindowTitle { get; set; }
        public Book SelectedBook { get; set; }
        public ObservableCollection<Author> Authors
        {
            get => _authors;
            set
            {
                _authors = value;
                OnPropertyChanged(nameof(Authors));
            }
        }
        public ObservableCollection<AuthorDto> AuthorsDtos 
        {
            get => _authorsDtos;
            set
            {
                _authorsDtos = value;
                OnPropertyChanged(nameof(AuthorsDtos));
            }
        }
        public ICommand CmdAddNewAuthor { get; set; }
        public ICommand CmdDeleteAuthor { get; set; }

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

        public string[] Publishers
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

        public AuthorDto SelectedAuthorDto 
        {
            get => _selectedAuthorDto;
            set
            {
                _selectedAuthorDto = value;
                OnPropertyChanged(nameof(SelectedAuthorDto));
            }
        }

        public BookEditCreateViewModel() : base(null)
        {
        }

        public BookEditCreateViewModel(IWindowController windowController, Book book) : base(windowController)
        {
            if (book == null)
            { WindowTitle = "Neuer Buch anlegen"; }
            else
            { WindowTitle = "Buch bearbeiten"; }
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
            List<AuthorDto> authorDtos = await unitOfWork
                .Authors
                .GetAllDtosAsync()
                ;
            AuthorsDtos = new ObservableCollection<AuthorDto>(authorDtos);
            Publishers = authorDtos.Select(p => p.Publishers).ToArray();
            Author[] authors = await unitOfWork.Authors.GetAllAuthorsAsync();
            Authors = new ObservableCollection<Author>(authors);
            if(SelectedBook != null)
            {
                int index = SelectedBook.BookAuthors.Select(a => a.AuthorId).FirstOrDefault();
                SelectedAuthorDto = await unitOfWork.Authors.GetDtoByIdAsync(index);
            }
        }

        private void LoadCommands()
        {
            CmdAddNewAuthor = new RelayCommand(
                execute: async _ =>
                {
                    await using IUnitOfWork unitOfWork = new UnitOfWork();
                    Author author = new Author
                    {
                        Name = SelectedAuthor.Name
                    };
                    await unitOfWork.Authors.AddAuthorAsync(author);
                    try
                    {
                        await unitOfWork.SaveChangesAsync();
                    }
                    catch (ValidationException e)
                    {
                        throw new ValidationException($"{e.Message}");
                    }
                }
                ,
                canExecute: _ => CmdAddNewAuthor != null
                );
            CmdDeleteAuthor = new RelayCommand(
                execute: async _ =>
                {
                    await using IUnitOfWork unitOfWork = new UnitOfWork();
                    Author author = await unitOfWork.Authors.GetAuthorByIdAsync(SelectedAuthor.Id);
                    unitOfWork.Authors.DeleteAuthor(author);
                    try
                    {
                        await unitOfWork.SaveChangesAsync();
                    }
                    catch (ValidationException e)
                    {
                        throw new ValidationException($"{e.Message}");
                    }
                }
                ,
                canExecute: _ => CmdDeleteAuthor != null
                );
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return ValidationResult.Success;
        }

    }
}
