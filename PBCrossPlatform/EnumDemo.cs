using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;

namespace PBCrossPlatform
{
    public enum EnumDemo
    {
        //[ProtoEnum(Value=2)]
        OK=4,

        //[ProtoEnum(Value=3)]
        NotOK=6
    }
}
