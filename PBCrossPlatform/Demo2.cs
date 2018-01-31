using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;

namespace PBCrossPlatform
{
    //[ProtoContract]
    public class Demo2
    {
        [ProtoMember(1)]
        public EnumDemo OK { get; set; }

        [ProtoMember(2)]
        public string Yes { get; set; }
    }
}
