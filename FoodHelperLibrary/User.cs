namespace FoodHelperLibrary
{
	public struct User
	{
		public int Id { get; }
		public string Name { get; }
		public string Password { get; }
		public bool Remember { get; }

		public User(string name, string password, bool? remember)
		{
			Id = FoodHelperDB.GetUserID(name);
			Name = name;
			Password = password;
			Remember = remember ?? false;
		}

		public User(int id, string name, string password)
		{
			Id = id;
			Name = name;
			Password = password;
			Remember = true;
		}
	}
}
