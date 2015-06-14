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
using Microsoft.Phone.Maps.Services;

namespace MobeeApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        private RecordManager recordManager = new RecordManager();
        private Geolocator geolocator = new Geolocator();

        public MainPage()
        {
            InitializeComponent();
        }

        private void BackHome(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (geolocator.LocationStatus == PositionStatus.Disabled)
            {
                MessageBox.Show(AppResources.LocationDisabled);
            }
            else
            {
                if (recordManager.isExist("coordinate"))
                {
                    string coordinateToString = recordManager.Read("coordinate");
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

        private async void SendSMS(object sender, System.Windows.Input.GestureEventArgs e)
        {

            if (recordManager.isExist("contactPhone") && geolocator.LocationStatus != PositionStatus.Disabled)
            {
                Geoposition myGeoposition = await geolocator.GetGeopositionAsync();
                Geocoordinate coordinate = myGeoposition.Coordinate;
                GeoCoordinate location = CoordinateConverter.ConvertGeocoordinate(coordinate);

                ReverseGeocodeQuery query = new ReverseGeocodeQuery()
                {
                    GeoCoordinate = location
                };
                query.QueryCompleted += LocationToAddress;
                query.QueryAsync();
            }
            else
            {
                MessageBox.Show(AppResources.ErrorCoordinateRegister);
            }
        }

        private void LocationToAddress(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            List<String> addressElements = new List<string>();
            GeoCoordinate coordinate = null;
            foreach (var place in e.Result)
            {
                addressElements.Add(place.Information.Address.HouseNumber);
                addressElements.Add(place.Information.Address.Street);
                addressElements.Add(place.Information.Address.City);
                addressElements.Add("(" + place.Information.Address.State + ")");
                coordinate = place.GeoCoordinate;
                break;
            }

            if (e.Result.Count > 0)
            {
                // Send an SMS with the address
                string address = string.Join(" ", addressElements.ToArray());
                SmsComposeTask smsComposeTask = new SmsComposeTask();
                smsComposeTask.To = recordManager.Read("contactPhone");
                smsComposeTask.Body = AppResources.SMSPart1 + " " + address + " " + AppResources.SMSPart2;
                smsComposeTask.Show();
            }
        }

        private void Parameters(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Parameters.xaml", UriKind.Relative));
        }
    }
}