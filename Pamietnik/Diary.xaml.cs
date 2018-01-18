using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Text;
using MySql.Data.MySqlClient;
using Windows.UI.Xaml.Media;

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

        // Popup z komunikatami

        internal async void ErrorInfo(string error)
        {
            StatusPopup.IsOpen = true;
            PopupStatusTextBlock.Foreground = new SolidColorBrush(Colors.DarkRed);
            PopupStatusTextBlock.Text = error;
            await Task.Delay(2000);
            StatusPopup.IsOpen = false;
        }

        internal async void SuccessInfo(string success)
        {
            StatusPopup.IsOpen = true;
            PopupStatusTextBlock.Text = success;
            await Task.Delay(2000);
            StatusPopup.IsOpen = false;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Data bieżąca i odliczanie

            DateTime stopDate = DateTime.Parse("12/2/2018");
            DateTime startDate = DateTime.Now;

            TimeSpan timeLeft = stopDate - startDate;
            countdown = string.Format("Do ferii zimowych pozostało {0} dni.", timeLeft.Days);

            CountdownTextBlock.Text = countdown;
            DateTextBlock.Text = "Dzisiaj jest " + DateTime.Today.ToString("D") + " r.";

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
            MainCalendar.SelectedDates.Add(DateTime.Now);
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
                MainBox.Document.SetText(TextSetOptions.None, "Nie można pobrać zawartości...");
                ErrorInfo(Messages.ConnectionError());
            }
        }

        // Zapisywanie wpisu

        private void SaveEntryBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DbConnections.CheckEntry(DbConnections.user, date);

                if (DbConnections.count > 0)
                {
                    EditPopup.IsOpen = true;
                }
                else
                {
                    MainBox.Document.GetText(TextGetOptions.None, out string entryText);
                    DbConnections.entry = entryText;
                    DbConnections.author = DbConnections.user;
                    DbConnections.SaveEntry(DbConnections.user, date, entryText);
                    MainBox.Document.SetText(TextSetOptions.None, entryText);
                    SuccessInfo(Messages.SaveSuccess());
                }
            }
            catch (MySqlException)
            {
                EditPopup.IsOpen = false;
                ErrorInfo(Messages.ConnectionError());
            }
        }

        // Edycja wpisu

        private void EditYesBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainBox.Document.GetText(TextGetOptions.None, out string entryText);
                DbConnections.entry = entryText;
                DbConnections.author = DbConnections.user;
                DbConnections.EditEntry(DbConnections.user, date, entryText);
                MainBox.Document.SetText(TextSetOptions.None, entryText);
                EditPopup.IsOpen = false;
                SuccessInfo(Messages.EditSuccess());
            }
            catch (MySqlException)
            {
                EditPopup.IsOpen = false;
                ErrorInfo(Messages.ConnectionError());
            }
        }

        private void EditNoBtn_Click(object sender, RoutedEventArgs e)
        {
            EditPopup.IsOpen = false;
        }

        // Usuwanie wpisu

        private void DeleteEntryBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DbConnections.CheckEntry(DbConnections.user, date);

                if (DbConnections.count > 0)
                {
                    DeletePopup.IsOpen = true;
                }
                else
                {
                    MainBox.Document.SetText(TextSetOptions.None, "");
                    ErrorInfo(Messages.EntryNotFound());
                }
            }
            catch (MySqlException)
            {
                ErrorInfo(Messages.ConnectionError());
            }
        }

        private void DeleteYesBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DbConnections.DeleteEntry(DbConnections.user, date);
                MainBox.Document.SetText(TextSetOptions.None, "");
                DeletePopup.IsOpen = false;
                SuccessInfo(Messages.DeleteSuccess());
            }
            catch (MySqlException)
            {
                DeletePopup.IsOpen = false;
                ErrorInfo(Messages.ConnectionError());
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
