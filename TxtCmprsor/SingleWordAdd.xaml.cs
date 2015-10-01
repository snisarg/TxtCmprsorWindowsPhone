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
    public partial class SingleWordAdd : PhoneApplicationPage
    {
        App a;
        public SingleWordAdd()
        {
            InitializeComponent();
            a = App.Current as App;
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            DataClass NewlyCreated = new DataClass();
            NewlyCreated.key = RealWord.Text.Trim().ToLower();
            NewlyCreated.value = ConvertWord.Text;
            NewlyCreated.use = toUse.IsChecked.Value;

            //Check to see if that word already exists
            foreach (DataClass ToCheck in a.Words)
            {
                if(ToCheck.key.Equals(NewlyCreated.key))
                {
                    MessageBox.Show("The word " + NewlyCreated.key + " already exists. Please edit that word from the 'Word List' page instead.");
                    return;
                }
            }

            a.Words.Add(NewlyCreated);
            if (NewlyCreated.use)    // add new entry if selection is true
                WordsToWorkListMgmt.AddToWorkList(NewlyCreated.key, a.Words, a.WorkList);
            NavigationService.GoBack();
        }

        // For validation purposes
        private void RealWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)  // Prevents SPACE from being entered
                e.Handled = true;
        }

        private void ConvertWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)  // Prevents SPACE from being entered
                e.Handled = true;
        }
    }
}