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
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Pamietnik
{
    public sealed partial class Registration : Page
    {
        #region Zmienne

        private static string name, user, pass, confirmPass;

        #endregion

        #region Konstruktor

        public Registration()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Program

        private async void SignUpBtn_Click(object sender, RoutedEventArgs e)
        {
            name = NameTextBox.Text;
            user = UserTextBox.Text;
            pass = PassBox.Password;
            confirmPass = ConfirmPassBox.Password;

            if (user == "" || pass == "" || name == "" || confirmPass == "")
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
                    DbConnections.Register(name, user, pass, confirmPass);
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

        // Przejście do logowania

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        #endregion
    }
}

