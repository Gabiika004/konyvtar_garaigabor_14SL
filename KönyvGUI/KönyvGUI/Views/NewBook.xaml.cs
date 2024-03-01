using KonyvGUI.Classes;
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
    /// Interaction logic for NewBook.xaml
    /// </summary>
    public partial class NewBook : Window
    {
        Database db = App.db;
        public NewBook()
        {
            InitializeComponent();
            db = new Database();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
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
                // Új könyv létrehozása az adatok alapján
                Book newBook = new Book(
                    BookTitleBox.Text,
                    AuthorNameBox.Text,
                    Convert.ToInt32(PublishYearBox.Text),
                    Convert.ToInt32(PageCountBox.Text)
                );

                var db = new Database();
                db.AddBook(newBook);
                MessageBox.Show("Könyv sikeresen hozzáadva!", "Könyv hozzáadva", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;

            }
            catch (Exception ex)
            {
                // Hiba kezelése
                MessageBox.Show($"Hiba történt: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Application.Current.MainWindow.Show(); 
        }

    }
}
