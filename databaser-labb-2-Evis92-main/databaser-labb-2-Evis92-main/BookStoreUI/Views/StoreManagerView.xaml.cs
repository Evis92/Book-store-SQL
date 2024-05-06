using Common.Models;
using DataAccess.Services;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace BookStoreUI.Views
{
    /// <summary>
    /// Interaction logic for StoreManagerView.xaml
    /// </summary>
    public partial class StoreManagerView : UserControl
    {
        public MainWindowContext MainWindowContext { get; set; }
        private readonly BookRepository _bookRepository;
        private readonly AuthorRepository _authorRepository;
        private readonly StoreRepository _storeRepository;
        private readonly InventoryRepository _inventoryRepository;


        public StoreManagerView()
        {
            InitializeComponent();

            

            MainWindowContext = new MainWindowContext();
            DataContext = MainWindowContext;

            _bookRepository = new BookRepository(ApplicationContextManager.Context);
            _authorRepository = new AuthorRepository(ApplicationContextManager.Context);
            _storeRepository = new StoreRepository(ApplicationContextManager.Context);
            _inventoryRepository = new InventoryRepository(ApplicationContextManager.Context);

            InventoryRepository.InventoryChanged += InventoryRepository_InventoryChanged;
            BookRepository.BookChanged += BookRepository_BookChanged;



            //-------- skriver ut alla boktitlar i Listvyn ALLBOOKS
            AllBooksList.ItemsSource = _bookRepository.GetAllBooks();




            // SKRIVER UT STORES I COMBOBOX---------------------------------------------------
            StoreOptions.ItemsSource = _storeRepository.GetAllStores();


        }

        private void BookRepository_BookChanged()
        {
            
                AllBooksList.ItemsSource = _bookRepository.GetAllBooks();
        }



        //Ändrar Quantity i BookStore-ListVyn 
        private void InventoryRepository_InventoryChanged()
        {

            if (StoreOptions.SelectedItem is StoreModel selectedStore)
            {
                BookStoreList.ItemsSource = _inventoryRepository.GetInventoryForStore(selectedStore.StoreId);
            }
        }




        private void AddBookBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedStore = StoreOptions.SelectedItem as StoreModel;
            var selectedBook = AllBooksList.SelectedItem as BookModel;

            //Om ingen bok valts i ListVyn skrivs detta ut i en meddelandebox
            if (selectedBook == null)
            {
                MessageBox.Show("You have not specified which book you want to add");
                return;
            }
            
            //Om ingen butik valts i ComboBoxen skrivs detta ut i en Meddelandebox
            if (selectedStore == null)
            {
                MessageBox.Show("You have not specified a store");
                return;
            }

            if (selectedStore != null && selectedBook != null)
            {
                _inventoryRepository.AddBookToInventory(selectedStore.StoreId, selectedBook, 1);
            }
        }




        private void RemoveBookBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedStore = StoreOptions.SelectedItem as StoreModel;
            var selectedInventory = BookStoreList.SelectedItem as InventoryModel;

           
            if (selectedInventory == null)
            {
                MessageBox.Show("You have not specified which book you want to remove");
                return;
            }
           
            if (selectedStore == null)
            {
                MessageBox.Show("You have not specified a store");
                return;
            }

            if (selectedStore != null && selectedInventory != null)
            {
                var selectedBook = selectedInventory.IsbnNavigation;
                _inventoryRepository.RemoveBookFromInventory(selectedStore.StoreId, selectedBook, 1);
            }
        }





        private void StoreOptions_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Uppdatera BookStoreList direkt
            if (StoreOptions.SelectedItem is StoreModel selectedStore)
            {
                BookStoreList.ItemsSource = _inventoryRepository.GetInventoryForStore(selectedStore.StoreId);
            }
        }

    }
    
}


