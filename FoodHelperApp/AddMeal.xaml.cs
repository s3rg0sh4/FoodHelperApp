using FoodHelperLibrary;
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

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace FoodHelperApp
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class AddMeal : Page
    {
        public ObservableCollection<string> CmbContent { get {
                try { return new ObservableCollection<string>(FoodHelperDB.GetRecipeAll()); }
                catch { return new ObservableCollection<string>(); }
            } }

        public AddMeal()
        {
            this.InitializeComponent();
        }

		private void AddMeal_Click(object sender, RoutedEventArgs e)
		{
            FoodHelperDB.AddUserAte(1, AddMealCmb.SelectedItem.ToString());
            Frame.GoBack();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
