using System;
using System.Collections.Generic;
namespace TxtCmprsor
{
    public class WordsToWorkListMgmt
    {
        public static void AddToWorkList(string key, List<DataClass> l, Dictionary<string, string> d)
        {
            if (!key.Equals("") || key != null)
            {
                foreach(DataClass dataClass in l)
                {
                    if (dataClass.key.Equals(key) && !d.ContainsKey(key))
                    {
                        d.Add(key, dataClass.value);
                        return;
                    }
                }
            }
        }

        public static void RemoveFromWorkList(string key, List<DataClass> l, Dictionary<string, string> d)
        {
            if (!key.Equals("") || key != null)
            {
                if(d.ContainsKey(key)) 
                    d.Remove(key);
            }
        }
    }
}
