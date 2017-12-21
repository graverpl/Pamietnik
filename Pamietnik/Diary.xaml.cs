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
    public sealed partial class Diary : Page
    {
        #region Zmienne

        private string countdown;

        #endregion

        #region Konstruktor

        public Diary()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Program

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Data bieżąca i odliczanie

            DateTime stopDate = DateTime.Parse("12/2/2018");
            DateTime startDate = DateTime.Now;

            TimeSpan timeLeft = stopDate - startDate;
            countdown = string.Format("Do ferii zimowych pozostało {0} dni.", timeLeft.Days);

            CountdownTextBlock.Text = countdown;
            DateTextBlock.Text = "Dzisiaj mamy " + DateTime.Today.ToString("D") + " r.";

            // Wyświetlenie imienia

            WelcomeTextBlock.Text = "Witaj " + DbConnections.GetName(MainPage.user) + "!";

            // Wyświetlenie dowcipu

            JokeTextBlock.Text = DbConnections.GetJoke();

            // Wpisy w pamiętniku

            entriesListView.Items.Add("Wpis 2");
            entriesListView.Items.Add("Wpis 3");
            entriesListView.Items.Add("Wpis 4");
            entriesListView.Items.Add("Wpis 5");
            entriesListView.Items.Add("Wpis 6");

        }

        #endregion
    }
}
