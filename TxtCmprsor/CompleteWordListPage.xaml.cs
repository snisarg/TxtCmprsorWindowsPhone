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
using System.Xml.Linq;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml;

namespace TxtCmprsor
{
    public partial class CompleteWordListPage : PhoneApplicationPage
    {
        App a;
        //IsolatedStorageFile isf;

        public CompleteWordListPage()
        {
            InitializeComponent();
            //isf = IsolatedStorageFile.GetUserStoreForApplication();
            a = App.Current as App;
        }

        private void refreshList()
        {
            a.Words.Sort((DataClass x, DataClass y) =>
            {
                return x.key.CompareTo(y.key);
            }); // Sort the words of the list in ascending order
            lstTasks.ItemsSource = null;
            lstTasks.ItemsSource = a.Words;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            refreshList();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked.Value)
            {
                System.Diagnostics.Debug.WriteLine("Checkbox checked");
                CheckBox i = sender as CheckBox;
                if (sender == null || e == null || i.Tag == null)
                    return;
                System.Diagnostics.Debug.WriteLine("Inside Checkbox checked : >" + i.Tag.ToString() + "<");
                WordsToWorkListMgmt.AddToWorkList(i.Tag.ToString(), a.Words, a.WorkList);
                a.Words.First(findkey => findkey.key.Equals(i.Tag.ToString())).use = true;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("checkbox unchecked");
                CheckBox i = sender as CheckBox;
                if (sender == null || e == null || i.Tag == null)
                    return;
                System.Diagnostics.Debug.WriteLine("Inside Checkbox unchecked : >" + i.Tag.ToString() + "<");
                WordsToWorkListMgmt.RemoveFromWorkList(i.Tag.ToString(), a.Words, a.WorkList);
                a.Words.First(findkey => findkey.key.Equals(i.Tag.ToString())).use = false;
            }
        }

        private void addNewConversionWordButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SingleWordAdd.xaml", UriKind.Relative));
        }

        private void MenuItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SingleWordEdit.xaml?word=" + (sender as MenuItem).Tag.ToString(), UriKind.Relative));
        }
    }
}
