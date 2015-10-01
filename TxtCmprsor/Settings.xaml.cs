using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace TxtCmprsor
{
    public partial class Settings : PhoneApplicationPage
    {
		System.IO.IsolatedStorage.IsolatedStorageSettings appSettings = System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings;
        public Settings()
        {
            InitializeComponent();
			apostropheSwitch.IsChecked = (bool)appSettings["apostrophe"];
            MsgHelper.IsChecked = (bool)appSettings["msgHelper"];
        }

        private void apostropheSwitch_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
        	appSettings["apostrophe"] = true;
        }

        private void apostropheSwitch_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
        	appSettings["apostrophe"] = false;
        }

        private void MsgHelper_Checked(object sender, RoutedEventArgs e)
        {
            appSettings["msgHelper"] = true;
        }

        private void MsgHelper_Unchecked(object sender, RoutedEventArgs e)
        {
            appSettings["msgHelper"] = false;
        }
    }
}
