using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using Common.Models;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Services;
using Microsoft.EntityFrameworkCore;

namespace BookStoreUI.Views
{
    /// <summary>
    /// Interaction logic for BookManagerView.xaml
    /// </summary>
    public partial class BookManagerView : UserControl
    {

        public MainWindowContext MainWindowContext { get; set; }

        private readonly BookRepository _bookRepository;
        private readonly AuthorRepository _authorRepository;
        private readonly PublisherRepository _publisherRepository;
        private readonly BookDetailRepository _bookDetailRepository;

        

        public BookManagerView()
        { 
            InitializeComponent();

            AuthorRepository.AuthorChanged += AuthorRepository_AuthorChanged;
            PublisherRepository.PublisherChanged += PublisherRepository_PublisherChanged;
            


            MainWindowContext = new MainWindowContext();
            DataContext = MainWindowContext;
           

            _bookRepository = new BookRepository(ApplicationContextManager.Context);
            _authorRepository = new AuthorRepository(ApplicationContextManager.Context);
            _publisherRepository = new PublisherRepository(ApplicationContextManager.Context);
            _bookDetailRepository = new BookDetailRepository(ApplicationContextManager.Context);



            BookList.SelectionChanged += BookList_OnSelectionChanged;




            //-------------- SKRIVER UT ALLA BÖCKER I LISTVYN

            var bookList = _bookRepository.GetAllBooks();

            foreach (var book in bookList)
            {
                this.BookList.Items.Add($"{book.Title}");
            }



            //---------------Adderar Författare till ComboBoxen---------------------------

            if (BookList.SelectedItem == null)
            {
                AuthorListBox.ItemsSource = _authorRepository.GetAllAuthors();

            }

            //----------------Adderar alla förläggare till comboboxen---------------------
            if (BookList.SelectedItem == null)
            {
                //PublisherComboBox.ItemsSource = _publisherRepository.GetAllPublishers();

                var publishers = _publisherRepository.GetAllPublishers(); 
                PublisherComboBox.ItemsSource = publishers;
                PublisherComboBox.DisplayMemberPath = "Name";
                PublisherComboBox.SelectedValuePath = "PublisherId";
            }

        }



        private void PublisherRepository_PublisherChanged()
        {
            if (PublisherComboBox.SelectedItem == null)
            {
                PublisherComboBox.ItemsSource = _publisherRepository.GetAllPublishers();
            }
        }

        private void AuthorRepository_AuthorChanged()
        {
            
            if (BookList.SelectedItem == null)
            {
                AuthorListBox.ItemsSource = _authorRepository.GetAllAuthors();

            }

        }

        private void BookList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (BookStoreDbContext context = new BookStoreDbContext())
            {

                if (BookList.SelectedItem != null)
                {

                    var selectedBook = BookList.SelectedItem;
                    

                    var book = context.Books
                        .Include(b => b.BookDetails)
                        .ThenInclude(bd => bd.Author)
                        .Include(b => b.BookDetails)
                        .ThenInclude(bd => bd.Publisher)
                        .Single(b => b.Title == selectedBook);


                    if (book != null)
                    {
                        var authors = book.BookDetails.Select(bd => bd.Author).Distinct();
                        var publishers = book.BookDetails.Select(bd => bd.Publisher).Distinct();



                        IsbnTextBox.Text = book.Isbn13;
                        TitleTextBox.Text = book.Title;
                        FormatTextBox.Text = book.Format;
                        PriceTextBox.Text = book.Price.ToString();
                        ReleaseDatePicker.Text = book.ReleaseDate.ToString();
                        TranslatorTextBox.Text = book.Translater;
                        PagesTextBox.Text = book.Pages.ToString();


                        IsbnTextBlock.Text = book.Isbn13;
                        TitleTextBlock.Text = book.Title;
                        LanguageTextBlock.Text = book.Language;
                        FormatTextBlock.Text = book.Format;
                        PriceTextBlock.Text = book.Price.ToString();
                        DateTextBlock.Text = book.ReleaseDate.ToString();
                        TranslatorTextBlock.Text = book.Translater;
                        PagesTextBlock.Text = book.Pages.ToString();
                        AuthorTextBlock.Text = string.Join(", ", authors.Select(a => $"{a.FirstName} {a.LastName}"));
                        PublisherTextBlock.Text = string.Join(", ", publishers.Select(p => p.Name));
                    }
                }
            }
        }

        private void AddBookBtn_OnClick(object sender, RoutedEventArgs e)
        {


           var Isbn13 = IsbnTextBox.Text;

           if (_bookRepository.BookExists(Isbn13))
           {
               MessageBox.Show("A book with the given ISBN already exists, if you want to edit the book, please press 'Update'");
               return;
           }


           var title = TitleTextBox.Text;
           var language = LanguageComboBox.Text;
           var format = FormatTextBox.Text;
           
           decimal Price;
           if (!decimal.TryParse(PriceTextBox.Text, out Price))
           {
               MessageBox.Show("Non valid price format");
               return;
           }

           DateTime releaseDate = ReleaseDatePicker.DisplayDate;
           DateOnly releaseDateOnly = DateOnly.FromDateTime(releaseDate);

           var translator = TranslatorTextBox.Text;
           
           int Pages;
           if (!int.TryParse(PagesTextBox.Text, out Pages))
           {
               MessageBox.Show("Pages must be entered in numbers");
               return;
           }


           var selectedAuthors = AuthorListBox.SelectedItems.Cast<AuthorModel>().ToList();

           var selectedPublisher = PublisherComboBox.SelectedItem as PublisherModel;

        
           var authorDetails = selectedAuthors.Select(author => new BookDetailModel
           {
               Isbn = Isbn13,
               AuthorId = author.AuthorId,
               PublisherId = selectedPublisher?.PublisherId ?? 0,
           }).ToList();

           BookModel book = new BookModel
           {
               Isbn13 = Isbn13,
               Title = title,
               Language = language,
               Format = format,
               Price = Price,
               ReleaseDate = releaseDateOnly,
               Translater = translator,
               Pages = Pages,
               BookDetails = authorDetails,
           };

            _bookRepository.AddBook(book);
            var bookList = _bookRepository.GetAllBooks();

            BookList.Items.Clear();
            
            foreach (var books in bookList)
            {
                this.BookList.Items.Add($"{books.Title}");
            }

            ClearBoxes();
        }





        private void UpdateBookBtn_OnClick(object sender, RoutedEventArgs e)
        {

            var Isbn13 = IsbnTextBox.Text;

            if (!_bookRepository.BookExists(Isbn13))
            {
                MessageBox.Show("A book with this ISBN does not exist");
                return;
            }


            var title = TitleTextBox.Text;
            var language = LanguageComboBox.Text;
            var format = FormatTextBox.Text;

            decimal Price;
            if (!decimal.TryParse(PriceTextBox.Text, out Price))
            {
                MessageBox.Show("Non valid price format");
                return;
            }

            DateTime releaseDate = ReleaseDatePicker.DisplayDate;
            DateOnly releaseDateOnly = DateOnly.FromDateTime(releaseDate);

            var translator = TranslatorTextBox.Text;

            int Pages;
            if (!int.TryParse(PagesTextBox.Text, out Pages))
            {
                MessageBox.Show("Pages must be entered in numbers");
                return;
            }



            var existingBook = _bookRepository.GetByIsbn(Isbn13);
            existingBook.Title = title;
            existingBook.Language = language;
            existingBook.Format = format;
            existingBook.Price = Price;
            existingBook.ReleaseDate = releaseDateOnly;
            existingBook.Translater = translator;
            existingBook.Pages = Pages;

            
            
            
            var selectedAuthors = AuthorListBox.SelectedItems.Cast<AuthorModel>().ToList();
            existingBook.BookDetails.Clear();

            foreach (var author in selectedAuthors)
            {
                var detail = new BookDetailModel
                {
                    Isbn = Isbn13,
                    AuthorId = author.AuthorId,
                    PublisherId = (PublisherComboBox.SelectedItem as PublisherModel)?.PublisherId ?? 0
                };

                existingBook.BookDetails.Add(detail);
            }


            _bookRepository.UpdateBook(existingBook);


            var bookList = _bookRepository.GetAllBooks();
            BookList.Items.Clear();
            foreach (var book in bookList)
            {
                BookList.Items.Add($"{book.Title}");
            }

            ClearBoxes();


        }

        private void DeleteBookBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var isbnToDelete = IsbnTextBox.Text;

            if (!_bookRepository.BookExists(isbnToDelete))
            {
                MessageBox.Show("A book with this ISBN does not exist");
                return;
            }

            // Confirm with the user before deletion
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this book?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _bookRepository.DeleteBook(isbnToDelete);
                MessageBox.Show("Book deleted successfully");
                ClearBoxes();
            }

            var bookList = _bookRepository.GetAllBooks();
            BookList.Items.Clear();
            foreach (var book in bookList)
            {
                BookList.Items.Add($"{book.Title}");
            }


            
        }

        public void ClearBoxes()
        {
            IsbnTextBox.Clear();
            TitleTextBox.Clear();
            LanguageComboBox.ItemsSource = null;
            FormatTextBox.Clear();
            PriceTextBox.Clear();
            ReleaseDatePicker.SelectedDate = null;
            TranslatorTextBox.Clear();
            PagesTextBox.Clear();
            //AuthorListBox.ItemsSource = null;
            //PublisherComboBox.ItemsSource = null;
        }

    }
}
