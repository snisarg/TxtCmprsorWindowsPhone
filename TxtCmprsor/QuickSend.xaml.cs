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
using Microsoft.Phone.Tasks;
using System.IO.IsolatedStorage;

namespace TxtCmprsor
{
    public partial class QuickSend : PhoneApplicationPage
    {
        const int MAXSIZE = 7;
        Microsoft.Phone.Tasks.PhoneNumberChooserTask PhoneNumberTask = new Microsoft.Phone.Tasks.PhoneNumberChooserTask();
        Queue<ContactPhone> RecentContacts = new Queue<ContactPhone>(MAXSIZE);
        IsolatedStorageSettings iss = IsolatedStorageSettings.ApplicationSettings;

        public QuickSend()
        {
            InitializeComponent();
            PhoneNumberTask.Completed += new EventHandler<Microsoft.Phone.Tasks.PhoneNumberResult>(pnct_Completed);
            if (iss.Contains("Recent"))
                //RecentContacts = (Queue<ContactPhone>)iss["Recent"];
                RecentContacts = new Queue<ContactPhone>((List<ContactPhone>)iss["Recent"]);
            else
                iss.Add("Recent", RecentContacts.ToList<ContactPhone>());
        }

        void pnct_Completed(object sender, Microsoft.Phone.Tasks.PhoneNumberResult e)
        {

            if (e.TaskResult == TaskResult.OK)
            {
                if (RecentContacts.Count == MAXSIZE)
                    RecentContacts.Dequeue();
                RecentContacts.Enqueue(new ContactPhone() { Name = e.DisplayName, PhoneNumber = e.PhoneNumber, IsSelected = true });
                UpdateList();
                //SendSelected(e.DisplayName, e.PhoneNumber);
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string Msg;
            NavigationContext.QueryString.TryGetValue("msg", out Msg);
            MessageText.Text = Msg;
            UpdateList();
            base.OnNavigatedTo(e);
        }

        private void SearchContactButton_Click(object sender, RoutedEventArgs e)
        {
            PhoneNumberTask.Show();
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            string SendingInternalRepresentation = "";
            SmsComposeTask composeSMS = new SmsComposeTask();
            composeSMS.Body = MessageText.Text;
            foreach (ContactPhone cp in RecentContacts)
            {
                if (cp.IsSelected)
                {
                    SendingInternalRepresentation += cp.Name + "<" + cp.PhoneNumber + ">; ";
                }
            }
            composeSMS.To = SendingInternalRepresentation;
            composeSMS.Show();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            iss["Recent"] = RecentContacts.ToList<ContactPhone>();
            base.OnNavigatedFrom(e);
        }

        void UpdateList()
        {
            //MessageBox.Show("Update");
            RecentContactListBox.ItemsSource = null;
            RecentContactListBox.ItemsSource = RecentContacts;
            SendContactListBox.ItemsSource = null;
            SendContactListBox.ItemsSource = RecentContacts;
        }

        private void Border_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Border b = sender as Border;
            ContactPhone cp = RecentContacts.First<ContactPhone>(x => x.Name.Equals(b.Tag.ToString()));
            cp.IsSelected = true;
            UpdateList();
            //SendSelected(cp.Name, cp.PhoneNumber);
        }
        /*
        void SendSelected(string name, string number)   // Initialize the sending task
        {
            SendingDisplayRepresentation += name + "; ";
            SendingInternalRepresentation += name + "<" + number + ">; ";
            ToTextBlock.Text = "To: " + SendingDisplayRepresentation;
        }
        */
        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            RecentContacts.Clear();
            UpdateList();
        }

        private void Border_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Border b = sender as Border;
            ContactPhone cp = RecentContacts.First<ContactPhone>(x => x.Name.Equals(b.Tag.ToString()));
            cp.IsSelected = false;
            UpdateList();
        }
    }
}