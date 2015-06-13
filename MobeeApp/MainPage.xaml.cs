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
using Windows.Devices.Geolocation;
using Microsoft.Phone.Tasks;
using System.Device.Location;

namespace MobeeApp
{
    public partial class MainPage : PhoneApplicationPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private void BackHome(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Geolocator geolocator = new Geolocator();
            if (geolocator.LocationStatus == PositionStatus.Disabled)
            {
                MessageBox.Show(AppResources.LocationDisabled);
            }
            else
            {

                BingMapsDirectionsTask bingMapsDirectionsTask = new BingMapsDirectionsTask();
                GeoCoordinate spaceNeedleLocation = new GeoCoordinate(47.6204, -122.3493);
                LabeledMapLocation spaceNeedleLML = new LabeledMapLocation(AppResources.GPS, spaceNeedleLocation);
                bingMapsDirectionsTask.End = spaceNeedleLML;
                bingMapsDirectionsTask.Show();
            }
        }

        private void Parameters(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Parameters.xaml", UriKind.Relative));
        }
    }
}