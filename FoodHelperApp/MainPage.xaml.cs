using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using FoodHelperLibrary;
using Windows.Storage;
using Windows.UI.Core;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Contacts;
using Windows.UI.Composition;

namespace FoodHelperApp
{
	/// <summary>
	/// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		public User User { get; set; }
		public Stats Stats => FoodHelperDB.GetUserStats(User.Id, Period.Day);
		public int Burned => FoodHelperDB.GetUserStatsBurned(User.Id, Period.Day);
		public ObservableCollection<string> DisplayMealList
		{
			get
			{
				try { return new ObservableCollection<string>(from s in FoodHelperDB.GetUserAte(User.Id, Period.Day) select $"{s.Count} * {s.Name}"); }
				catch (ArgumentNullException) { return new ObservableCollection<string>(); }
			}
		}


		public MainPage()
		{
			this.InitializeComponent();
			//ивент при нажатии любой из кнопок обновляет данные в блоках

			SizeChanged += ResiseCheck;

			//BurnedToday.Text = FoodHelperDB.GetUserStatsBurned(User.Id, Period.Day).ToString();
			//Gained.Text = FoodHelperDB.GetUserStats(User.Id, Period.Day).cal.ToString();
			//Proteins.Text = FoodHelperDB.GetUserStats(User.Id, Period.Day).protein.ToString();
			//Fats.Text = FoodHelperDB.GetUserStats(User.Id, Period.Day).fat.ToString();
			//Carbs.Text = FoodHelperDB.GetUserStats(User.Id, Period.Day).carb.ToString();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{

			base.OnNavigatedTo(e);
			User = (User)e.Parameter;
			if (User.Remember)
			{
				ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue
				{
					["UserId"] = User.Id.ToString(),
					["UserLogin"] = User.Name,
					["UserPassword"] = User.Password
				};
				ApplicationData.Current.LocalSettings.Values["UserInfo"] = composite;
			}
		}


		private void ResiseCheck(object sender, SizeChangedEventArgs e)
		{
			if (e.NewSize.Height < 720)
			{
				BurnedIMG.Visibility = Visibility.Collapsed;
				CaloriesIMG.Visibility = Visibility.Collapsed;
				ProteinsIMG.Visibility = Visibility.Collapsed;
				FatsIMG.Visibility = Visibility.Collapsed;
				CarbsIMG.Visibility = Visibility.Collapsed;
			}
			else if (e.NewSize.Height >= 720)
			{
				BurnedIMG.Visibility = Visibility.Visible;
				CaloriesIMG.Visibility = Visibility.Visible;
				ProteinsIMG.Visibility = Visibility.Visible;
				FatsIMG.Visibility = Visibility.Visible;
				CarbsIMG.Visibility = Visibility.Visible;
			}
			if (e.NewSize.Width < 1024)
			{
				BurnedIMG.Visibility = Visibility.Collapsed;
				CaloriesIMG.Visibility = Visibility.Collapsed;
				ProteinsIMG.Visibility = Visibility.Collapsed;
				FatsIMG.Visibility = Visibility.Collapsed;
				CarbsIMG.Visibility = Visibility.Collapsed;
			}
			else if (e.NewSize.Width >= 1024)
			{
				BurnedIMG.Visibility = Visibility.Visible;
				CaloriesIMG.Visibility = Visibility.Visible;
				ProteinsIMG.Visibility = Visibility.Visible;
				FatsIMG.Visibility = Visibility.Visible;
				CarbsIMG.Visibility = Visibility.Visible;
			}
		}

		private void Exit_Click(object sender, RoutedEventArgs e)
		{
			if (Frame.CanGoBack)
				Frame.GoBack();
			else
				Frame.Navigate(typeof(Auth));

			ApplicationData.Current.LocalSettings.Values["UserInfo"] = null;
		}

		private void Close_Click(object sender, RoutedEventArgs e) => Application.Current.Exit();

		private void AddIngredient_Click(object sender, RoutedEventArgs e) => Frame.Navigate(typeof(AddIngredient));

		private void AddRecipe_Click(object sender, RoutedEventArgs e) => Frame.Navigate(typeof(AddRecipe));

		private void AddBurnedToday_Click(object sender, RoutedEventArgs e) => Frame.Navigate(typeof(AddBurned));

		private void AddAteToday_Click(object sender, RoutedEventArgs e) => Frame.Navigate(typeof(AddMeal));
	}
}
