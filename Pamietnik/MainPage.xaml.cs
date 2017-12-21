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

namespace Pamietnik
{ 
    public sealed partial class MainPage : Page
    {
        #region Zmienne

        internal static string user, pass;

        #endregion

        #region Konstruktor

        public MainPage()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Program

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            user = UserTextBox.Text;
            pass = PassBox.Password;

            if (user == "" || pass == "")
            {
                StatusTextBlock.Text = "Pola nie mogą być puste." + "\nProszę popraw dane i spróbuj jeszcze raz...";
                return;
            }

            bool loginSuccessful = DbConnections.DataValidation(user, pass);

            if (loginSuccessful)
            {
                this.Frame.Navigate(typeof(Diary));
            }
            else
            {
                StatusTextBlock.Text = "Niepoprawny login lub hasło." + "\nProszę popraw dane i spróbuj jeszcze raz...";
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

