using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MobeeApp
{
    public partial class Parameters : PhoneApplicationPage
    {
        public Parameters()
        {
            InitializeComponent();
        }

        private void Back_main_page(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}