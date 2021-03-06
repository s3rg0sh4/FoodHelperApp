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
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FoodHelperApp
{
	/// <summary>
	/// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
	/// </summary>
	public sealed partial class MainPage : Page, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		private Period period;
		public Period Period 
		{ 
			get { return period; } 
			set 
			{ 
				period = value;
				NotifyPropertyChanged("Stats");
				NotifyPropertyChanged("Burned");
				NotifyPropertyChanged("DisplayMealList");
			} 
		}
		
		public User User { get; set; }
		public Stats Stats => FoodHelperDB.GetUserStats(User.Id, Period);
		public int Burned => FoodHelperDB.GetUserStatsBurned(User.Id, Period);
		public ObservableCollection<string> DisplayMealList
		{
			get
			{
				try { return new ObservableCollection<string>(from s in FoodHelperDB.GetUserAte(User.Id, Period) select $"{s.Count} * {s.Name}"); }
				catch (ArgumentNullException) { return new ObservableCollection<string>(); }
			}
		}


		public MainPage()
		{
			this.InitializeComponent();
			LeftButton.Visibility = Visibility.Collapsed;
		}

		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        private void PreviousPeriod_Click(object sender, RoutedEventArgs e) => UpdateBlockInfo(--Period);

        private void NextPeriod_Click(object sender, RoutedEventArgs e) => UpdateBlockInfo(++Period);

        private void UpdateBlockInfo(Period period)
        {
			switch (Period)
			{
				case Period.Day:
					BlockInfo.Text = "Представлены данные за сегодня";
					LeftButton.Visibility = Visibility.Collapsed;
					RightButton.Visibility = Visibility.Visible;
					break;
				case Period.Week:
					BlockInfo.Text = "Представлены данные за неделю";
					LeftButton.Visibility = Visibility.Visible;
					RightButton.Visibility = Visibility.Visible;
					break;
				case Period.Month:
					BlockInfo.Text = "Представлены данные за месяц";
					LeftButton.Visibility = Visibility.Visible;
					RightButton.Visibility = Visibility.Collapsed;
					break;

			}
		}
	}
}
