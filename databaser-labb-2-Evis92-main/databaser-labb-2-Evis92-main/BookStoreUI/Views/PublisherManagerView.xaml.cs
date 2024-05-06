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
    /// Interaction logic for PublisherManagerView.xaml
    /// </summary>
    public partial class PublisherManagerView : UserControl
    {

        public MainWindowContext MainWindowContext { get; set; }
        private readonly PublisherRepository _publisherRepository;

        public PublisherManagerView()
        {
            InitializeComponent();

            MainWindowContext = new MainWindowContext();
            DataContext = MainWindowContext;

            _publisherRepository = new PublisherRepository(ApplicationContextManager.Context);

            PublisherListView.ItemsSource = _publisherRepository.GetAllPublishers();
        }


        private void AddPublisher_OnClick(object sender, RoutedEventArgs e)
        {
            string publisherName = PublisherNameTxtBox.Text;

            if (string.IsNullOrWhiteSpace(publisherName))
            {
                MessageBox.Show("Please enter name of publisher");
                return;
            }

            if (_publisherRepository.PublisherExists(publisherName) == true)
            {
                MessageBox.Show("Publisher already exists");
                PublisherNameTxtBox.Clear();
                return;
            }

            PublisherModel newPublisher = new PublisherModel
            {
                Name = publisherName
            };

            _publisherRepository.AddPublisher(newPublisher);
            
            var _publishers = _publisherRepository.GetAllPublishers();
            PublisherListView.ItemsSource = null;
            PublisherListView.ItemsSource = _publishers;


            PublisherNameTxtBox.Clear();
        }
    }
}
