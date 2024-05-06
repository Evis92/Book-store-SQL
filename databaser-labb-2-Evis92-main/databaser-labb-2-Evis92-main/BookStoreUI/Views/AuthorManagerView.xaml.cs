using Common.Models;
using DataAccess.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookStoreUI.Views
{
    /// <summary>
    /// Interaction logic for AuthorManagerView.xaml
    /// </summary>
    public partial class AuthorManagerView : UserControl
    {
        public MainWindowContext MainWindowContext { get; set; }

        private readonly BookRepository _bookRepository;
        private readonly AuthorRepository _authorRepository;
        private readonly PublisherRepository _publisherRepository;

       

        public AuthorManagerView()
        {
            InitializeComponent();

            MainWindowContext = new MainWindowContext();
            DataContext = MainWindowContext;


            _bookRepository = new BookRepository(ApplicationContextManager.Context);
            _authorRepository = new AuthorRepository(ApplicationContextManager.Context);
            _publisherRepository = new PublisherRepository(ApplicationContextManager.Context);

            AuthorListView.ItemsSource = _authorRepository.GetAllAuthors();
        }




        private void AddAuthor_OnClick(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameTxtBox.Text;
            string lastName = LastNameTxtBox.Text;

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {

                MessageBox.Show("Please enter valid names");
                return;
            }

            DateOnly selectedDate = DateOfBirthDatePicker.SelectedDate != null
                ? DateOnly.FromDateTime(DateOfBirthDatePicker.SelectedDate.Value)
                : default;

            if (_authorRepository.AuthorExist(firstName, lastName, selectedDate) == true)
            {
                MessageBox.Show("Author already exists");
                FirstNameTxtBox.Clear();
                LastNameTxtBox.Clear();
                return;
            }

            AuthorModel newAuthor = new AuthorModel
            {
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = selectedDate
            };

            _authorRepository.AddAuthor(newAuthor);

            var authors = _authorRepository.GetAllAuthors();
            AuthorListView.ItemsSource = null;
            AuthorListView.ItemsSource = authors;
            FirstNameTxtBox.Clear();
            LastNameTxtBox.Clear();

        }


    }
}
