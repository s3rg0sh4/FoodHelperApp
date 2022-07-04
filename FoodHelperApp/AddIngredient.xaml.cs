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

namespace FoodHelperApp
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class AddIngredient : Page
    {
        public AddIngredient()
        {
            this.InitializeComponent();
        }

		private void AddIngredientButton_Click(object sender, RoutedEventArgs e)
		{
            try
            {
                FoodHelperDB.AddIngredient(NameIngField.Text, int.Parse(CountCal.Text),
                                int.Parse(CountProtein.Text), int.Parse(CountrFat.Text), int.Parse(CountCarb.Text));
                Frame.GoBack();
            } catch (Exception) { 
                //Сделай, пожалуйста, красиво <3
            };
		}

        private void Close_Click(object sender, RoutedEventArgs e) => Frame.GoBack();
    }
}
