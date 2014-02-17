using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mForex.API
{
    [ProtoContract]
    public enum TradeAction
    {
        [ProtoEnum]
        Opened = 0,
        [ProtoEnum]
        Modified = 1,
        [ProtoEnum]
        Closed = 2
    }
}
