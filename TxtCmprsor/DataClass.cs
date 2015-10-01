using System;
using System.Runtime.Serialization;

namespace TxtCmprsor
{
    [DataContract]
    public class DataClass
    {
        [DataMember]
        public bool use { get; set; }
        [DataMember]
        public string key { get; set; }
        [DataMember]
        public string value { get; set; }
    }
}
