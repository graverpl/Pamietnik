using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using MySql.Data.MySqlClient;

namespace Pamietnik
{
    internal sealed partial class MainPage : Page
    {
        #region Konstruktor

        public MainPage()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Program

        // Weryfikacja danych i logowanie

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DbConnections.user = UserTextBox.Text;
                DbConnections.pass = PassBox.Password;

                if (DbConnections.user == "" || DbConnections.pass == "")
                {
                    StatusTextBlock.Text = Messages.MandatoryFields();
                    return;
                }
                    bool loginSuccessful = DbConnections.DataValidation(UserTextBox.Text, PassBox.Password);

                    if (loginSuccessful)
                    {
                        this.Frame.Navigate(typeof(Diary));
                    }
                    else
                    {
                        StatusTextBlock.Text = Messages.LoginError();
                    }
            }
            catch (MySqlException)
            {
                StatusTextBlock.Text = Messages.ConnectionError();
            }
        }

        // Przejście do rejestracji

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Registration));
        }

        #endregion


    }
}

