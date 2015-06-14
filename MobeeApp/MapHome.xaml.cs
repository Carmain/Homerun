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
                string coordinateToString = recordManager.Read("coordinate");
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
                recordManager.CreateOrUpdate("coordinate", myGeoCoordinate.ToString());
            }

            // Set the center of the map with the geoCoordinate
            HomeLocation.Center = myGeoCoordinate;
            HomeLocation.ZoomLevel = 13;

            SetMarker(myGeoCoordinate);
        }

        private void HomeLocation_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            GeoCoordinate location = HomeLocation.ConvertViewportPointToGeoCoordinate(e.GetPosition(HomeLocation));
            recordManager.CreateOrUpdate("coordinate", location.ToString());
            SetMarker(location);
        }

        private void SetMarker(GeoCoordinate location)
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
            locationOverlay.GeoCoordinate = location;

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