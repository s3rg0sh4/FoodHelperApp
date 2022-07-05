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
    public sealed partial class Register : Page
    {
        public Register()
        {
            this.InitializeComponent();
        }

        private void Registrate_Click(object sender, RoutedEventArgs e)
        {
            if (!FoodHelperDB.CheckUser(Login.Text, Password.Password))
            {
                FoodHelperDB.AddUser(Login.Text, Password.Password);
                Frame.Navigate(typeof(MainPage), new User(Login.Text, Remember.IsChecked));
            }
            else
                Frame.GoBack(); //Если юзер существует, доделать
        }

		private void Close_Click(object sender, RoutedEventArgs e) => Frame.GoBack();
	}
}
