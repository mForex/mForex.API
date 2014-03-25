using System;
using ProtoBuf;

namespace mForex.API.Packets
{

    [ProtoContract]   
    public class TickRegistrationRequestPacket : APINetworkPacket
    {    
        [ProtoMember(1, IsRequired = true)]
        public int RequestId { get; private set; }

        [ProtoMember(2, IsRequired = true)]
        public string Symbol { get; private set; }

        [ProtoMember(3, IsRequired = true)]
        public RegistrationAction RegistrationAction { get; private set; }

        public TickRegistrationRequestPacket()
            : base(APINetworkPacketType.TickRegistrationRequest)
        {
        }
    }

    [ProtoContract]
    public class TickRegistrationResponsePacket : APINetworkPacket
    {
        [ProtoMember(1, IsRequired = true)]
        public int RequestId { get; private set; }

        [ProtoMember(2, IsRequired = true)]
        public bool Status { get; private set; }

        [ProtoMember(3, IsRequired = true)]
        public APIErrorCode ErrorCode { get; private set; }

        public TickRegistrationResponsePacket()
            : base(APINetworkPacketType.TickRegistrationResponse)
        {
        }

        public TickRegistrationResponsePacket(int requestId, APIErrorCode errorCode)
            : base(APINetworkPacketType.TickRegistrationResponse)
        {

            this.RequestId = requestId;
            this.Status = errorCode == APIErrorCode.OK;
            this.ErrorCode = errorCode;
        }
    }
    
    [ProtoContract]
    public class TickPacket : APINetworkPacket
    {
        [ProtoMember(1, IsRequired = true)]
        public Tick[] Ticks { get; set; }

        public TickPacket()
            : this(null)
        { }

        public TickPacket(Tick[] ticks)
            : base(APINetworkPacketType.Ticks)
        {
            this.Ticks = ticks;
        }
    }


    [ProtoContract]
    public class Tick
    {
        [ProtoMember(1)]
        public string Symbol { get; set; }

        [ProtoMember(2)]
        public double Bid { get; set; }

        [ProtoMember(3)]
        public double Ask { get; set; }

        [ProtoMember(4)]
        public DateTime Time { get; set; }

        [ProtoMember(5)]
        public ConversionRate ConversionRate { get; set; }

        public Tick() { }
        public Tick(string symbol, double bid, double ask, DateTime time)
        {
            Symbol = symbol;
            Bid = bid;
            Ask = ask;
            Time = time;
        }
    }

    [ProtoContract]
    public class ConversionRate
    {
        [ProtoMember(1)]
        public string Symbol { get; set; }

        [ProtoMember(2)]
        public string DepositCurrency { get; set; }

        [ProtoMember(3)]
        public double Bid { get; set; }

        [ProtoMember(4)]
        public double Ask { get; set; }

        public ConversionRate() { }
        public ConversionRate(string symbol, string depositCurrency, double bid, double ask)
        {
            Symbol = symbol;
            DepositCurrency = depositCurrency;
            Bid = bid;
            Ask = ask;
        }
    }
}
