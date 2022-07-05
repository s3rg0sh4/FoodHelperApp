namespace FoodHelperLibrary
{
	public struct Stats 
	{
		public double Calories { get; }
		public double Proteins { get; }
		public double Fats { get; }
		public double Carbs { get; }

		public Stats(double calories, double proteins, double fats, double carbs)
		{
			Calories = calories;
			Proteins = proteins;
			Fats = fats;
			Carbs = carbs;
		}
	}
}
