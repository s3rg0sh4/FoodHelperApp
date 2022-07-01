using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodHelperApp.Objects
{
	public class User
	{
		[Key]
		public int UserID { get; set; }
		public string Login { get; set; }
		public string Password { get; set; }

	}
}
