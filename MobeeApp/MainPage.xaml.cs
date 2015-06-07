using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MobeeApp.Resources;
using System.Diagnostics;

namespace MobeeApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructeur
        public MainPage()
        {
            InitializeComponent();

            // Exemple de code pour la localisation d'ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void Go_home(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Debug.WriteLine("My map");
            NavigationService.Navigate(new Uri("/Map_navigate.xaml", UriKind.Relative));
        }

        private void Parameters(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Debug.WriteLine("Configuration page");
            NavigationService.Navigate(new Uri("/Parameters.xaml", UriKind.Relative));
        }
    }
}