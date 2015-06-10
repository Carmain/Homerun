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
using MobeeApp.Resources;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Globalization;

namespace MobeeApp
{
    public partial class MapHome : PhoneApplicationPage
    {
        private RecordManager recordManager = new RecordManager();
        private Serializer serializer = new Serializer();

        public MapHome()
        {
            InitializeComponent();
            ShowLocation();
        }

        private async void ShowLocation()
        {
            GeoCoordinate myGeoCoordinate = null;

            if (recordManager.isExist("coordinate"))
            {
                string coordinateToString = recordManager.read("coordinate");
                MessageBox.Show(coordinateToString);

                string[] parts = coordinateToString.Split(',');
                double latitude = Double.Parse(parts[0], CultureInfo.InvariantCulture);
                double longitude = Double.Parse(parts[1], CultureInfo.InvariantCulture);

                myGeoCoordinate = new GeoCoordinate(latitude, longitude);
            }
            else
            {
                Geolocator myGeolocator = new Geolocator();
                Geoposition myGeoposition = await myGeolocator.GetGeopositionAsync();
                Geocoordinate coordinate = myGeoposition.Coordinate;
                myGeoCoordinate = CoordinateConverter.ConvertGeocoordinate(coordinate);
            }

            // Make my current location the center of the Map.
            HomeLocation.Center = myGeoCoordinate;
            HomeLocation.ZoomLevel = 5; // Set to 13

            SetMarker(myGeoCoordinate);
        }

        private void HomeLocation_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            GeoCoordinate location = HomeLocation.ConvertViewportPointToGeoCoordinate(e.GetPosition(HomeLocation));
            System.Diagnostics.Debug.WriteLine("latitude :" + location.Latitude + ", longitude : " + location.Longitude); // Delete later
            
            ReverseGeocodeQuery query = new ReverseGeocodeQuery()
            {
                GeoCoordinate = location
            };
            query.QueryCompleted += LocationToAddress;
            query.QueryAsync();
        }

        void LocationToAddress(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            List<String> addressElements = new List<string>();
            GeoCoordinate coordinate = null;
            foreach (var place in e.Result)
            {
                addressElements.Add(place.Information.Address.HouseNumber);
                addressElements.Add(place.Information.Address.Street);
                addressElements.Add(place.Information.Address.City);
                addressElements.Add(place.Information.Address.PostalCode);
                addressElements.Add(place.Information.Address.Country);
                addressElements.Add(place.Information.Address.Township);
                addressElements.Add(place.Information.Address.State);
                coordinate = place.GeoCoordinate;
                break;
            }

            if (e.Result.Count > 0)
            {
                string address = string.Join(".", addressElements.ToArray());
                recordManager.createOrUpdate("address", address);
                recordManager.createOrUpdate("coordinate", coordinate.ToString());
                
                MessageBox.Show(address);     // DELETE AFTER DEBUG
                MessageBox.Show(coordinate.ToString()); // DELETE AFTER DEBUG
            }
            else
            {
                MessageBox.Show(AppResources.errorLocation);
                
            }
        }

        private void SetMarker(GeoCoordinate myGeoCoordinate)
        {
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
    }
}