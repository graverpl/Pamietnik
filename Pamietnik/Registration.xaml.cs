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
                StatusTextBlock.Text = "Pola nie mogą być puste." + "\nWprowadź poprawne dane...";
            }
            else if (PassBox.Password != ConfirmPassBox.Password)
            {
                StatusTextBlock.Text = "Hasła nie są identyczne." + "\nSpróbuj jeszcze raz...";
            }
            else
            {
                DbConnections.Register(name, user, pass, confirmPass);
                StatusTextBlock.Text = "Konto zostało założone. Możesz się już zalogować...";
                await Task.Delay(3000);
                this.Frame.Navigate(typeof(MainPage));
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

