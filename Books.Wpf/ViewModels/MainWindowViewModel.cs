
using Books.Core.Contracts;
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
        private Book _selectedBook;
        private Author _selectedAuthor;
        private ICommand _cmdEditBook;
        private ICommand _cmdDeleteBook;
        private string _searchText;
        private Book[] _filteredBooks;

        public ObservableCollection<Book> Books { get; private set; }
        public ObservableCollection<Author> Authors { get; private set; }
        public ICommand CmdNewBook { get; set; }

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
        public Book[] FilteredBooks 
        {
            get => _filteredBooks;
            set
            {
                _filteredBooks = value;
                OnPropertyChanged(nameof(FilteredBooks));
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
                execute: _ =>
                {
                    var window = new BookEditCreateViewModel(Controller, SelectedBook);
                    window.Controller.ShowWindow(window, true);
                }
                ,
                canExecute: _ => SelectedBook != null
                );
        }

        /// <summary>
        /// Lädt die gefilterten Buchdaten
        /// </summary>
        public async Task LoadBooks()
        {
            Book[] books;
            await using IUnitOfWork unitOfWork = new UnitOfWork();
            if (SearchText == null)
            {
                books = await unitOfWork.Books.GetAllBooksAsync();
            }
            else
            {
                books = await unitOfWork.Books.GetFilteredBooksAsync(SearchText);
            }
            Author[] authors = await unitOfWork.Authors.GetAllAuthorsAsync();

            Books = new ObservableCollection<Book>(books);
            FilteredBooks = books;
            Authors = new ObservableCollection<Author>(authors);

            SelectedBook = Books.FirstOrDefault();
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
            if(Books == null)
            {
                yield return new ValidationResult($"Bücher Datenbank ist fehlerhaft", new string[] { nameof(Books)});
            }
            if (Authors == null)
            {
                yield return new ValidationResult($"Authoren Datenbank ist fehlerhaft", new string[] { nameof(Authors) });
            }
        }

        // commands
        //public ICommand CmdNewBook 
        //{
        //    get
        //    {
        //        if (_cmdNewBook == null)
        //        {
        //            _cmdNewBook = new RelayCommand(
        //                execute: _ =>
        //                {
        //                    var window = new BookEditCreateViewModel(Controller, SelectedBook);
        //                    window.Controller.ShowWindow(window, true);
        //                }
        //                ,
        //                canExecute: _ => SelectedBook != null
        //                );

        //        }
        //        return _cmdNewBook;
        //    }
        //    set
        //    {
        //        _cmdNewBook = value;
        //    }
        //}
        public ICommand CmdEditBook
        {
            get
            {
                if (_cmdEditBook == null)
                {
                    _cmdEditBook = new RelayCommand(
                        execute: _ =>
                        {
                            var window = new BookEditCreateViewModel(Controller, SelectedBook);
                            window.Controller.ShowWindow(window, true);
                        }
                        ,
                        canExecute: _ => SelectedBook != null
                        );
                }
                return _cmdEditBook;
            }
            set
            {
                _cmdEditBook = value;
            }
        }
        public ICommand CmdDeleteBook
        {
            get
            {
                if(_cmdDeleteBook == null)
                {
                    _cmdDeleteBook = new RelayCommand(
                          execute: _ =>
                          {
                              var window = new BookEditCreateViewModel(Controller, SelectedBook);
                              window.Controller.ShowWindow(window, true);
                          }
                        ,
                        canExecute: _ => SelectedBook != null
                        );
                }
                return _cmdDeleteBook;
            } 
            set
            {
                _cmdDeleteBook = value;
            }
        }
    }
}
