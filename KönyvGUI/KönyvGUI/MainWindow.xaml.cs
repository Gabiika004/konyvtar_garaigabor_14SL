using KonyvGUI.Classes;
using KonyvGUI.Views;
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
using static System.Reflection.Metadata.BlobBuilder;

namespace KonyvGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Database db = App.db;
        List<Book> books = App.books;
        List<string> authors = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            db = new Database();
            books = db.getAllBooks();
            authors = db.getAllAuthors();
            LoadSelectors(authors);
            RefreshDataGrid();
        }


        private void LoadSelectors(List<string> authors)
        {
            var authorModels = authors.Select(author => new AuthorSelectorModel
            {
                AuthorName = author,
                IsSelected = true
            }).ToList();

            SelectorListView.ItemsSource = authorModels;
        }

        private void AuthorCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            RefreshDataGrid();
        }

        private void AuthorCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            RefreshDataGrid();
        }

        private void RefreshDataGrid()
        {
            var selectedAuthors = ((List<AuthorSelectorModel>)SelectorListView.ItemsSource)
                .Where(model => model.IsSelected)
                .Select(model => model.AuthorName)
                .ToList();

            var filteredBooks = books.Where(book => selectedAuthors.Contains(book.Author)).ToList();
            BookDataGrid.ItemsSource = filteredBooks;
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            NewBook newBook = new NewBook();
            var result = newBook.ShowDialog();

            if (result == true)
            {
                RefreshData();
            }
        }

        private void RefreshData()
        {
            books = db.getAllBooks(); // Újra betöltjük a könyveket az adatbázisból
            authors = db.getAllAuthors(); // Az szerzők listájának újratöltése
            LoadSelectors(authors); // Szűrők újratöltése
            RefreshDataGrid(); // DataGrid frissítése
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = BookDataGrid.SelectedItem as Book;
            if (selectedBook == null)
            {
                MessageBox.Show("Kérlek, válassz ki egy könyvet a módosításhoz!");
                return;
            }

            var updateBookWindow = new UpdateBook(selectedBook);
            updateBookWindow.ShowDialog();
            RefreshData();
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = BookDataGrid.SelectedItems.Cast<Book>().ToList();
            if (selectedItems.Count == 0)
            {
                MessageBox.Show("Nincs kiválasztott elem a törléshez.");
                return;
            }

            var message = $"Biztosan törölni szeretnéd a kiválasztott {selectedItems.Count} elemet?";
            var result = MessageBox.Show(message, "Törlés megerősítése", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                foreach (var item in selectedItems)
                {
                    db.DeleteBook(item.Id);
                }

                RefreshDataGrid();
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }
   
    }
}
