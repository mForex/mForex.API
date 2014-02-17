using mForex.API.Data;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mForex.API.Packets
{
    [ProtoContract]
    public class TradeTransRequestPacket : APINetworkPacket
    {
        [ProtoMember(1, IsRequired = true)]
        public int RequestId { get; private set; }

        [ProtoMember(2, IsRequired = true)]
        public TradeCommand TradeCommand { get; private set; }

        [ProtoMember(3, IsRequired = true)]
        public TransactionType TransactionType { get; private set; }

        [ProtoMember(4, IsRequired = true)]
        public double Price { get; private set; }

        [ProtoMember(5, IsRequired = true)]
        public double StopLoss { get; private set; }

        [ProtoMember(6, IsRequired = true)]
        public double TakeProfit { get; private set; }

        [ProtoMember(7, IsRequired = true)]
        public string Symbol { get; private set; }

        [ProtoMember(8, IsRequired = true)]
        public double Volume { get; private set; }

        [ProtoMember(9, IsRequired = true)]
        public int Order { get; private set; }

        [ProtoMember(10, IsRequired = true)]
        public string Comment { get; private set; }

        [ProtoMember(11, IsRequired = true)]
        public DateTime Expiration { get; private set; }

        public TradeTransRequestPacket()
            : base(APINetworkPacketType.TradeTransRequest)
        { }

        public TradeTransRequestPacket(int requestId,
                        TradeCommand tradeCommand,
                        TransactionType transactionType, double price,
                        double stopLoss, double takeProfit,
                        string symbol, double volume, int order,
                        string comment, DateTime expiration)
            : base(APINetworkPacketType.TradeTransRequest)
        {
            RequestId = requestId;
            TradeCommand = tradeCommand;
            TransactionType = transactionType;
            Price = price;
            StopLoss = stopLoss;
            TakeProfit = takeProfit;
            Symbol = symbol;
            Volume = volume;
            Order = order;
            Comment = comment;
            Expiration = expiration;
        }

       public override string ToString()
        {
            return string.Format("req: {0}, cmd: {1}, type: {2}, price: {3}, " +
                                 "sl: {4}, tp: {5}, symbol: {6}, vol: {7}, order: {8}, cmt: {9}, exp: {10}",
                                 RequestId, TradeCommand, TradeCommand, Price,
                                 StopLoss, TakeProfit, Symbol, Volume, Order, Comment, Expiration);
        }
    }

    [ProtoContract]
    public class TradeTransResponsePacket : APINetworkPacket, IIdentifiable
    {

        [ProtoMember(1, IsRequired = true)]
        public int RequestId { get; private set; }

        [ProtoMember(2, IsRequired = true)]
        public bool Status { get; private set; }

        [ProtoMember(3, IsRequired = true)]
        public APIErrorCode ErrorCode { get; private set; }

        [ProtoMember(4, IsRequired = true)]
        public int Order { get; private set; }

        [ProtoMember(5, IsRequired = true)]
        public int RetCode { get; private set; }

        public TradeTransResponsePacket()
            : base(APINetworkPacketType.TradeTransResponse)
        {
        }
    }
}
