using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Runtime.Serialization;

namespace TxtCmprsor
{
    [DataContract]
    public class ContactPhone
    {
        public bool IsSelected = false;
        public Visibility RecentList
        {
            get
            {
                if (IsSelected == false)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }
        public Visibility SendList
        {
            get
            {
                if (IsSelected == false)
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
        }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string PhoneNumber { get; set; }

        /*public ContactPhone(string Name, string PhoneNumber)
        {
            this.Name = Name;
            this.PhoneNumber = PhoneNumber;
        }
         * */
    }
}
