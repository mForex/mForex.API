using System;
using System.Threading.Tasks;
using mForex.API.Packets;

namespace mForex.API
{
    public interface ITradeProvider
    {
        /// <summary>
        /// Sends trade request to the server.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="tradeCommand"></param>
        /// <param name="price"></param>
        /// <param name="stopLoss"></param>
        /// <param name="takeProfit"></param>
        /// <param name="volume"></param>
        /// <returns>A task that represents order opening process. The value of the result contains returnet packet.</returns>
        Task<TradeTransResponsePacket> OpenOrder(string symbol, TradeCommand tradeCommand, double price, double stopLoss, double takeProfit, double volume);

        /// <summary>
        /// Sends trade request to the server.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="tradeCommand"></param>
        /// <param name="price"></param>
        /// <param name="stopLoss"></param>
        /// <param name="takeProfit"></param>
        /// <param name="volume"></param>
        /// <param name="comment"></param>
        /// <returns>A task that represents order opening process. The value of the result contains returnet packet.</returns>
        Task<TradeTransResponsePacket> OpenOrder(string symbol, TradeCommand tradeCommand, double price, double stopLoss, double takeProfit, double volume,
                                                 string comment);

        /// <summary>
        /// Sends order modification request to the server.
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="newPrice"></param>
        /// <param name="newStopLoss"></param>
        /// <param name="newTakeProfit"></param>
        /// <param name="newVolume"></param>
        /// <param name="newExpiration"></param>
        /// <returns>A task that represents order modification process. The value of the result contains returnet packet.</returns>
        Task<TradeTransResponsePacket> ModifyOrder(int orderId, double newPrice, double newStopLoss, double newTakeProfit, double newVolume, DateTime newExpiration);

        /// <summary>
        /// Sends delete pending order request to the server.
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>A task that represents transaction deleting process. The value of the result contains returnet packet.</returns>
        Task<TradeTransResponsePacket> DeleteOrder(int orderId);

        /// <summary>
        /// Sends close market order request to the server.
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="volume"></param>
        /// <returns>A task that represents transaction closing process. The value of the result contains returnet packet.</returns>
        /// <remarks> If volume to be closed is smaller than requested orders' volume, 
        /// then order is partialy closed. Resultant order will be given new order id.
        /// </remarks>
        Task<TradeTransResponsePacket> CloseOrder(int orderId, double volume);
    }
}
