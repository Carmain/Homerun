using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using MobeeApp.Resources;
using System.IO.IsolatedStorage;
using Windows.Devices.Geolocation;

namespace MobeeApp
{
    public partial class Parameters : PhoneApplicationPage
    {
        private PhoneNumberChooserTask phoneNumberChooserTask;
        private RecordManager recordManager = new RecordManager();

        public Parameters()
        {
            InitializeComponent();
            phoneNumberChooserTask = new PhoneNumberChooserTask();
            phoneNumberChooserTask.Completed += new EventHandler<PhoneNumberResult>(GetContact);
        }

        private void BackMainPage(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ChooseContact(object sender, System.Windows.Input.GestureEventArgs e)
        {
            phoneNumberChooserTask.Show();
        }

        private void DeleteContact(object sender, System.Windows.Input.GestureEventArgs e)
        {
            recordManager.Delete("contactName");
            recordManager.Delete("contactPhone");
            Name.Text = AppResources.AddContactHint;
            Phone.Text = "";
            MessageBox.Show(AppResources.DeleteMessage);
        }

        void GetContact(object sender, PhoneNumberResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                bool successContact = recordManager.CreateOrUpdate("contactName", e.DisplayName);
                bool successPhone = recordManager.CreateOrUpdate("contactPhone", e.PhoneNumber);

                if (successContact && successPhone)
                {
                    Name.Text = e.DisplayName;
                    Phone.Text = e.PhoneNumber;
                }
                else
                {
                    MessageBox.Show(AppResources.ProblemSaveData);
                }
            }
        }

        private void ChoosePlace(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Geolocator geolocator = new Geolocator();
            if (geolocator.LocationStatus == PositionStatus.Disabled)
            {
                MessageBox.Show(AppResources.LocationDisabled);
            }
            else
            {
                NavigationService.Navigate(new Uri("/MapHome.xaml", UriKind.Relative));
            }
        }

        private void CollapseStackPanel(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (ContactManagement.Visibility == System.Windows.Visibility.Collapsed)
                ContactManagement.Visibility = System.Windows.Visibility.Visible;
            else
                ContactManagement.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}