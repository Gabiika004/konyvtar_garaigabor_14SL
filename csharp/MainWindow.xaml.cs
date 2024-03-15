using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // HttpClient példány létrehozása
        private static readonly HttpClient client = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
            // A HttpClient alap címének beállítása
            client.BaseAddress = new Uri("http://127.0.0.1:8000/api/");
        }
        private async void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateRegistrationInput())
            {
                return;
            }

            var user = new
            {
                name = txtRegisterName.Text,
                email = txtRegisterEmail.Text,
                password = txtRegisterPassword.Password
            };

            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                // Az API regisztrációs endpointjára küldött POST kérés
                var response = await client.PostAsync("register", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Sikeres regisztráció!");
                }
                else
                {
                    MessageBox.Show("Regisztráció sikertelen. Próbáld újra!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt: {ex.Message}");
            }
        }

        private bool ValidateRegistrationInput()
        {
            if (string.IsNullOrWhiteSpace(txtRegisterName.Text))
            {
                MessageBox.Show("A név megadása kötelező.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtRegisterEmail.Text))
            {
                MessageBox.Show("Az email cím megadása kötelező.");
                return false;
            }
            // Egy egyszerű email cím validálás
            if (!txtRegisterEmail.Text.Contains("@") || !txtRegisterEmail.Text.Contains("."))
            {
                MessageBox.Show("Érvénytelen email cím.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtRegisterPassword.Password))
            {
                MessageBox.Show("A jelszó megadása kötelező.");
                return false;
            }
            if (txtRegisterPassword.Password.Length < 8)
            {
                MessageBox.Show("A jelszónak legalább 8 karakter hosszúnak kell lennie.");
                return false;
            }

            return true;
        }
        private bool ValidateLoginInput()
        {
            if (string.IsNullOrWhiteSpace(txtLoginName.Text))
            {
                MessageBox.Show("A név megadása kötelező.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtLoginEmail.Text))
            {
                MessageBox.Show("Az email cím megadása kötelező.");
                return false;
            }
            // Egy egyszerű email cím validálás
            if (!txtLoginEmail.Text.Contains("@") || !txtLoginEmail.Text.Contains("."))
            {
                MessageBox.Show("Érvénytelen email cím.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtLoginPassword.Password))
            {
                MessageBox.Show("A jelszó megadása kötelező.");
                return false;
            }
            if (txtLoginPassword.Password.Length < 8)
            {
                MessageBox.Show("A jelszónak legalább 8 karakter hosszúnak kell lennie.");
                return false;
            }

            return true;
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateLoginInput())
            {
                return;
            }

            var user = new
            {
                name = txtLoginName.Text,
                email = txtLoginEmail.Text,
                password = txtLoginPassword.Password
            };

            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("login", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent);

                    // Sikeres bejelentkezés esetén megnyit egy új ablakot a felhasználó email címével
                    ShowUserEmailWindow(loginResponse.Email);
                }
                else
                {
                    MessageBox.Show("Bejelentkezés sikertelen. Próbáld újra!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt: {ex.Message}");
            }
        }

        // Ez a metódus megnyit egy új ablakot, amely megjeleníti a felhasználó email címét
        private void ShowUserEmailWindow(string userEmail)
        {
            var userEmailWindow = new UserEmailWindow(userEmail);
            userEmailWindow.Show();
        }


    }
}