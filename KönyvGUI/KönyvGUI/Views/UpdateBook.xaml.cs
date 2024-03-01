using KonyvGUI.Classes;
using Mysqlx;
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
using System.Windows.Shapes;

namespace KonyvGUI.Views
{
    /// <summary>
    /// Interaction logic for UpdateBook.xaml
    /// </summary>
    public partial class UpdateBook : Window
    {
        private Book currentBook;
        Database db = App.db;
        internal UpdateBook(Book book)
        {
            InitializeComponent();
            db = new Database();
            currentBook = book;
            FillDataFields();
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            // Ellenőrizzük, hogy minden mező ki van-e töltve
            if (string.IsNullOrWhiteSpace(AuthorNameBox.Text) ||
                string.IsNullOrWhiteSpace(BookTitleBox.Text) ||
                string.IsNullOrWhiteSpace(PublishYearBox.Text) ||
                string.IsNullOrWhiteSpace(PageCountBox.Text))
            {
                MessageBox.Show("Minden mezőt ki kell tölteni!", "Hiányzó adatok", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                string title = BookTitleBox.Text;
                string author = AuthorNameBox.Text;
                Int16 publishYear = Convert.ToInt16(PublishYearBox.Text);
                int pageCount = Convert.ToInt32(PageCountBox.Text);

                db.ChangeBook(currentBook.Id,title,author,publishYear,pageCount);

                MessageBox.Show("Könyv sikeresen módosítva!", "Könyv módosítva", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;

            }
            catch (Exception ex)
            {
                // Hiba kezelése
                MessageBox.Show($"Hiba történt: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void FillDataFields()
        {
            AuthorNameBox.Text = currentBook.Author;
            BookTitleBox.Text = currentBook.Title;
            PublishYearBox.Text = currentBook.Publish_year.ToString();
            PageCountBox.Text = currentBook.Page_count.ToString();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

            var message = $"Biztosan törölni szeretnéd {currentBook.Title} című könvet?";
            var result = MessageBox.Show(message, "Törlés megerősítése", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (db.DeleteBook(currentBook.Id))
                {
                    MessageBox.Show("Sikeres törlés!", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
                    var mainWindow = new MainWindow(); 
                    mainWindow.Show(); 
                    this.Close();
                }       
            }  

        }
    }
}
