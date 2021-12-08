﻿using System;
using System.IO;
using System.Data.SQLite;

namespace Groupwork
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // At the moment, destroy the database and begin anew
            if (File.Exists("MyDatabase.sqlite"))
            {
                File.Delete("MyDatabase.sqlite");
            }
            SQLiteConnection.CreateFile("MyDatabase.sqlite");
            Console.WriteLine("Created a database!");

            // Open a database connection
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
            m_dbConnection.Open();

            // Create a table
            string sql = "CREATE TABLE Highscores (name TEXT, score INTEGER)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            // Insert dummy data
            sql = "INSERT INTO Highscores (name, score) VALUES ('Me', 9001)";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            sql = "INSERT INTO Highscores (name, score) VALUES ('Myself', 6000)";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            sql = "INSERT INTO Highscores (name, score) VALUES ('And I', 9001)";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();



            // Read the data from the database
            sql = "SELECT * FROM Highscores ORDER BY score DESC";
            command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine("Name: " + reader["name"] + "\tScore: " + reader["score"]);
            }

            // Close the connection
            m_dbConnection.Close();
        }
    }
}
