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

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace FoodHelperApp
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {


        public MainPage()
        {
			//LoginText.Text = "s3rg0sh4<3Athenaja";
            this.InitializeComponent();
        }

		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{

		}

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
	}
}
