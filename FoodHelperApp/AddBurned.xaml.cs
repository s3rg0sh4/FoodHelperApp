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
using System.Drawing;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace FoodHelperApp
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class AddBurned : Page
    {
        public AddBurned()
        {
            this.InitializeComponent();
        }

		private void AddBurned_Click(object sender, RoutedEventArgs e)
		{
            try
            {
                FoodHelperDB.AddUserStatsBurned(int.Parse(Cals.Text), 1);
                Cals.PlaceholderForeground = new SolidColorBrush(Windows.UI.Color.FromArgb(255,50,50,50));
                Frame.GoBack();
            } catch (FormatException)
			{
                Cals.Text = "";
                Cals.PlaceholderText = "Введите число";
                Cals.PlaceholderForeground = new SolidColorBrush(Windows.UI.Colors.Red);
            }
		}

		private void Close_Click(object sender, RoutedEventArgs e) => Frame.GoBack();

	}
}
