namespace FoodHelperLibrary
{
	public struct User
	{
		public int Id { get; }
		public bool Remember { get; }

		public User(string name, bool? remember)
		{
			Id = FoodHelperDB.GetUserID(name);
			Remember = remember ?? false;
		}
	}
}
