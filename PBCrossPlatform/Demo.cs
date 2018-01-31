using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace PBCrossPlatform
{
    [DataContract]
    public class Demo
    {
        [DataMember(Order = 1)]
        public string Url { get; set; }

        [DataMember(Order = 2)]
        public string Title { get; set; }

        [DataMember(Order = 3)]
        public List<string> Snipets { get; set; }

        [DataMember(Order = 4)]
        public Dictionary<int, Dictionary<string, string>> Metadata { get; set; }

        [DataMember(Order = 5)]
        public List<int> IntValues { get; set; }
    }
}
