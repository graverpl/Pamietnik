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
using System.Timers;

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=234238

namespace Pamietnik
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public  partial class Diary : Page
    {
        const string connString = "server=karolczak.atthost24.pl;user id=4263_diary;pwd=Karol123!;persistsecurityinfo=True;database=4263_diary";
        DateTime endTime = new DateTime(2018, 01, 01, 0, 0, 0);


        public Diary()
        {
            this.InitializeComponent();
        }

       
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Data bieżąca i odliczanie

            DateTime stopDate = DateTime.Parse("12/2/2018");
            DateTime startDate = DateTime.Now;

            TimeSpan timeLeft = stopDate - startDate;
            string countDown = string.Format("Do ferii zimowych pozostało {0} dni.", timeLeft.Days);

            DateTextBlock.Text = "Dzisiaj mamy " + DateTime.Today.ToString("D") + " r.";
            CountdownTextBlock.Text = countDown;


            // Dowcip

            using (MySqlConnection conn = new MySqlConnection(connString))
            using (MySqlCommand cmd = new MySqlCommand("SELECT " + "Joke " + "FROM jokes " + "ORDER BY RAND()" + "LIMIT 1;", conn))
            {
                cmd.Connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    JokeTextBlock.Text = reader.GetString("Joke");
                }

                entriesListView.Items.Add("Wpis 2");
                entriesListView.Items.Add("Wpis 3");
                entriesListView.Items.Add("Wpis 4");
                entriesListView.Items.Add("Wpis 5");
                entriesListView.Items.Add("Wpis 6");
            }

        }
    }
}