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

        private static string today = DateTime.Now.ToString("yyyy-mm-dd");

        public async static void InitializeDatabase()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync("FoodHelper.db", CreationCollisionOption.OpenIfExists);
            //string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "FoodHelper.db");
            using (SqliteConnection connection = new SqliteConnection($"Filename={dbpath}"))
            {
                connection.Open();
                SqliteCommand createIngredientsTable = new SqliteCommand(
                    "CREATE TABLE IF NOT EXISTS " +
                    "Ingredients (" +
                    "ingredientID INTEGER PRIMARY KEY NOT NULL, " +
                    "ingredientName CHARACTER(30)  NOT NULL, " +
                    "calories DOUBLE NOT NULL, " +
                    "proteins DOUBLE NOT NULL, " +
                    "fats DOUBLE NOT NULL, " +
                    "carbs DOUBLE NOT NULL" +
                    ");", connection);

                SqliteCommand createRecipiesTable = new SqliteCommand(
                    "CREATE TABLE IF NOT EXISTS " +
                    "Recipies (" +
                    "recipeID INTEGER PRIMARY KEY NOT NULL, " +
                    "recipeName CHARACTER(50) NOT NULL " +
                    ");", connection);

                SqliteCommand createIngredientsRecipiesTable = new SqliteCommand(
                    "CREATE TABLE IF NOT EXISTS " +
                    "Ingredients_Recipies (" +
                    "ingredientID INTEGER NOT NULL, " +
                    "recipeID INTEGER NOT NULL, " +
                    "weight DOUBLE NOT NULL, " +
                    "CONSTRAINT FKingredientID FOREIGN KEY (ingredientID) REFERENCES Ingredients(ingredientID), " +
                    "CONSTRAINT FKrecipeID FOREIGN KEY (recipeID) REFERENCES Recipies(recipeID), " +
                    "CONSTRAINT IngredientsRecipesPK PRIMARY KEY (ingredientID, recipeID)" +
                    ");", connection);

                SqliteCommand createUsersTable = new SqliteCommand(
                    "CREATE TABLE IF NOT EXISTS " +
                    "Users (" +
                    "userID INTEGER PRIMARY KEY NOT NULL, " +
                    "login CHARACTER(20) NOT NULL UNIQUE, " +
                    "password CHARACTER(20) NOT NULL " +
                    ");", connection);

                SqliteCommand createUsersRecipiesTable = new SqliteCommand(
                    "CREATE TABLE IF NOT EXISTS " +
                    "Users_Recipies (" +
                    "userID INTEGER NOT NULL, " +
                    "recipeID INTEGER NOT NULL, " +
                    "date DATE NOT NULL, " +
                    "count INTEGER, " +
                    "CONSTRAINT FKuserID FOREIGN KEY (userID) REFERENCES Users(userID), " +
                    "CONSTRAINT FKrecipeID FOREIGN KEY (recipeID) REFERENCES Recipies(recipeID), " +
                    "CONSTRAINT UsersRecepiesPK PRIMARY KEY (userID, recipeID, date)" +
                    ");", connection);

                SqliteCommand createBurnedTable = new SqliteCommand(
                    "CREATE TABLE IF NOT EXISTS " +
                    "Burned (" +
                    "burnedID INTEGER PRIMARY KEY NOT NULL, " +
                    "calories DOUBLE, " +
                    "date DATE, " +
                    "userID INTEGER NOT NULL, " +
                    "CONSTRAINT FKuserID FOREIGN KEY (userID) REFERENCES Users(userID)" +
                    ");", connection);

                SqliteCommand createRecipeStatView = new SqliteCommand(
                    "CREATE VIEW IF NOT EXISTS RecipeStat AS " +
                    "SELECT recipeID, recipeName, SUM(calories * ir.weight / 100) AS calories, " +
                    "SUM(proteins * ir.weight / 100) AS proteins, " +
                    "SUM(fats * ir.weight / 100) AS fats, SUM(carbs * ir.weight / 100) AS carbs " +
                    "FROM Ingredients_Recipies ir " +
					"JOIN Ingredients i USING(ingredientID) " +
                    "JOIN Recipies r USING(recipeID) " +
                    "GROUP BY recipeID;", connection);

                createIngredientsTable.ExecuteNonQuery();
                createRecipiesTable.ExecuteNonQuery();
                createIngredientsRecipiesTable.ExecuteNonQuery();
                createUsersTable.ExecuteNonQuery();
                createUsersRecipiesTable.ExecuteNonQuery();
                createBurnedTable.ExecuteNonQuery();
                createRecipeStatView.ExecuteNonQuery();
            }
        }

        public static void AddUser(string login, string password)
		{
            using (SqliteConnection connection = new SqliteConnection($"Filename={dbpath}"))
			{
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;
                command.CommandText = "INSERT INTO Users(login, password) VALUES(@login,@password); ";
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@password", password);
                command.ExecuteNonQuery();
			}
		}

        public static bool CheckUser(string login, string password)
		{
            using (SqliteConnection connection = new SqliteConnection($"Filename={dbpath}"))
			{
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM Users " +
                    "WHERE login LIKE @login AND password LIKE @password; ";
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@password", password);
                return command.ExecuteReader().HasRows;
            }
		}

        public static bool CheckUserLogin(string login)
		{
            using (SqliteConnection connection = new SqliteConnection($"Filename={dbpath}"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM Users " +
                    "WHERE login LIKE @login; ";
                command.Parameters.AddWithValue("@login", login);
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
                    while (result.Read()) 
                    { 
                        return int.Parse(result["userID"].ToString());
                    }
                return -1;
            }
        }

        public static int GetUserStatsBurned(int userID, int days)
        {
            using (SqliteConnection connection = new SqliteConnection($"Filename={dbpath}"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                switch (days)
                {
                    case 0:
                        command.CommandText = "SELECT SUM(calories) AS calories FROM Burned b " +
                    "WHERE userID = @userID AND statDay BETWEEN DATE('now', '-0 days') AND DATE('now'); ";
                        break;
                    case 7:
                        command.CommandText = "SELECT SUM(calories) AS calories FROM Burned b " +
                    "WHERE userID = @userID AND statDay BETWEEN DATE('now', '-7 days') AND DATE('now'); ";
                        break;
                    case 30:
                        command.CommandText = "SELECT SUM(calories) AS calories FROM Burned b " +
                    "WHERE userID = @userID AND statDay BETWEEN DATE('now', '-30 days') AND DATE('now'); ";
                        break;
                }
                command.Parameters.AddWithValue("@userID", userID);
                command.Connection = connection;
                SqliteDataReader result = command.ExecuteReader();
                if (result.HasRows)
                    while (result.Read())
                    {
                        return int.Parse(result["calories"].ToString());
                    }
                return -1;
            }
        }

        public static void AddUserStatsBurned(int burnedCal, int userID)
        {
            using (SqliteConnection connection = new SqliteConnection($"Filename={dbpath}"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.CommandText = "SELECT calories FROM Burned b WHERE userID = 1 AND `date` = DATE('now'); ";
                command.Parameters.AddWithValue("@userID", userID);
                command.Connection = connection;
                SqliteDataReader result = command.ExecuteReader();
                if (result.HasRows)
                {
                    command = new SqliteCommand();
                    command.CommandText = "UPDATE Burned SET calories = calories + @burnedCal " +
                        "WHERE userID = @userID AND `date`= Date('now'); ";
                    command.Parameters.AddWithValue("@burnedCal", burnedCal);
                    command.Parameters.AddWithValue("@userID", userID); ;
                    command.Connection = connection;
                    command.ExecuteNonQuery();
                }
                else
                {
                    command = new SqliteCommand();
                    command.CommandText = "INSERT INTO Burned(calories, `date`, userID) VALUES(@burnedCal, DATE('now'), @userID); ";
                    command.Parameters.AddWithValue("@burnedCal", burnedCal);
                    command.Parameters.AddWithValue("@userID", userID); ;
                    command.Connection = connection;
                    command.ExecuteNonQuery();
                }
            }
        }

        public static (double cal, double protein, double fat, double carb) GetUserStats(int userID, int days)
        {
            using (SqliteConnection connection = new SqliteConnection($"Filename={dbpath}"))
            {
                //var stats = new List<(double cal, double protein, double fat, double carb)>();
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                switch (days)
                {
                    case 0:
                        command.CommandText = "SELECT SUM(calories*`count`) AS cal, " +
                    "SUM(proteins*`count`) AS protein, " +
                    "SUM(fats*`count`) AS fat, SUM(carbs*`count`) AS carb FROM RecipeStat rs " +
                    "JOIN Users_Recipies ur USING(recipeID) " +
                    "WHERE userID = @userID AND `date` BETWEEN DATE('now', '-0 days') AND Date('now'); ";
                        break;
                    case 7:
                        command.CommandText = "SELECT SUM(calories*`count`) AS cal, " +
                    "SUM(proteins*`count`) AS protein, " +
                    "SUM(fats*`count`) AS fat, SUM(carbs*`count`) AS carb FROM RecipeStat rs " +
                    "JOIN Users_Recipies ur USING(recipeID) " +
                    "WHERE userID = @userID AND `date` BETWEEN DATE('now', '-7 days') AND Date('now'); ";
                        break;
                    case 30:
                        command.CommandText = "SELECT SUM(calories*`count`) AS cal, " +
                    "SUM(proteins*`count`) AS protein, " +
                    "SUM(fats*`count`) AS fat, SUM(carbs*`count`) AS carb FROM RecipeStat rs " +
                    "JOIN Users_Recipies ur USING(recipeID) " +
                    "WHERE userID = @userID AND `date` BETWEEN DATE('now', '-30 days') AND Date('now'); ";
                        break;

                }
                command.Parameters.AddWithValue("@userID", userID);
                command.Connection = connection;
                SqliteDataReader result = command.ExecuteReader();
                if (result.HasRows)
                    while (result.Read())
                    {
                        return (double.Parse(result["cal"].ToString()),
                            double.Parse(result["protein"].ToString()),
                            double.Parse(result["fat"].ToString()),
                            double.Parse(result["carb"].ToString()));
                    }
                return (0, 0, 0, 0);
            }
        }

        public static List<(string Name, int Count)> GetUserAte(int userID, int days)
        {
            using (SqliteConnection connection = new SqliteConnection($"Filename={dbpath}"))
            {
                var stats = new List<(string Name, int Count)>();
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                switch (days)
                {
                    case 0:
                        command.CommandText = "SELECT recipeName, count " +
                    "FROM Users_Recipies ur " +
                    "JOIN Recipies r USING(recipeID) " +
                    "WHERE userID = @userID AND `date` BETWEEN DATE('now', '-0 days') AND Date('now'); ";
                        break;
                    case 7:
                        command.CommandText = "SELECT recipeName, count " +
                    "FROM Users_Recipies ur " +
                    "JOIN Recipies r USING(recipeID) " +
                    "WHERE userID = @userID AND `date` BETWEEN DATE('now', '-7 days') AND Date('now'); ";
                        break;
                    case 30:
                        command.CommandText = "SELECT recipeName, count " +
                    "FROM Users_Recipies ur " +
                    "JOIN Recipies r USING(recipeID) " +
                    "WHERE userID = @userID AND `date` BETWEEN DATE('now', '-30 days') AND Date('now'); ";
                        break;
                }
                
                command.Parameters.AddWithValue("@userID", userID);
                command.Parameters.AddWithValue("@days", days);
                command.Connection = connection;
                SqliteDataReader result = command.ExecuteReader();
                if (result.HasRows)
                {
                    while (result.Read()) stats.Add((result["recipeName"].ToString(), int.Parse(result["count"].ToString())));
                }
                else return null;
                return stats;
            }
        }

        public static void AddUserAte(int userID, string recipeName)
        {
            using (SqliteConnection connection = new SqliteConnection($"Filename={dbpath}"))
            {
                int recipeID = -1;
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.CommandText = "SELECT recipeID FROM Recipies r WHERE recipeName = @recipeName; ";
                command.Parameters.AddWithValue("@recipeName", recipeName);
                command.Connection = connection;
                SqliteDataReader result = command.ExecuteReader();
                if (result.HasRows)
                    while (result.Read())
                    {
                         recipeID = int.Parse(result["recipeID"].ToString());
                    }

                command = new SqliteCommand();
                command.CommandText = "SELECT userID, recipeID, `date` FROM Users_Recipies ur " +
                    "WHERE recipeID = @recipeID AND userID = @userID AND `date`= Date('now'); ";
                command.Parameters.AddWithValue("@recipeID", recipeID);
                command.Parameters.AddWithValue("@userID", userID);
                command.Connection = connection;
                result = command.ExecuteReader();
                if (result.HasRows) {
                    command = new SqliteCommand();
                    command.CommandText = "UPDATE Users_Recipies SET count = count + 1 " +
                        "WHERE recipeID = @recipeID AND userID = @userID AND `date`= Date('now'); ";
                    command.Parameters.AddWithValue("@recipeID", recipeID);
                    command.Parameters.AddWithValue("@userID", userID); ;
                    command.Connection = connection;
                    command.ExecuteNonQuery();
                }
                else
                {
                    command = new SqliteCommand();
                    command.CommandText = "INSERT INTO Users_Recipies(userID, recipeID, `date`, `count`) " +
                        "VALUES(@userID, @recipeID, Date('now'), 1); ";
                    command.Parameters.AddWithValue("@recipeID", recipeID);
                    command.Parameters.AddWithValue("@userID", userID); ;
                    command.Connection = connection;
                    command.ExecuteNonQuery();
                }

            }
        }

        public static List<string> GetRecipeAll()
        {
            using (SqliteConnection connection = new SqliteConnection($"Filename={dbpath}"))
            {
                var recipiesList = new List<string>();
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.CommandText = "SELECT recipeName  FROM Recipies r; ";
                command.Connection = connection;
                SqliteDataReader result = command.ExecuteReader();
                if (result.HasRows) while (result.Read()) recipiesList.Add(result["recipeName"].ToString());
                else return null;
                return recipiesList;
            }
        }

        public static List<string> GetIngredientList()
        {
            using (SqliteConnection connection = new SqliteConnection($"Filename={dbpath}"))
            {
                var IngredientsList = new List<string>();
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.CommandText = "SELECT ingredientName FROM Ingredients i;";
                command.Connection = connection;
                SqliteDataReader result = command.ExecuteReader();
                if (result.HasRows) while (result.Read()) IngredientsList.Add(result["ingredientName"].ToString());
                else return null;
                return IngredientsList;
            }
        }

        public static void AddIngredient(string ingredientName, int cals, int proteins, int fats, int carbs)
        {
            using (SqliteConnection connection = new SqliteConnection($"Filename={dbpath}"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.CommandText = "INSERT INTO Ingredients(ingredientName, calories, proteins, fats, carbs) " +
                    "VALUES(@ingredientName, @cals, @proteins, @fats, @carbs);";
                command.Parameters.AddWithValue("@ingredientName", ingredientName);
                command.Parameters.AddWithValue("@cals", cals);
                command.Parameters.AddWithValue("@proteins", proteins);
                command.Parameters.AddWithValue("@fats", fats);
                command.Parameters.AddWithValue("@carbs", carbs);
                command.Connection = connection;
                command.ExecuteNonQuery();
            }
        }

        public static void AddRecipe(string recipeName, List<(string Name, int Weight)> ingredients)
        {
            using (SqliteConnection connection = new SqliteConnection($"Filename={dbpath}"))
            {
                int recipeID = -1;
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.CommandText = "INSERT INTO Recipies(recipeName) VALUES('@recipeName') RETURNING recipeID;";
                command.Parameters.AddWithValue("@recipeName", recipeName);
                command.Connection = connection;
                SqliteDataReader result = command.ExecuteReader();
                if (result.HasRows)
                    while (result.Read())
                    {
                        recipeID = int.Parse(result["recipeID"].ToString());
                    }
                else throw new Exception("Нет id");

                List<(int Id, int Weight)> ids = new List<(int, int)>();

                foreach (var ingredient in ingredients)
                {
                    command = new SqliteCommand();
                    command.CommandText = "SELECT ingredientID  FROM Ingredients i WHERE ingredientName = '@ingredientName'; ";
                    command.Parameters.AddWithValue("@ingredientName", ingredient.Name);
                    command.Connection = connection;
                    result = command.ExecuteReader();
                    if (result.HasRows)
                    {
                        result.Read();
                        ids.Add((int.Parse(result["ingredientID"].ToString()), ingredient.Weight));
                    }
                }

                foreach (var id in ids)
                {
                    command = new SqliteCommand();
                    command.CommandText = "INSERT INTO Ingredients_Recipies(ingredientID, recipeID, weight) " +
                        "VALUES(@ingredientID, @recipeID, @weight); ";
                    command.Parameters.AddWithValue("@ingredientID", id.Id);
                    command.Parameters.AddWithValue("@recipeID", recipeID);
                    command.Parameters.AddWithValue("@weight", id.Weight);
                    command.Connection = connection;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
