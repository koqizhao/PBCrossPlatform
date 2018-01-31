using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;

namespace MapContract
{
    [ProtoContract]
    public class Demo
    {
        [ProtoMember(1)]
        public string Url { get; set; }

        [ProtoMember(2)]
        public string Title { get; set; }

        [ProtoMember(3)]
        public List<string> Snipets { get; set; }

        [ProtoMember(4)]
        public Dictionary<int, Dictionary<string, string>> Metadata { get; set; }

    }
}
