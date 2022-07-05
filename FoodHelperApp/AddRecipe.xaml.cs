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
        private int children = 1;
        public AddRecipe()
        {
            
            this.InitializeComponent();
        }

		private void AddRecipe_Click(object sender, RoutedEventArgs e)
		{
            
            FoodHelperDB.AddRecipe(RecipeNameField.Text, 
                new List<(string Name, int Weight)>(from a in CmbStack.Children join b in TbStack.Children 
                                                    on CmbStack.Children.IndexOf(a) equals TbStack.Children.IndexOf(b) 
                                                    select (((ComboBox)a).SelectedItem.ToString(), int.Parse(((TextBox)b).Text.ToString()))));

            children = 1;
            Frame.GoBack();
        }

        private void Close_Click(object sender, RoutedEventArgs e) 
        {
            children = 1;
            Frame.GoBack(); 
        }

        private void AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            CmbStack.Children.Add(new ComboBox() { Name = $"CmbIng{children}"});
            TbStack.Children.Add(new TextBox() { Name = $"TbIng{children}"});
            children++;
        }

        
    }
}
