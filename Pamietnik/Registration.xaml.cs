using System;
using System.Collections.Generic;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Pamietnik
{
    internal sealed partial class Registration : Page
    {
        #region Konstruktor

        public Registration()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Program

        // Formularz rejestracji

        private async void SignUpBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DbConnections.name = NameTextBox.Text;
                DbConnections.user = UserTextBox.Text;
                DbConnections.pass = PassBox.Password;
                DbConnections.confirmPass = ConfirmPassBox.Password;

                if (DbConnections.user == "" || DbConnections.pass == "" || DbConnections.name == "" || DbConnections.confirmPass == "")
                {
                    StatusTextBlock.Text = Messages.MandatoryFields();
                }
                else if (PassBox.Password != ConfirmPassBox.Password)
                {
                    StatusTextBlock.Text = Messages.PassConfirmationError();
                }
                else
                {
                    try
                    {
                        DbConnections.Register(NameTextBox.Text, UserTextBox.Text, PassBox.Password, ConfirmPassBox.Password);
                        StatusTextBlock.Text = Messages.RegistrationCorrect();
                        await Task.Delay(3000);
                        this.Frame.Navigate(typeof(MainPage));
                    }
                    catch (MySqlException)
                    {
                        StatusTextBlock.Text = Messages.ConnectionError();
                    }
                }
            }
            catch (Exception)
            {
                StatusTextBlock.Text = Messages.ConnectionError();
            }
        }

        // Przejście do logowania

        private void LoginFwdTextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        #endregion
    }
}

