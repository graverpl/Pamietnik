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

            // Wpisy w pamiętniku

            entriesListView.Items.Add("Wpis 2");
            entriesListView.Items.Add("Wpis 3");
            entriesListView.Items.Add("Wpis 4");
            entriesListView.Items.Add("Wpis 5");
            entriesListView.Items.Add("Wpis 6");
            entriesListView.Items.Add("Wpis 6");
            entriesListView.Items.Add("Wpis 6");
            entriesListView.Items.Add("Wpis 6");
            entriesListView.Items.Add("Wpis 6");
            entriesListView.Items.Add("Wpis 6");

        }

        #endregion

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

        private void SaveNewEntryBtn_Click(object sender, RoutedEventArgs e)
        {  
            currentRichEditBox.Document.GetText(TextGetOptions.None, out string entryText);
            entry = entryText;
            author = MainPage.user;
            try
            {
                DbConnections.SaveEntry(author, entryText);
            }
            catch (MySqlException) {}
        }

        private void mainPivot_GotFocus(object sender, RoutedEventArgs e)
        {
            Pivot p = sender as Pivot;
            PivotItem pi = p.SelectedItem as PivotItem;
            RichEditBox_SetFocus(pi);
        }

        private void PivotItem_Loaded(System.Object sender, RoutedEventArgs e)
        {
            PivotItem pi = sender as PivotItem;
            RichEditBox_SetFocus(pi);
            try
            {
                currentRichEditBox.Document.SetText(TextSetOptions.None, DbConnections.ShowEntry(MainPage.user));
            }
            catch (ArgumentNullException)

            {
                currentRichEditBox.Document.SetText(TextSetOptions.None, "Spokojnie, nikt nie patrzy. Możesz coś napisać...");
            }
            
        }

        private void RichEditBox_SetFocus(PivotItem pi)
        {
            RichEditBox reb = pi.Content as RichEditBox;
            reb.Focus(FocusState.Keyboard);
            currentRichEditBox = reb;
            
        }
    }
}
