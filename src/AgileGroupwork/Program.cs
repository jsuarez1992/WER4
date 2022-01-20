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
            //add password encryption 
            string input = "";
            string userInput = "";
            string insertName = "";
            string scoreInput = "";
            int insertScore = 0;
            string currentUser = "";
            
            
            // find another solution that doesnt destroy the database
            if (File.Exists("MyDatabase.sqlite"))
            {
                File.Delete("MyDatabase.sqlite");
            }
            SQLiteConnection.CreateFile("MyDatabase.sqlite");
            

            // Open a database connection
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
            m_dbConnection.Open();



            string sqluser = "CREATE TABLE User (user TEXT, password TEXT)";
            
            SQLiteCommand commanduser = new SQLiteCommand(sqluser, m_dbConnection);
            commanduser.ExecuteNonQuery();

            // Create tables
            string sql = "CREATE TABLE Scoreboard (name TEXT, score INTEGER, category TEXT, user TEXT)";
            
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            //Log in part
            while (true)
            {
                Console.WriteLine("Log in! (Press 1)");
                Console.WriteLine("Create user! (Press 2)");
                Console.WriteLine("Quit! (Press 3)");
                userInput = Console.ReadLine();

                if (userInput == "3")
                {
                    break;
                }
                else if (userInput == "1")
                {
                    // work in progress , comment section out if want to test run
                    /*Console.WriteLine();
                    Console.WriteLine("Enter your username");
                    string userId = Console.ReadLine();
                    string sqlLogin = String.Format("Select user from User where user='{0}'", userId);
                    commanduser = new SQLiteCommand(sqlLogin, m_dbConnection);
                    SQLiteDataReader userReader = commanduser.ExecuteReader();
                    userReader.Read();
                    Console.WriteLine(userReader["user"]);
                    Console.WriteLine(); */
                }
                else if (userInput == "2")
                {
                    
                    Console.WriteLine();
                    Console.WriteLine("Enter your username:");
                    string userName = Console.ReadLine();
                    currentUser = userName;
                    Console.WriteLine("Make a password");
                    string passWord = Console.ReadLine();
                    Console.WriteLine();

                    try
                    {
                        string sqlusername = String.Format("INSERT INTO User (user, password) VALUES ('{0}', '{1}')", userName, passWord);
                        commanduser = new SQLiteCommand(sqlusername, m_dbConnection);
                        commanduser.ExecuteNonQuery();
                        Console.WriteLine("User created!");
                        Console.WriteLine();
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("You wrote an incorrect input");
                    Console.WriteLine();
                }
            }
            

            

            //The main program starts
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
                    string sqlCommand = "Select name, score, category from Scoreboard";
                    command = new SQLiteCommand(sqlCommand, m_dbConnection);
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine("Name: " + reader["name"] + " Score: " + reader["score"] + " Category: " + reader["category"]);
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
                    Console.WriteLine("Enter category");
                    string categoryInput = Console.ReadLine();
                    Console.WriteLine();
                    try
                    {
                        // find away to add currentUser into the scoreboard
                        insertScore = Convert.ToInt32(scoreInput);
                        sql = String.Format("INSERT INTO Scoreboard (name, score, category, user) VALUES ('{0}', '{1}','{2}','{3}')", insertName, insertScore, categoryInput, currentUser);
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
                    Console.WriteLine("You wrote an incorrect input");
                    Console.WriteLine();
                }
            }
            m_dbConnection.Close();
        }
    }
}
