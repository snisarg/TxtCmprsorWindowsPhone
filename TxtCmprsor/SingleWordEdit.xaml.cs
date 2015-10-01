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
    public partial class SingleWordEditOrAdd : PhoneApplicationPage
    {
        App a;
        DataClass currentSelected;
        public SingleWordEditOrAdd()
        {
            InitializeComponent();
            a = App.Current as App;
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string WordSelected="defalut";
            if (NavigationContext.QueryString.ContainsKey("word"))
            {
                NavigationContext.QueryString.TryGetValue("word", out WordSelected);
                LoadPageElements(WordSelected);
            }
            else
            {
                // wrong page call, error!
                NavigationService.GoBack();
            }
            base.OnNavigatedTo(e);
        }
        //Bind the currently passed object to the page.
        void LoadPageElements(string wordSelected) 
        {
            currentSelected = a.Words.First(E => E.key.Equals(wordSelected));
            this.DataContext = currentSelected;
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            WordsToWorkListMgmt.RemoveFromWorkList(currentSelected.key, a.Words, a.WorkList); // remove old entry
            currentSelected.key = RealWord.Text.Trim().ToLower();
            currentSelected.value = ConvertWord.Text;
            currentSelected.use = toUse.IsChecked.Value;
            if (currentSelected.use)    // add new entry if selection is true
                WordsToWorkListMgmt.AddToWorkList(currentSelected.key, a.Words, a.WorkList);
            NavigationService.GoBack();
        }

        // For validation purposes
        private void RealWord_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space)  // Prevents SPACE from being entered
				e.Handled = true;
        }

        private void ConvertWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)  // Prevents SPACE from being entered
                e.Handled = true;
        }
    }
}