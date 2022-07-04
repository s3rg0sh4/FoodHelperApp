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

namespace FoodHelperApp
{
	/// <summary>
	/// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		private ObservableCollection<(double cal, double protein, double fat, double carb)> userStats;
		public ObservableCollection<(double cal, double protein, double fat, double carb)> UserStats => userStats;

		private ObservableCollection<string> displayMealList;
		public ObservableCollection<string> DisplayMealList => displayMealList;
		public MainPage()
		{
			this.InitializeComponent();
			displayMealList = new ObservableCollection<string>();
			//ивент при нажатии любой из кнопок обновляет данные в блоках

			SizeChanged += ResiseCheck;

			BurnedToday.Text = FoodHelperDB.GetUserStatsBurned(1, 0).ToString();
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
				Application.Current.Exit();
		}

		private void AddIngredient_Click(object sender, RoutedEventArgs e) => Frame.Navigate(typeof(AddIngredient));

		private void AddRecipe_Click(object sender, RoutedEventArgs e) => Frame.Navigate(typeof(AddRecipe));

		private void AddBurnedToday_Click(object sender, RoutedEventArgs e) => Frame.Navigate(typeof(AddBurned));

		private void AddAteToday_Click(object sender, RoutedEventArgs e) => Frame.Navigate(typeof(AddMeal));
	}
}
