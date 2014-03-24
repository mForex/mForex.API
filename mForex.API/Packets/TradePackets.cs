using System;
using ProtoBuf;

namespace mForex.API.Packets
{
    [ProtoContract]
    public class TradeRecord
    {
        [ProtoMember(1, IsRequired = true)]
        public int Login { get; set; }

        [ProtoMember(2, IsRequired = true)]
        public int Order { get; set; }

        [ProtoMember(3, IsRequired = true)]
        public TradeCommand TradeCommand { get; set; }

        [ProtoMember(4, IsRequired = true)]
        public string Symbol { get; set; }

        [ProtoMember(5, IsRequired = true)]
        public double Volume { get; set; }

        [ProtoMember(6, IsRequired = true)]
        public double Profit { get; set; }

        [ProtoMember(7)]
        public double Swaps { get; set; }

        [ProtoMember(8)]
        public double Commission { get; set; }

        [ProtoMember(9)]
        public double StopLoss { get; set; }

        [ProtoMember(10)]
        public double TakeProfit { get; set; }

        [ProtoMember(11, IsRequired = true)]
        public double OpenPrice { get; set; }

        [ProtoMember(12, IsRequired = true)]
        public DateTime OpenTime { get; set; }

        [ProtoMember(13)]
        public double ClosePrice { get; set; }

        [ProtoMember(14)]
        public DateTime CloseTime { get; set; }

        [ProtoMember(15, IsRequired = true)]
        public bool Closed { get; set; }

        [ProtoMember(16, IsRequired = true)]
        public int Digits { get; set; }

        [ProtoMember(17)]
        public string Comment { get; set; }

        [ProtoMember(18)]
        public DateTime Expiration { get; set; }

        public void Copy(TradeRecord tradeRecord)
        {
            if (tradeRecord == null)
                throw new ArgumentNullException("tradeRecord");

            this.Closed = tradeRecord.Closed;
            this.ClosePrice = tradeRecord.ClosePrice;
            this.CloseTime = tradeRecord.CloseTime;
            this.Comment = tradeRecord.Comment;
            this.Commission = tradeRecord.Commission;
            this.Digits = tradeRecord.Digits;
            this.Expiration = tradeRecord.Expiration;
            this.Login = tradeRecord.Login;
            this.OpenPrice = tradeRecord.OpenPrice;
            this.OpenTime = tradeRecord.OpenTime;
            this.Order = tradeRecord.Order;
            this.Profit = tradeRecord.Profit;
            this.StopLoss = tradeRecord.StopLoss;
            this.Symbol = tradeRecord.Symbol;
            this.TakeProfit = tradeRecord.TakeProfit;
            this.Swaps = tradeRecord.Swaps;
            this.TradeCommand = tradeRecord.TradeCommand;
            this.Volume = tradeRecord.Volume;
        }
    }

    [ProtoContract]
    public class ClosedTradesRequestPacket : APINetworkPacket
    {
        public ClosedTradesRequestPacket()
            : base(APINetworkPacketType.ClosedTradesRequest)
        { }

        public ClosedTradesRequestPacket(int reqId, DateTime dateFrom, DateTime dateTo)
            : base(APINetworkPacketType.ClosedTradesRequest)
        {
            this.RequestId = reqId;
            this.DateFrom = dateFrom;
            this.DateTo = dateTo;
        }

        [ProtoMember(1, IsRequired = true)]
        public int RequestId { get; set; }

        [ProtoMember(2, IsRequired = true)]
        public DateTime DateFrom { get; set; }

        [ProtoMember(3, IsRequired = true)]
        public DateTime DateTo { get; set; }
    }

    [ProtoContract]
    public class ClosedTradesResponsePacket : APINetworkPacket, IIdentifiable
    {
        public ClosedTradesResponsePacket()
            : base(APINetworkPacketType.ClosedTradesResponse)
        {
        }

        [ProtoMember(1, IsRequired = true)]
        public int RequestId { get; set; }

        [ProtoMember(2, IsRequired = true)]
        public bool Status { get; set; }

        [ProtoMember(3, IsRequired = true)]
        public APIErrorCode ErrorCode { get; set; }

        [ProtoMember(4, IsRequired = true)]
        public TradeRecord[] Trades { get; set; }

        [ProtoMember(5, IsRequired = true)]
        public DateTime DateFrom { get; set; }

        [ProtoMember(6, IsRequired = true)]
        public DateTime DateTo { get; set; }
    }        
    
    [ProtoContract]
    public class TradeUpdatePacket : APINetworkPacket
    {
        [ProtoMember(1)]
        public TradeRecord Trade { get; private set; }

        [ProtoMember(2)]
        public TradeAction Action { get; private set; }

        public TradeUpdatePacket()
            : base(APINetworkPacketType.TradeUpdate)
        {     
            
        }

        public TradeUpdatePacket(TradeRecord trade, TradeAction action)
            : base(APINetworkPacketType.TradeUpdate)
        {        
            this.Trade = trade;
            this.Action = action;
        }
    }

    [ProtoContract]
    public class TradesInfoRequestPacket : APINetworkPacket
    {
        public TradesInfoRequestPacket()
            : base(APINetworkPacketType.TradesInfoRequest)
        { }

        public TradesInfoRequestPacket(int requestId) 
            : base(APINetworkPacketType.TradesInfoRequest) 
        {
            RequestId = requestId;
        }

        [ProtoMember(1, IsRequired = true)]
        public int RequestId { get; set; }
    }

    [ProtoContract]
    public class TradesInfoResponsePacket : APINetworkPacket, IIdentifiable
    {
        public TradesInfoResponsePacket()
            : base(APINetworkPacketType.TradesInfoResponse)
        {
        }
        
        [ProtoMember(1, IsRequired = true)]
        public int RequestId { get; private set; }

        [ProtoMember(2, IsRequired = true)]
        public bool Status { get; private set; }

        [ProtoMember(3, IsRequired = true)]
        public APIErrorCode ErrorCode { get; private set; }

        [ProtoMember(4, IsRequired = true)]
        public TradeRecord[] Trades { get; private set; }
    }
    
}
