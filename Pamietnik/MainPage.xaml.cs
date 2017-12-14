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


//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x415

namespace Pamietnik
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        const string connString = "server=karolczak.atthost24.pl;user id=4263_diary;pwd=Karol123!;persistsecurityinfo=True;database=4263_diary";

        public MainPage()
        {
            this.InitializeComponent();
        }

        private bool DataValidation(string user, string pass)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            using (MySqlCommand cmd = new MySqlCommand("SELECT " +
                "Username, Password " +
                "FROM users " +
                "WHERE Username=@user AND Password=@pass;", conn))
            {

                cmd.Parameters.AddWithValue("@user", user);
                cmd.Parameters.AddWithValue("@pass", pass);
                cmd.Connection = conn;
                cmd.Connection.Open();

                MySqlDataReader login = cmd.ExecuteReader();
                if (login.Read())
                {
                    conn.Close();
                    return true;
                }
                else
                {
                    conn.Close();
                    return false;
                }
            }
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string user = UserTextBox.Text;
            string pass = PassBox.Password;

            if (user == "" || pass == "")
            {
                StatusTextBlock.Text = "Pola nie mogą być puste." + "\nProszę popraw dane i spróbuj jeszcze raz...";
                return;
            }

            bool loginSuccessful = DataValidation(user, pass);

            if (loginSuccessful)
            {
                this.Frame.Navigate(typeof(Diary), null);
            }
            else
            {
                StatusTextBlock.Text = "Niepoprawny login lub hasło." + "\nProszę popraw dane i spróbuj jeszcze raz...";
            }
        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Registration));
        }

       
    }
}

