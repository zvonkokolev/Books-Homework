
using Books.Core.Contracts;
using Books.Core.DataTransferObjects;
using Books.Core.Entities;
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
    public class MainWindowViewModel : BaseViewModel
    {
        #region fields
        private Book _selectedBook;
        private Author _selectedAuthor;
        private ICommand _cmdEditBook;
        private ICommand _cmdDeleteBook;
        private string _searchText;
        //private Book[] _filteredBooks;
        private BookDto _selectedBookDto;
        private ObservableCollection<BookDto> _booksDto;

        //private BookDto[] _filteredBooksDto;
        #endregion
        #region properties
        public ObservableCollection<BookDto> BooksDto 
        {
            get => _booksDto;
            set
            {
                _booksDto = value;
                OnPropertyChanged(nameof(BooksDto));
            }
        }
        public ObservableCollection<Author> Authors { get; private set; }
        public ICommand CmdNewBook { get; set; }
        public ICommand CmdEditBook { get; set; }
        public ICommand CmdDeleteBook { get; set; }

        public BookDto SelectedBookDto
        {
            get { return _selectedBookDto; }
            set
            {
                _selectedBookDto = value;
                //EditMemberCommandText = $"Member {SelectedMember?.LastName} {SelectedMember?.FirstName} bearbeiten";
                OnPropertyChanged();
            }
        }

        public string SearchText 
        {
            get => _searchText;
            set
            {
                _searchText = value;
                _ = LoadBooks();
                OnPropertyChanged(nameof(SearchText));
            }
        }
        public Book SelectedBook 
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                OnPropertyChanged(nameof(SelectedBook));
            }
        }
        public Author SelectedAuthor
        {
            get => _selectedAuthor;
            set
            {
                _selectedAuthor = value;
                OnPropertyChanged(nameof(SelectedAuthor));
            }
        }
        #endregion
        public MainWindowViewModel() : base(null)
        {
        }

        public MainWindowViewModel(IWindowController windowController) : base(windowController)
        {
            LoadCommands();
        }

        private void LoadCommands()
        {
            CmdNewBook = new RelayCommand(
                execute: async _ =>
                {
                    var controller = new WindowController();
                    var viewModel = await BookEditCreateViewModel.Create(controller, null);
                    controller.ShowWindow(viewModel);
                }
                ,
                canExecute: _ => SelectedBookDto != null
                )
                ;
            CmdEditBook = new RelayCommand(
                execute: async _ =>
                {
                    await using IUnitOfWork unitOfWork = new UnitOfWork();
                    Book book = await unitOfWork.Books.GetBookByIdAsync(SelectedBookDto.Id);
                    var controller = new WindowController();
                    var viewModel = await BookEditCreateViewModel.Create(controller, book);
                    controller.ShowWindow(viewModel);
                }
                ,
                canExecute: _ => SelectedBookDto != null
                )
                ;
            CmdDeleteBook = new RelayCommand(
                  execute: async _ =>
                  {
                      await using IUnitOfWork unitOfWork = new UnitOfWork();
                      Book book = await unitOfWork.Books.GetBookByIdAsync(SelectedBookDto.Id);
                      unitOfWork.Books.DeleteBook(book);
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
                canExecute: _ => SelectedBookDto != null
                )
                ;
        }

        /// <summary>
        /// Lädt die gefilterten Buchdaten
        /// </summary>
        public async Task LoadBooks()
        {
            //Book[] books;
            BookDto[] booksDto;
            await using IUnitOfWork unitOfWork = new UnitOfWork();
            if (SearchText == null)
            {
                //books = await unitOfWork.Books.GetAllBooksAsync();
                booksDto = await unitOfWork.Books.GetAllBooksDtoAsync();
            }
            else
            {
                //books = await unitOfWork.Books.GetFilteredBooksAsync(SearchText);
                booksDto = await unitOfWork.Books.GetFilteredBooksDtoAsync(SearchText);
            }
            Author[] authors = await unitOfWork.Authors.GetAllAuthorsAsync();

            //Books = new ObservableCollection<Book>(books);
            //FilteredBooks = books;
            BooksDto = new ObservableCollection<BookDto>(booksDto);
            //FilteredBooksDto = booksDto;
            Authors = new ObservableCollection<Author>(authors);
            //SelectedBook = Books.FirstOrDefault();
            SelectedBookDto = BooksDto.FirstOrDefault();
            SelectedAuthor = Authors.FirstOrDefault();

        }

        public static async Task<BaseViewModel> Create(IWindowController controller)
        {
            var model = new MainWindowViewModel(controller);
            await model.LoadBooks();
            return model;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(BooksDto == null)
            {
                yield return new ValidationResult($"Bücher Datenbank ist fehlerhaft", new string[] { nameof(Books)});
            }
            if (Authors == null)
            {
                yield return new ValidationResult($"Authoren Datenbank ist fehlerhaft", new string[] { nameof(Authors) });
            }
        }
    }
}
