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
                    "Ingredients_Recipies (" +
                    "ingredientID INTEGER NOT NULL, " +
                    "recipeID INTEGER NOT NULL, " +
                    "weight DOUBLE NOT NULL, " +
                    "CONSTRAINT FKingredientID FOREIGN KEY (ingredientID) REFERENCES Ingredients(ingredientID), " +
                    "CONSTRAINT FKrecipeID FOREIGN KEY (recipeID) REFERENCES Recipies(recipeID), " +
                    "CONSTRAINT IngredientsRecipesPK PRIMARY KEY (ingredientID, recipeID)" +
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
                    "CONSTRAINT FKuserID FOREIGN KEY (userID) REFERENCES Users(userID), " +
                    "CONSTRAINT FKrecipeID FOREIGN KEY (recipeID) REFERENCES Recipies(recipeID), " +
                    "CONSTRAINT UsersRecepiesPK PRIMARY KEY (userID, recipeID, date)" +
                    ");" +
                    "CREATE TABLE IF NOT EXISTS " +
                    "Burned (" +
                    "burnedID INTEGER PRIMARY KEY NOT NULL, " +
                    "calories DOUBLE, " +
                    "date DATE, " +
                    "userID INTEGER NOT NULL, " +
                    "CONSTRAINT FKuserID FOREIGN KEY (userID) REFERENCES Users(userID)" +
                    ");" +
                    "CREATE VIEW IF NOT EXISTS RecipeStat AS " +
                    "SELECT recipeID, recipeName, SUM(calories * ir.weight / 100) AS calories, " +
                    "SUM(proteins * ir.weight / 100) AS proteins, " +
                    "SUM(fats * ir.weight / 100) AS fats, SUM(carbs * ir.weight / 100) AS carbs " +
                    "FROM Ingredients_Recipies ir JOIN Ingredients i USING(ingredientID) " +
                    "JOIN Recipies r USING(recipeID) " +
                    "GROUP BY recipeID ",
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

        public static int GetUserID(string login)
        {
            using (SqliteConnection connection = new SqliteConnection($"Filename={dbpath}"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.CommandText = "SELECT userID FROM Users " +
                    "WHERE login = @login ";
                command.Parameters.AddWithValue("@login", login);
                command.Connection = connection;
                SqliteDataReader result = command.ExecuteReader();
                if (result.HasRows)
                    while (result.Read()) { 
                        return int.Parse(result["userID"].ToString());
                    }
                return -1;
            }
        }

        public static int GetUserStats(string userID)
        {
            return -1;
        }
    }

}
