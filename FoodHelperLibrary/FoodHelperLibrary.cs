using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Windows.Storage;

namespace FoodHelperLibrary
{
    public static class FoodHelperDB
    {
        readonly static string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "FoodHelper.db");
        public async static void InitializeDatabase()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync("FoodHelper.db", CreationCollisionOption.OpenIfExists);
            //string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "FoodHelper.db");
            using (SqliteConnection connection = new SqliteConnection($"Filename={dbpath}"))
            {
                connection.Open();

                new SqliteCommand(
                    "CREATE TABLE IF NOT EXISTS " +
                    "Ingredients (" +
                    "ingredientID INTEGER PRIMARY KEY NOT NULL, " +
                    "ingredientName CHARACTER(30) NOT NULL, " +
                    "calories DOUBLE NOT NULL, " +
                    "proteins DOUBLE NOT NULL, " +
                    "fats DOUBLE NOT NULL, " +
                    "carbs DOUBLE NOT NULL" +
                    ");" +
                    "CREATE TABLE IF NOT EXISTS " +
                    "Recipies (" +
                    "recipeID INTEGER PRIMARY KEY NOT NULL, " +
                    "recipeName CHARACTER(50) NOT NULL, " +
                    "weight DOUBLE NOT NULL" +
                    ");" +
                    "CREATE TABLE IF NOT EXISTS " +
                    "Ingredients_Recipes (" +
                    "ingredientID INTEGER NOT NULL, " +
                    "recipeID INTEGER NOT NULL, " +
                    "weight DOUBLE NOT NULL, " +
                    "FOREIGN FKingredientID KEY (ingredientID) REFERENCES Ingredients(ingredientID), " +
                    "FOREIGN FKrecipeID KEY (recipeID) REFERENCES Recipies(recipeID), " +
                    "PRIMARY KEY (ingredientID, recipeID)" +
                    ");" +
                    "CREATE TABLE IF NOT EXISTS " +
                    "Users (" +
                    "userID INTEGER PRIMARY KEY NOT NULL, " +
                    "login CHARACTER(20) NOT NULL UNIQUE, " +
                    "password CHARACTER(20) NOT NULL" +
					"" +
                    ");" +
                    "CREATE TABLE IF NOT EXISTS " +
                    "Users_Recepies (" +
                    "userID INTEGER NOT NULL, " +
                    "recipeID INTEGER NOT NULL, " +
                    "date DATE NOT NULL, " +
                    "count INTEGER, " +
                    "FOREIGN KEY FKuserID (userID) REFERENCES Users(userID), " +
                    "FOREIGN KEY FKrecipeID (recipeID) REFERENCES Recipies(recipeID), " +
                    "PRIMARY KEY UsersRecepiesPK (userID, recipeID, date)" +
                    ");" +
                    "CREATE TABLE IF NOT EXISTS " +
                    "Burned (" +
                    "burnedID INTEGER PRIMARY KEY NOT NULL, " +
                    "calories DOUBLE, " +
                    "date DATE, " +
                    "userID INTEGER NOT NULL, " +
                    "FOREIGN KEY FKuserID (userID) REFERENCES Users(userID)" +
                    ");", 
                    connection).ExecuteReader();
            }
        }

        public static void AddUser(string login, string password)
		{
            using (SqliteConnection connection = new SqliteConnection($"Filename={dbpath}"))
			{
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;
                command.CommandText = "INSERT INTO Users VALUES(@login,@password)";
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@password", password);
                command.ExecuteNonQuery();

			}
		}

        public static bool GetUserList(string login, string password)
		{

            using (SqliteConnection connection = new SqliteConnection($"Filename={dbpath}"))
			{
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.CommandText = "SELECT * FROM Users" +
                    "WHERE login=@login and password=@password";
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@password", password);
                return command.ExecuteReader().HasRows;
            }
		}
    }

}
