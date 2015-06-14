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
            GeoCoordinate myGeoCoordinate = new GeoCoordinate();

            if (recordManager.isExist("coordinate"))
            {
                string coordinateToString = recordManager.read("coordinate");
                string[] parts = coordinateToString.Split(',');
                double latitude = Double.Parse(parts[0], CultureInfo.InvariantCulture);
                double longitude = Double.Parse(parts[1], CultureInfo.InvariantCulture);
                myGeoCoordinate = new GeoCoordinate(latitude, longitude);
            }
            else
            {
                Geolocator geolocator = new Geolocator();
                Geoposition myGeoposition = await geolocator.GetGeopositionAsync();
                Geocoordinate coordinate = myGeoposition.Coordinate;
                myGeoCoordinate = CoordinateConverter.ConvertGeocoordinate(coordinate);
            }

            // Set the center of the map with the geoCoordinate
            HomeLocation.Center = myGeoCoordinate;
            HomeLocation.ZoomLevel = 13;

            SetMarker(myGeoCoordinate);
        }

        private void HomeLocation_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            GeoCoordinate location = HomeLocation.ConvertViewportPointToGeoCoordinate(e.GetPosition(HomeLocation));
            ReverseGeocodeQuery query = new ReverseGeocodeQuery()
            {
                GeoCoordinate = location
            };
            query.QueryCompleted += LocationToAddress;
            query.QueryAsync();

            SetMarker(location);
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
            }
            else
            {
                MessageBox.Show(AppResources.ErrorLocation);
                
            }
        }

        private void SetMarker(GeoCoordinate myGeoCoordinate)
        {
            
            Ellipse marker = new Ellipse(); // Mark for the location
            MapOverlay locationOverlay = new MapOverlay(); // MapOverlay to contain the circle.
            MapLayer layer = new MapLayer(); // MapLayer to contain the MapOverlay.

            Color phoneColor = (Color)Application.Current.Resources["PhoneAccentColor"];
            marker.Fill = new SolidColorBrush(phoneColor);
            marker.Height = 20;
            marker.Width = 20;

            locationOverlay.Content = marker;
            locationOverlay.PositionOrigin = new Point(0.5, 0.5);
            locationOverlay.GeoCoordinate = myGeoCoordinate;

            layer.Add(locationOverlay);
            HomeLocation.Layers.Clear();
            HomeLocation.Layers.Add(layer);
        }

        private void BackParameters(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}