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
		const string login = "s3rg0sh4<3Athenaja";
		const string password = "qwerty";
		private List<string> mealList = new List<string>();
		private ObservableCollection<string> displayMealList = new ObservableCollection<string>();
		int pageIndex = -1;
		bool isLastPage = false;
		bool isFirstPage = false;
		const int pageSize = 5;

		public MainPage()
		{
			this.InitializeComponent();
			mealList = new List<string>() { "Aboba1", "Aboba2", "Aboba3", "Aboba4", "Aboba5", "Aboba6", "Aboba7", "Aboba8", "Aboba9", "Aboba0" };
			NextButton_Click(null, null);
			ArrowLeft.Visibility = Visibility.Collapsed;
			SizeChanged += ResiseCheck;

		}

		public ObservableCollection<string> DisplayMealList
		{
			get { return this.displayMealList; }
		}

		void ResiseCheck(object sender, SizeChangedEventArgs e)
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

		private void ExitButton_Click(object sender, RoutedEventArgs e) => Application.Current.Exit();

		private void AddIngredientButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void AddRecipeButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void AddBurnedToday_Click(object sender, RoutedEventArgs e)
		{

		}

		private void AddAteToday_Click(object sender, RoutedEventArgs e)
		{

		}


		private void NextButton_Click(object sender, RoutedEventArgs e)
		{
			pageIndex++;
			int pageCount = mealList.Count / pageSize;
			isFirstPage = pageIndex == 0;
			displayMealList = new ObservableCollection<string>(mealList.Skip(pageIndex * pageSize).Take(pageSize).ToList());
			ArrowRight.Visibility = isLastPage ? Visibility.Collapsed : Visibility.Visible;
		}

		private void PreviousButton_Click(object sender, RoutedEventArgs e)
		{
			pageIndex--;
			int pageCount = mealList.Count / pageSize;
			isLastPage = pageCount == pageIndex;
			displayMealList = new ObservableCollection<string>(mealList.Skip(pageIndex * pageSize).Take(pageSize).ToList());
			ArrowLeft.Visibility = isFirstPage ? Visibility.Collapsed : Visibility.Visible;
		}
	}
}
