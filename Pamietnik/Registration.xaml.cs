using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;


//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x415

namespace Pamietnik
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class Registration : Page
    {
        const string connString = "server=karolczak.atthost24.pl;user id=4263_diary;pwd=Karol123!;persistsecurityinfo=True;database=4263_diary";

        public Registration()
        {
            this.InitializeComponent();
        }

        private async void SignUpBtn_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string user = UserTextBox.Text;
            string pass = PassBox.Password;
            string confirmPass = ConfirmPassBox.Password;

            if (user == "" || pass == "" || name == "" || confirmPass == "")
            {
                StatusTextBlock.Text = "Pola nie mogą być puste." + "\nWprowadź poprawne dane...";
            }
            else if (PassBox.Password != ConfirmPassBox.Password)
            {
                StatusTextBlock.Text = "Hasła nie są identyczne." + "\nSpróbuj jeszcze raz...";
            }
            else
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO users " + "(Username, Password, Name) " +
                    "VALUES " + "(@user, @pass, @name);", conn))
                {
                    cmd.Connection.Open();
                    cmd.Parameters.AddWithValue("@user", user);
                    cmd.Parameters.AddWithValue("@pass", pass);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.ExecuteNonQuery();
                    StatusTextBlock.Text = "Konto zostało założone. Możesz się już zalogować...";
                    await Task.Delay(3000);
                    this.Frame.Navigate(typeof(MainPage));
                }
            }
        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}

