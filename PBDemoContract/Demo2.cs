using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;

namespace PBDemoContract
{
    [ProtoContract]
    public class Demo2
    {
        [ProtoMember(1)]
        public string StringField2 { get; set; }

        [ProtoMember(2)]
        public int IntField2 { get; set; }
    }
}
