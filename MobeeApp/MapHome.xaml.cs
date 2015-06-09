using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.Devices.Geolocation;
using System.Device.Location;
using System.Windows.Shapes;
using System.Windows.Media;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Services;
using System.Text;

namespace MobeeApp
{
    public partial class MapHome : PhoneApplicationPage
    {
        public MapHome()
        {
            InitializeComponent();
            ShowLocation();
        }

        private async void ShowLocation()
        {
            // Get my current location.
            Geolocator myGeolocator = new Geolocator();
            Geoposition myGeoposition = await myGeolocator.GetGeopositionAsync();
            Geocoordinate myGeocoordinate = myGeoposition.Coordinate;
            GeoCoordinate myGeoCoordinate = CoordinateConverter.ConvertGeocoordinate(myGeocoordinate);

            // Make my current location the center of the Map.
            HomeLocation.Center = myGeoCoordinate;
            HomeLocation.ZoomLevel = 13;

            // Create a small circle to mark the current location.
            Ellipse myCircle = new Ellipse();
            myCircle.Fill = new SolidColorBrush(Colors.Blue);
            myCircle.Height = 20;
            myCircle.Width = 20;
            myCircle.Opacity = 50;

            // Create a MapOverlay to contain the circle.
            MapOverlay myLocationOverlay = new MapOverlay();
            myLocationOverlay.Content = myCircle;
            myLocationOverlay.PositionOrigin = new Point(0.5, 0.5);
            myLocationOverlay.GeoCoordinate = myGeoCoordinate;

            // Create a MapLayer to contain the MapOverlay.
            MapLayer myLocationLayer = new MapLayer();
            myLocationLayer.Add(myLocationOverlay);

            // Add the MapLayer to the Map.
            HomeLocation.Layers.Add(myLocationLayer);
        }

        private void HomeLocation_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            GeoCoordinate location = HomeLocation.ConvertViewportPointToGeoCoordinate(e.GetPosition(HomeLocation));
            System.Diagnostics.Debug.WriteLine("latitude :" + location.Latitude + ", longitude : " + location.Longitude);

            ReverseGeocodeQuery query = new ReverseGeocodeQuery()
            {
                GeoCoordinate = new GeoCoordinate(location.Latitude, location.Longitude)
            };
            query.QueryCompleted += LocationToAddress;
            query.QueryAsync();
        }

        void LocationToAddress(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            StringBuilder placeString = new StringBuilder();
            foreach (var place in e.Result)
            {
                placeString.AppendLine(place.GeoCoordinate.ToString());
                placeString.AppendLine(place.Information.Address.City);
                placeString.AppendLine(place.Information.Address.Country);
                placeString.AppendLine(place.Information.Address.HouseNumber);
                placeString.AppendLine(place.Information.Address.PostalCode);
                placeString.AppendLine(place.Information.Address.Province);
                placeString.AppendLine(place.Information.Address.State);
                placeString.AppendLine(place.Information.Address.Street);
                placeString.AppendLine(place.Information.Address.Township);
            }
            MessageBox.Show(placeString.ToString());
        }
    }
}