using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MySql.Data.MySqlClient;
using Windows.UI.Text;


namespace Pamietnik
{
    internal sealed partial class Diary : Page
    {
        #region Zmienne

        private RichEditBox currentRichEditBox;
        private string countdown, date;

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
                DateTextBlock.Text = "Dzisiaj mamy " + DateTime.Today.ToString("D") + " r.";
            }
            catch (Exception)
            {
                mainBox.Document.SetText(TextSetOptions.None, Messages.GeneralError());
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

        // Zapisywanie wpisu

        private void SaveNewEntryBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currentRichEditBox.Document.GetText(TextGetOptions.None, out string entryText);
                DbConnections.entry = entryText;
                DbConnections.author = DbConnections.user;

                try
                {
                    DbConnections.SaveEntry(DbConnections.user, entryText, date);
                }
                catch (MySqlException)
                {
                    mainBox.Document.SetText(TextSetOptions.None, Messages.ConnectionError());
                }
            }
            catch (MySqlException)
            {
                mainBox.Document.SetText(TextSetOptions.None, Messages.ConnectionError());
            }

        }

        // Usuwanie wpisu

        private void DeleteEntryBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DbConnections.DeleteEntry(DbConnections.user, date);
                mainBox.Document.SetText(TextSetOptions.None, DbConnections.ShowEntry(DbConnections.user, date));
            }
            catch (MySqlException)
            {
                mainBox.Document.SetText(TextSetOptions.None, Messages.ConnectionError());  
            }

        }

        // Ustawianie aktywnego boxu

        private void PivotItem_Loaded(System.Object sender, RoutedEventArgs e)
        {
            PivotItem pi = sender as PivotItem;
            RichEditBox_SetFocus(pi);
        }

        private void mainPivot_GotFocus(object sender, RoutedEventArgs e)
        {
            Pivot p = sender as Pivot;
            PivotItem pi = p.SelectedItem as PivotItem;
            RichEditBox_SetFocus(pi);
        }

        private void RichEditBox_SetFocus(PivotItem pi)
        {
            RichEditBox reb = pi.Content as RichEditBox;
            currentRichEditBox = reb;
            reb.Focus(FocusState.Keyboard);
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
                mainBox.Document.SetText(TextSetOptions.None, Messages.GeneralError());
            }

        }

        private void MainCalendar_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            try
            {
                date = MainCalendar.SelectedDates[0].ToString("yyyy-MM-dd");
                entriesListView.Items.Add(MainCalendar.SelectedDates[0].ToString("yyyy-MM-dd (dddd)"));
            }
            catch (System.Runtime.InteropServices.COMException) { }

            try
            {
                mainBox.Document.SetText(TextSetOptions.None, DbConnections.ShowEntry(DbConnections.user, date));
            }
            catch (MySqlException)
            {
                mainBox.Document.SetText(TextSetOptions.None, Messages.ConnectionError());
            }
        }

        #endregion
    }
}
