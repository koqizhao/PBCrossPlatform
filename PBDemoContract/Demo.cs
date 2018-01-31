using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;

namespace PBDemoContract
{
    [ProtoContract]
    public class Demo
    {
        [ProtoMember(1)]
        public string StringField { get; set; }

        [ProtoMember(2)]
        public int IntField { get; set; }
    }
}
