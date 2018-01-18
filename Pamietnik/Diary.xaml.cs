using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Text;
using MySql.Data.MySqlClient;

namespace Pamietnik
{
    internal sealed partial class Diary : Page
    {
        #region Zmienne

        private string countdown, date, currentEntryDate;

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

            try
            {
                DateTime stopDate = DateTime.Parse("12/2/2018");
                DateTime startDate = DateTime.Now;

                TimeSpan timeLeft = stopDate - startDate;
                countdown = string.Format("Do ferii zimowych pozostało {0} dni.", timeLeft.Days);

                CountdownTextBlock.Text = countdown;
                DateTextBlock.Text = "Dzisiaj jest " + DateTime.Today.ToString("D") + " r.";
            }
            catch (Exception)
            {
                MainBox.Document.SetText(TextSetOptions.None, Messages.GeneralError());
            }

            // Wyświetlenie imienia

            try
            {
                WelcomeTextBlock.Text = "Witaj " + DbConnections.GetName(DbConnections.user) + "!";
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

        // Obsługa kalendarza

        private void MainCalendar_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                MainCalendar.SelectedDates.Add(DateTime.Now);
            }
            catch (Exception)
            {
                MainBox.Document.SetText(TextSetOptions.None, Messages.GeneralError());
            }
        }

        private void MainCalendar_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            try
            {
                date = MainCalendar.SelectedDates[0].ToString("yyyy-MM-dd");
                currentEntryDate = MainCalendar.SelectedDates[0].ToString("d MMMM (dddd)");
                CurrentEntryDateTextBlock.Text = $"Twój wpis z dnia {currentEntryDate}:";
            }
            catch (System.Runtime.InteropServices.COMException) { }

            try
            {
                MainBox.Document.SetText(TextSetOptions.None, DbConnections.ShowEntry(DbConnections.user, date));
            }
            catch (MySqlException)
            {
                MainBox.Document.SetText(TextSetOptions.None, Messages.ConnectionError());
            }
        }

        // Zapisywanie wpisu

        private void SaveEntryBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainBox.Document.GetText(TextGetOptions.None, out string entryText);
                DbConnections.entry = entryText;
                DbConnections.author = DbConnections.user;

                try
                {
                    DbConnections.SaveEntry(DbConnections.user, date, entryText);
                }
                catch (MySqlException)
                {
                    MainBox.Document.SetText(TextSetOptions.None, Messages.ConnectionError());
                }
            }
            catch (MySqlException)
            {
                MainBox.Document.SetText(TextSetOptions.None, Messages.ConnectionError());
            }
        }

        // Usuwanie wpisu

        private void DeleteEntryBtn_Click(object sender, RoutedEventArgs e)
        {
            DeletePopup.IsOpen = true;
        }

        private void DeleteYesBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DbConnections.DeleteEntry(DbConnections.user, date);
                MainBox.Document.SetText(TextSetOptions.None, Messages.DeleteSuccess());
                DeletePopup.IsOpen = false;
            }
            catch (MySqlException)
            {
                MainBox.Document.SetText(TextSetOptions.None, Messages.ConnectionError());
            }
        }

        private void DeleteNoBtn_Click(object sender, RoutedEventArgs e)
        {
            DeletePopup.IsOpen = false;
        }

        // Wylogowanie

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        #endregion
    }
}
