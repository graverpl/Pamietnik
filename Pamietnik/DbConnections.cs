﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Pamietnik
{
    internal abstract class DbConnections
    {
        #region Zmienne

        const string connString = "server=karolczak.atthost24.pl;user id=4263_diary;pwd=Karol123!;persistsecurityinfo=True;database=4263_diary";
        internal protected static string user, pass, confirmPass, author, name, entry, joke;

        #endregion

        #region Metody

        // Sprawdzanie poprawności danych logowania

        internal static bool DataValidation(string user, string pass)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            using (MySqlCommand cmd = new MySqlCommand("SELECT " + "Username, Password " + "FROM users " + "WHERE Username=@user AND Password=@pass;", conn))
            {
                cmd.Parameters.AddWithValue("@user", user);
                cmd.Parameters.AddWithValue("@pass", pass);
                cmd.Connection.Open();

                MySqlDataReader reader = cmd.ExecuteReader();
                return reader.Read();
            }
        }

        // Pobieranie imienia użytkownika

        internal static string GetName(string user)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            using (MySqlCommand cmd = new MySqlCommand("SELECT " + "Name " + "FROM users " + "WHERE Username=@user;", conn))
            {
                cmd.Parameters.AddWithValue("@user", user);
                cmd.Connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    name = reader.GetString("Name");
                }
                return name;
            }
        }

        // Pobieranie losowego dowcipu

        internal static string GetJoke()
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            using (MySqlCommand cmd = new MySqlCommand("SELECT " + "Joke " + "FROM jokes " + "ORDER BY RAND()" + "LIMIT 1;", conn))
            {
                cmd.Connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    joke = reader.GetString("Joke");
                }
            }
            return joke;
        }

        // Wysyłanie danych rejestracyjnych

        internal static void Register(string name, string user, string pass, string confirmPass)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            using (MySqlCommand cmd = new MySqlCommand("INSERT INTO users " + "(Username, Password, Name) " +
                "VALUES " + "(@user, @pass, @name);", conn))
            {
                cmd.Connection.Open();
                cmd.Parameters.AddWithValue("@user", user);
                cmd.Parameters.AddWithValue("@pass", pass);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.ExecuteNonQuery();
            }
        }

        // Dodawanie wpisu

        internal static void SaveEntry(string author, string entry, string date)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            using (MySqlCommand cmd = new MySqlCommand("INSERT INTO entries " + "(Author, Entry, EntryDate) " +
                "VALUES " + "(@author, @entry, @date);", conn))
            {
                cmd.Connection.Open();
                cmd.Parameters.AddWithValue("@author", author);
                cmd.Parameters.AddWithValue("@entry", entry);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.ExecuteNonQuery();
            }
        }

        // Odczytywanie wpisu

        internal static string ShowEntry(string author, string date)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            using (MySqlCommand cmd = new MySqlCommand("SELECT " + "Entry " + "FROM entries " + "WHERE Author=@author AND EntryDate=@date;",conn))
            {
                cmd.Connection.Open();
                cmd.Parameters.AddWithValue("@author", author);
                cmd.Parameters.AddWithValue("@date", date);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    entry = reader.GetString("Entry");
                }
                else
                {
                    entry = "Brak wpisów tego dnia...";
                }
            }
            return entry;
        }

        #endregion
    }
}
