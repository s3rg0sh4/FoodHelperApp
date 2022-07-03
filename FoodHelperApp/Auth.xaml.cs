using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using FoodHelperLibrary;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace FoodHelperApp
{
	/// <summary>
	/// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
	/// </summary>
	public sealed partial class Auth : Page
	{
		public Auth()
		{

			this.InitializeComponent();
		}

        private void Auth_Click(object sender, RoutedEventArgs e)
		{
			bool remember = Remember.IsChecked != null && Remember.IsChecked != false;

			if (FoodHelperDB.CheckUser(Login.Text, Password.Password)) 
			{
				FoodHelperDB.UpdateUserRemember(Login.Text, remember);
				Frame.Navigate(typeof(MainPage)); 
			}
			else
			{
				Login.Text = "";
				Password.Password = "";
			}
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
			Frame.Navigate(typeof(Register));
        }
    }
}
