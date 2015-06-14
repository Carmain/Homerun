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
using System.Globalization;

namespace MobeeApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        private RecordManager recordManager = new RecordManager();

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
                if (recordManager.isExist("coordinate"))
                {
                    string coordinateToString = recordManager.read("coordinate");
                    string[] parts = coordinateToString.Split(',');
                    double latitude = Double.Parse(parts[0], CultureInfo.InvariantCulture);
                    double longitude = Double.Parse(parts[1], CultureInfo.InvariantCulture);

                    BingMapsDirectionsTask bingMapsDirectionsTask = new BingMapsDirectionsTask();
                    GeoCoordinate spaceNeedleLocation = new GeoCoordinate(latitude, longitude);
                    LabeledMapLocation spaceNeedleLML = new LabeledMapLocation(AppResources.GPS, spaceNeedleLocation);
                    bingMapsDirectionsTask.End = spaceNeedleLML;
                    bingMapsDirectionsTask.Show();
                }
                else
                {
                    MessageBox.Show(AppResources.ErrorCoordinateRegister);
                }
            }
        }
            }
        }

        private void Parameters(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Parameters.xaml", UriKind.Relative));
        }
    }
}