using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace PBCrossPlatform
{
    [DataContract]
    public class DecimalDemo
    {
        [DataMember(Order = 1)]
        public string Title { get; set; }

        [DataMember(Order = 2)]
        public decimal DecimalValue { get; set; }

    }
}
