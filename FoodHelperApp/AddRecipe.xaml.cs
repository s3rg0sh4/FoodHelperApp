using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// 
    

    public sealed partial class AddRecipe : Page
    {
        public ObservableCollection<string> CmbContent { get { return new ObservableCollection<string>(FoodHelperDB.GetIngredientList()); } }
        public AddRecipe()
        {
            
            this.InitializeComponent();
        }

		private void AddRecipe_Click(object sender, RoutedEventArgs e)
		{

		}

		private void Close_Click(object sender, RoutedEventArgs e) => Frame.GoBack();

        private void AddIngredient_Click(object sender, RoutedEventArgs e)
        {


		}

        
    }
}
