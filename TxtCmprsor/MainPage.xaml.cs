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
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml;
using System.Xml.Linq;

namespace TxtCmprsor
{
    public partial class MainPage : PhoneApplicationPage
    {
        App a;
        int oi = 0;
        bool TextBoxDoubleChange = false;  //used as TextChanged event gets fired twice. true false used to single it out
        Dictionary<string, string> WorkList;
        string replace = "", lastWord;
		System.IO.IsolatedStorage.IsolatedStorageSettings appSettings = System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings;
		bool Apostrophe = true;
		
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            a = App.Current as App;
            WorkList = a.WorkList;  // dictionary shifted to App, #v0.1. Taking the object from there.
            //isf = IsolatedStorageFile.GetUserStoreForApplication();
            // No longer needed from v0.2
            //setupWorkList();
        } 
        
        private void sms_Click(object sender, EventArgs e)
        {
            if ((bool)appSettings["msgHelper"] == false)
            {
                SmsComposeTask composeSMS = new SmsComposeTask();
                composeSMS.Body = ActualMsg.Text;
                composeSMS.Show();
            }
            else
            {
                NavigationService.Navigate(new Uri("/QuickSend.xaml?msg=" + Uri.EscapeDataString(ActualMsg.Text), UriKind.Relative));
            }
        }

        private void email_Click(object sender, EventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();
            emailComposeTask.Body = ActualMsg.Text;
            emailComposeTask.Show();
        }

        private void social_Click(object sender, EventArgs e)
        {
            ShareStatusTask shareStatusTask = new ShareStatusTask();
            shareStatusTask.Status = ActualMsg.Text;
            shareStatusTask.Show();
        }

        private void Settings_Click(object sender, System.EventArgs e)
        {
			NavigationService.Navigate(new Uri("/CompleteWordListPage.xaml", UriKind.Relative));
        }
		 private void About_Click(object sender, System.EventArgs e)
        {
			NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Apostrophe = (bool)appSettings["apostrophe"];
            ShowDrafts();
        }

        private void copyTest_Click(object sender, System.EventArgs e)
        {
        	System.Windows.Clipboard.SetText(ActualMsg.Text);
        }

        private void ApplicationBarMenuItem_Click(object sender, System.EventArgs e)
        {
        	NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }

        private void ActualMsg_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBoxDoubleChange = !TextBoxDoubleChange;
            if (TextBoxDoubleChange)    // TextChanged fired twice, this filters one of the calls, improved efficiency
            {
                return;
            }

            // Show and hide the Save to Draft button
            if (ActualMsg.Text.Length == 0)
                SaveToDraftButton.IsEnabled = false;
            else
                SaveToDraftButton.IsEnabled = true;

            int LastCursorPosition = ActualMsg.SelectionStart;   // Save the position of the cursor
            if (Apostrophe)
            {
                int OriginalTextLength = ActualMsg.Text.Length;
                string WithoutAppostrophe = ActualMsg.Text.Replace(@"'", "");
                ActualMsg.Text = WithoutAppostrophe;
                LastCursorPosition = LastCursorPosition - OriginalTextLength + WithoutAppostrophe.Length;
                ActualMsg.Select(LastCursorPosition, 0);   //Reposition cursor at the end string at the textbox
            }

            String StringBeforeCursor = ActualMsg.Text.Substring(0, LastCursorPosition);

            //Checking for 'Space' char at the end, and starting the last word extraction procedure
            try
            {
                if (StringBeforeCursor[StringBeforeCursor.Length - 1] == ' ' || StringBeforeCursor[StringBeforeCursor.Length - 1] == '.' || StringBeforeCursor[StringBeforeCursor.Length - 1] == '?' || StringBeforeCursor[StringBeforeCursor.Length - 1] == '!' || StringBeforeCursor[StringBeforeCursor.Length - 1] == ',' || StringBeforeCursor[StringBeforeCursor.Length - 1] == '\r') // Decides when to invoke the function. In WP, only /r is used for NEWLINE
                {
                    string AfterCursor = ActualMsg.Text.Substring(LastCursorPosition);
                    char ConditionalEndChar = StringBeforeCursor[StringBeforeCursor.Length - 1];
                    for (oi = StringBeforeCursor.Length - 2; oi >= 0 && (StringBeforeCursor[oi] != ' ' && StringBeforeCursor[oi] != '\r'); oi--) ; //start from end looking for second last ' ' char
                    oi++;
                    if (oi >= 0 && oi != (StringBeforeCursor.Length - 2))
                    {
                        lastWord = StringBeforeCursor.Substring(oi, StringBeforeCursor.Length - oi - 1).ToLower(); //Converting all keys to lower. So all comparisons are made in Lower.
                        if (WorkList.TryGetValue(lastWord, out replace))
                        {
                            ActualMsg.Text = StringBeforeCursor.Substring(0, oi) + replace + ConditionalEndChar + AfterCursor;
                            ActualMsg.Select(LastCursorPosition - (lastWord.Length) + (replace.Length), 0);
                        }
                    }
                    /* OLD ALGO
                    //MessageBox.Show("inside space comparison");
                    lastWord = s.Substring(oi, (s.Length - oi - 1)).ToLower();
                    textBlock1.Text = " " + TextBoxDoubleChange;
                    if (workList.TryGetValue(lastWord, out replace))
                    {
                        ActualMsg.Text = s.Substring(0, oi) + replace + " ";
                        ActualMsg.Select(s.Length, 0);
                    }
                    oi = s.Length;
                    */
                }
                // To show how many characters have been typed. Similar to the default msg for SMS
                if (ActualMsg.Text.Length < 100)
                    msgLength.Text = "";
                else if (ActualMsg.Text.Length < 141)
                    msgLength.Text = "Twitter Limit: " + ActualMsg.Text.Length + "/140";
                else if (ActualMsg.Text.Length < 161)
                    msgLength.Text = ActualMsg.Text.Length + "/160";
                else if (ActualMsg.Text.Length < 305)
                    msgLength.Text = "2 messages, " + ActualMsg.Text.Length + "/304";
                else if (ActualMsg.Text.Length < 457)
                    msgLength.Text = "3 messages, " + ActualMsg.Text.Length + "/456";
                else
                    msgLength.Text = "Just long!!!";
            }
            catch (Exception) { };  // for s.Length-1 in if() when nothing in textbox
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            a.DraftMsg.Add(ActualMsg.Text);
            ShowDrafts();
        }

        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TextBlock send = sender as TextBlock;
            ActualMsg.Text += send.Tag.ToString();
        }

        private void MenuItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            a.DraftMsg.Remove((sender as MenuItem).Tag.ToString());
            ShowDrafts();
        }

        private void ShowDrafts()
        {
            DraftsListBox.ItemsSource = null;
            if (a.DraftMsg.Count > 0)
            {
                DraftHeader.Visibility = Visibility.Visible;
                DraftsListBox.ItemsSource = a.DraftMsg;
            }
            else
                DraftHeader.Visibility = Visibility.Collapsed;
        }
    }
}