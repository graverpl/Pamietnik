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
using Windows.UI.Text;
using System.Web;

namespace Pamietnik
{
    public sealed partial class Diary : Page
    {
        private RichEditBox currentRichEditBox;
        private static string author, date, entry;

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

            try
            {
                WelcomeTextBlock.Text = "Witaj " + DbConnections.GetName(MainPage.user) + "!";
            }
            catch (MySqlException)
            {
                WelcomeTextBlock.Text = "Witaj!";
            }

            // Wyświetlenie dowcipu

            try
            {
                JokeTextBlock.Text = DbConnections.GetJoke();
            }
            catch (MySqlException)
            {
                JokeTextBlock.Text = "";
            }
        }

        // Wczytywanie wpisów po dacie

        private void LoadEntriesByDate()
        {
            entriesListView.Items.Add(MainCalendar.SelectedDates[0].ToString("dd/MM/yyyy (ddd)"));
        }

        // Dodawanie nowego pola dla wpisu

        private void AddNewEntryBtn_Click(object sender, RoutedEventArgs e)
        {
            PivotItem pi = new PivotItem();
            var entryText = String.Format("Wpis {0}", mainPivot.Items.Count + 1);
            pi.Header = entryText;
            RichEditBox reb = new RichEditBox();
            reb.HorizontalAlignment = HorizontalAlignment.Stretch;
            reb.VerticalAlignment = VerticalAlignment.Stretch;
            pi.Content = reb;
            pi.Loaded += PivotItem_Loaded;
            mainPivot.Items.Add(pi);
            mainPivot.SelectedIndex = mainPivot.Items.Count - 1;
        }

        // Zapisywanie wpisu w bazie

        private void SaveNewEntryBtn_Click(object sender, RoutedEventArgs e)
        {
            currentRichEditBox.Document.GetText(TextGetOptions.None, out string entryText);
            entry = entryText;
            author = MainPage.user;
            date = MainCalendar.SelectedDates[0].ToString("yyyy-MM-dd");

            try
            {
                DbConnections.SaveEntry(author, entryText, date);
            }
            catch (MySqlException) { }
        }

        // Ustawianie aktywnego pola

        private void mainPivot_GotFocus(object sender, RoutedEventArgs e)
        {
            Pivot p = sender as Pivot;
            PivotItem pi = p.SelectedItem as PivotItem;
            RichEditBox_SetFocus(pi);
        }

        private void MainCalendar_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            LoadEntriesByDate();
        }

        private void MainCalendar_Loaded(object sender, RoutedEventArgs e)
        {
            MainCalendar.SelectedDates.Add(DateTime.Now);
        }

        private void RichEditBox_SetFocus(PivotItem pi)
        {
            RichEditBox reb = pi.Content as RichEditBox;
            reb.Focus(FocusState.Keyboard);
            currentRichEditBox = reb;
        }

        // Ładowanie zakładek

        private void PivotItem_Loaded(System.Object sender, RoutedEventArgs e)
        {
            PivotItem pi = sender as PivotItem;
            RichEditBox_SetFocus(pi);
            
        }

        #endregion
    }
}
