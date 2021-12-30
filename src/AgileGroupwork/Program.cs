using System;
using System.IO;
using System.Data.SQLite;
using System.Collections.Generic;

namespace Groupwork
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string input = "";
            string insertName = "";
            string scoreInput = "";
            int insertScore = 0;
            // At the moment, destroy the database and begin anew
            if (File.Exists("MyDatabase.sqlite"))
            {
                File.Delete("MyDatabase.sqlite");
            }
            SQLiteConnection.CreateFile("MyDatabase.sqlite");

            // Open a database connection
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
            m_dbConnection.Open();

            // Create a table
            string sql = "CREATE TABLE Scoreboard (name TEXT, score INTEGER)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();


            //The program starts
            while (true)
            {
                Console.WriteLine("Show all the scores (Press 1)");
                Console.WriteLine("Add a score for a person (Press 2)");
                Console.WriteLine("Show a person's score (Press 3)");
                Console.WriteLine("Quit (Press 4)");
                input = Console.ReadLine();

                if (input == "4")
                {
                    break;
                }
                else if (input == "1")
                {
                    Console.WriteLine();
                    string sqlCommand = "Select score from Scoreboard";
                    command = new SQLiteCommand(sqlCommand, m_dbConnection);
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine("Score: " + reader["score"]);
                    }
                    Console.WriteLine();

                }
                else if (input == "2")
                {
                    Console.WriteLine();
                    Console.WriteLine("Give a name to the person");
                    insertName = Console.ReadLine();
                    Console.WriteLine("Give a score to that person");
                    scoreInput = Console.ReadLine();
                    Console.WriteLine();
                    try
                    {
                        insertScore = Convert.ToInt32(scoreInput);
                        sql = String.Format("INSERT INTO Scoreboard (name, score) VALUES ('{0}', '{1}')", insertName, insertScore);
                        command = new SQLiteCommand(sql, m_dbConnection);
                        command.ExecuteNonQuery();
                        Console.WriteLine();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine();
                    }
                }
                else if (input == "3")
                {
                    Console.WriteLine();
                    Console.WriteLine("Give me a name");
                    string personName = Console.ReadLine();
                    string sqlCommand = String.Format("Select score from Scoreboard where name='{0}'", personName);
                    command = new SQLiteCommand(sqlCommand, m_dbConnection);
                    SQLiteDataReader reader = command.ExecuteReader();
                    reader.Read();
                    Console.WriteLine(reader["score"]);
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("you wrote an incorrect input");
                    Console.WriteLine();
                }
            }
            m_dbConnection.Close();
        }
    }
}
