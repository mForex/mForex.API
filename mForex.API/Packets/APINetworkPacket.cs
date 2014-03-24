
namespace mForex.API.Packets
{
    public abstract class APINetworkPacket
    {
        public APINetworkPacketType Type { get; private set; }

        public APINetworkPacket(APINetworkPacketType type)
        {
            Type = type;
        }
    }
}
