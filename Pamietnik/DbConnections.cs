﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Pamietnik
{
    internal class DbConnections
    {
        #region Zmienne

        const string connString = "server=karolczak.atthost24.pl;user id=4263_diary;pwd=Karol123!;persistsecurityinfo=True;database=4263_diary";
        private static string name, joke;

        #endregion

        #region Metody

        // Sprawdzanie poprawności danych logowania

        public static bool DataValidation(string user, string pass)
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

        public static string GetName(string user)
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

        // Pobieranie dowcipu

        public static string GetJoke()
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

        public static void Register(string name, string user, string pass, string confirmPass)
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

        #endregion
    }
}
