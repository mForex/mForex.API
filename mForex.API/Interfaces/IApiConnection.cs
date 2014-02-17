using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
