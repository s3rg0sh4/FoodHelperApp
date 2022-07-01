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

                String tableCommand =
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
                    "FOREIGN KEY (ingredientID) REFERENCES Ingredients(ingredientID), " +
                    "FOREIGN KEY (recipeID) REFERENCES Recipies(recipeID), " +
                    "PRIMARY KEY (ingredientID, recipeID)" +
                    ");" +
                    "CREATE TABLE IF NOT EXISTS " +
                    "Users (" +
                    "userID INTEGER PRIMARY KEY NOT NULL, " +
                    "login CHARACTER(20) NOT NULL, " +
                    "password CHARACTER(20) NOT NULL" +
                    ");" +
                    "CREATE TABLE IF NOT EXISTS " +
                    "Users_Recepies (" +
                    "userID INTEGER NOT NULL, " +
                    "recipeID INTEGER NOT NULL, " +
                    "date DATE NOT NULL, " +
                    "count INTEGER, " +
                    "FOREIGN KEY (userID) REFERENCES Users(userID), " +
                    "FOREIGN KEY (recipeID) REFERENCES Recipies(recipeID), " +
                    "PRIMARY KEY (userID, recipeID, date)" +
                    ");" +
                    "CREATE TABLE IF NOT EXISTS " +
                    "Burned (" +
                    "burnedID INTEGER PRIMARY KEY NOT NULL, " +
                    "calories DOUBLE, " +
                    "date DATE, " +
                    "userID INTEGER NOT NULL, " +
                    "FOREIGN KEY (userID) REFERENCES Users(userID)" +
                    ");";

                SqliteCommand createTable = new SqliteCommand(tableCommand, connection);

                createTable.ExecuteReader();
            }
        }

        public static void AddUser(string login, string password)
		{
            using (SqliteConnection connection = new SqliteConnection($"Filename={dbpath}"))
			{
                connection.Open();
                SqliteCommand addUser = new SqliteCommand();
                addUser.Connection = connection;
                addUser.CommandText = "INSERT INTO Users VALUES(@login,@password)";
                addUser.Parameters.AddWithValue("@login", login);
                addUser.Parameters.AddWithValue("@password", password);
			}
		}

        public static void CheckUser(string login, string password)
		{
            using (SqliteConnection connection = new SqliteConnection())
			{
                connection.Open();

			}
		}
    }

}
