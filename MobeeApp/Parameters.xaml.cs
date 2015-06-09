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
using System.IO.IsolatedStorage;

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
            phoneNumberChooserTask.Completed += new EventHandler<PhoneNumberResult>(phoneNumberChooserTask_Completed);
        }

        private void Back_main_page(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Choose_contact(object sender, System.Windows.Input.GestureEventArgs e)
        {
            phoneNumberChooserTask.Show();
        }

        private void delete_contact(object sender, System.Windows.Input.GestureEventArgs e)
        {
            recordManager.delete("contactName");
            recordManager.delete("contactPhone");
        }

        void phoneNumberChooserTask_Completed(object sender, PhoneNumberResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                MessageBox.Show("The phone number for " + e.DisplayName + " is " + e.PhoneNumber);

                Name.Text = e.DisplayName;
                Phone.Text = e.PhoneNumber;

                recordManager.createOrUpdate("contactName", e.DisplayName);
                recordManager.createOrUpdate("contactPhone", e.PhoneNumber);
            }
        }

        private void Choose_place(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Map_home.xaml", UriKind.Relative));
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