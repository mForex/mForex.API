using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace mForex.API.Packets
{
    class PacketSerializer
    {
        Func<MemoryStream, APINetworkPacket>[] map; 

        public PacketSerializer()
        {
            map = new Func<MemoryStream, APINetworkPacket>[1024];

            foreach (var type in this.GetType().Assembly
                                     .GetTypes()
                                     .Where(t => t.IsSubclassOf(typeof(APINetworkPacket)))
                                     .Where(t => !t.IsAbstract)
                                     .Where(t => t.IsClass))
            {
                var mi = typeof(Serializer).GetMethod("Deserialize").MakeGenericMethod(type);
                var m = mi.CreateDelegate(typeof(Func<MemoryStream, APINetworkPacket>)) as Func<MemoryStream, APINetworkPacket>;

                int typeId = (int)(Activator.CreateInstance(type) as APINetworkPacket).Type;

                map[typeId] = m;
            }
        }


        public byte[] SerializeWithHeader(APINetworkPacket packet)
        {
            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                writer.Write(0);
                writer.Write((int)packet.Type);
                Serializer.Serialize(ms, packet);
                
                ms.Position = 0;
                writer.Write((int)ms.Length);

                ms.Position = ms.Length;
                return ms.ToArray();
            }
        }

        public APINetworkPacket DeserializePacket(int packetId, byte[] packet)
        {            
            if (packetId < 0 || packetId >= map.Length || map[packetId] == null)
                throw new InvalidOperationException("Packet id " + packetId + " has no registered deserializer");

            using (var ms = new MemoryStream(packet))
            {
                return map[packetId](ms);
            }
        }
    }
}
