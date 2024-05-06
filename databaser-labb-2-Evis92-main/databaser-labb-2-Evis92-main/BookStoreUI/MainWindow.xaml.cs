using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BookStoreUI.Views;
using Common.Models;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace BookStoreUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private AuthorManagerView authorManager;
        private BookManagerView bookManager;

        public MainWindow()
        {
            InitializeComponent();



            //Göra en: public class MainWindowContext : INotifyPropertyChanged?? För bindingsarna??

        }






    }
}