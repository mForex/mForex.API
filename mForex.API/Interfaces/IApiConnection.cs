using System;
using System.Threading.Tasks;
using mForex.API.Packets;

namespace mForex.API
{
    public interface IApiConnection
    {
        event Action<Exception> Disconnected;
        event Action<APINetworkPacket> PacketReceived;

        Task Connect();
        void Disconnect();
        
        void SendPacket(APINetworkPacket packet);        
    }
}
