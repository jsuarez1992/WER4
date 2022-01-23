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
            // commands used to make program work
            string input = "";
            string programQuit = "ok";
            string insertName = "";
            string scoreInput = "1";
            int insertScore = 0;
            string currentUser = "";
            string categoryInput = "";

            // these lines create a database incase there isn't one before
            string sql = "CREATE TABLE Scoreboard (name TEXT, score INTEGER, category TEXT, user TEXT)";
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
            string sqluser = "CREATE TABLE User (user TEXT, password TEXT)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteCommand commanduser = new SQLiteCommand(sqluser, m_dbConnection);
            if (!File.Exists("MyDatabase.sqlite"))
            {
                SQLiteConnection.CreateFile("MyDatabase.sqlite");

                m_dbConnection.Open();
                commanduser.ExecuteNonQuery();
                command.ExecuteNonQuery();
            }
            else
            {
                m_dbConnection.Open();
            }

            //User logs in, if username doesn't exist you can create a new user
            bool login = true;
            while (login)
            {
                string sqlLogin = "";
                string userPassword = "";
                string tempUser = "";
                programQuit="";
                Console.WriteLine("Enter your username:");
                string userId = Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine("Enter your password");
                userPassword = Console.ReadLine();
                try
                {
                    /*if (userId == userPassword)
                    {
                        Console.WriteLine("Silly, username can't be the same as password!");
                    }*/
                    sqlLogin = String.Format("Select user,password from User where user='{0}'", userId);
                    command = new SQLiteCommand(sqlLogin, m_dbConnection);
                    SQLiteDataReader reader = command.ExecuteReader();
                    reader.Read();
                    tempUser = Convert.ToString(reader["user"]);
                    string tempPassword = Convert.ToString(reader["password"]);
                    if (tempPassword == userPassword)
                    {
                        login = false;
                        currentUser = tempUser;
                    }
                    else
                    {
                        Console.WriteLine("Wrong password");
                    }
                }
                catch (Exception e)
                {
                    string message = "User not found!";
                    Exception e2 = (Exception)Activator.CreateInstance(e.GetType(), message, e);
                    Console.WriteLine(e2.Message);
                    Console.WriteLine();
                    Console.WriteLine("Do you want to create a user? Write yes if you want to");
                    string userDoesntExist = Console.ReadLine();
                    //currentUser = tempUser;
                    if (userDoesntExist == "yes")
                    {
                        Console.WriteLine();
                        string sqlusername = String.Format("INSERT INTO User (user, password) VALUES ('{0}', '{1}')", userId, userPassword);
                        currentUser=userId;
                        commanduser = new SQLiteCommand(sqlusername, m_dbConnection);
                        commanduser.ExecuteNonQuery();
                        Console.WriteLine("User created!");
                        login=false;
                    }
                    else
                    {
                        programQuit = "quit";
                        //changes
                        //break;
                    }
                }
            }

            //The main program starts
            if (programQuit != "quit")
            {
                while (true)
                {
                    Console.WriteLine("Show all the scores (Press 1)");
                    Console.WriteLine("Add a score for a person (Press 2)");
                    Console.WriteLine("Show a person's score (Press 3)");
                    Console.WriteLine("Delete a person's score (Press 4)");
                    Console.WriteLine("Change a person's score (Press 5)");
                    Console.WriteLine("Quit (Press 6)");
                    input = Console.ReadLine();

                    if (input == "6")
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
                        categoryInput = Console.ReadLine();
                        Console.WriteLine();
                        try
                        {
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
                        //changes
                        Console.WriteLine();
                        Console.WriteLine("Give me a name");
                        string personName = Console.ReadLine();
                        try
                        {
                            string sqlCommand = String.Format("Select name, score, category from Scoreboard where name='{0}'", personName);
                            command = new SQLiteCommand(sqlCommand, m_dbConnection);
                            SQLiteDataReader reader = command.ExecuteReader();
                            reader.Read();
                            Console.WriteLine("Name: " + reader["name"] + " Score: " + reader["score"] + " Category: " + reader["category"]);
                        }
                        catch (Exception e)
                        {
                            string message = "Wrong input";
                            Exception e2 = (Exception)Activator.CreateInstance(e.GetType(), message, e);
                            Console.WriteLine(e2.Message);
                            Console.WriteLine();
                        }
                        Console.WriteLine();
                    }
                    else if (input == "4")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Give me a name");
                        string personName = Console.ReadLine();

                        //changes
                        int personSCore = 0;
                        string personsUser = "";
                        try
                        {
                            string datasUserPerson = String.Format("Select name,user,score from Scoreboard where name='{0}'", personName);
                            command = new SQLiteCommand(datasUserPerson, m_dbConnection);
                            SQLiteDataReader reader = command.ExecuteReader();
                            reader.Read();
                            personSCore = Convert.ToInt32(reader["score"]);
                            string cUser = String.Format("Select user,name from Scoreboard where name='{0}'", reader["name"]);
                            command = new SQLiteCommand(cUser, m_dbConnection);
                            SQLiteDataReader secondReader = command.ExecuteReader();
                            secondReader.Read();
                            personsUser = Convert.ToString(secondReader["user"]);
                        }
                        catch (Exception e)
                        {
                            string message = "User not found!";
                            Exception e2 = (Exception)Activator.CreateInstance(e.GetType(), message, e);
                            Console.WriteLine(e2.Message);
                            Console.WriteLine();
                        }
                        if (personsUser == currentUser)
                        {
                            string deleteCommand = String.Format("DELETE FROM Scoreboard WHERE name='{0}'and score='{1}'", personName, personSCore);
                            command = new SQLiteCommand(deleteCommand, m_dbConnection);
                            command.ExecuteNonQuery();
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine("Your user do not have permission to change the database");
                        }
                    }
                    else if (input == "5")
                    {
                        string personsName = "";
                        string newScoreInput = "";
                        string datasUserPerson = "";
                        string personsUser="";
                        int newScore = 0;
                        Console.WriteLine();
                        Console.WriteLine("Give a person's name");
                        personsName = Console.ReadLine();
                        try
                        {
                            datasUserPerson = String.Format("Select score,user from Scoreboard where name='{0}'", personsName);
                            command = new SQLiteCommand(datasUserPerson, m_dbConnection);
                            SQLiteDataReader reader = command.ExecuteReader();
                            reader.Read();
                            Console.WriteLine("Current score: " + reader["score"]);
                            Console.WriteLine();
                            personsUser=Convert.ToString(reader["user"]);
                            Console.WriteLine("Give a new value of the score");
                            newScoreInput = Console.ReadLine();
                            newScore = Convert.ToInt32(newScoreInput);
                            if(currentUser==personsUser)
                            {
                            datasUserPerson = String.Format("UPDATE Scoreboard SET score='{0}' where name ='{1}'", newScoreInput, personsName);
                            command = new SQLiteCommand(datasUserPerson, m_dbConnection);
                            command.ExecuteNonQuery();
                            Console.WriteLine("It is updated");
                            Console.WriteLine();
                            }
                            else
                            {
                                Console.WriteLine("Your user do not have permission to change the database");
                            }
                        }
                        catch (Exception e)
                        {
                            string message = "Incorrect input";
                            Exception e2 = (Exception)Activator.CreateInstance(e.GetType(), message, e);
                            Console.WriteLine(e2.Message);
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("You wrote an incorrect input");
                        Console.WriteLine();
                    }
                }
                m_dbConnection.Close();
            }
            else
            {
                return;
            }
        }
    }
}