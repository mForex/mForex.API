using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mForex.API
{
    class ReceiveBuffer
    {
        public byte[] buffer;
        public int bufferIdx;

        public ReceiveBuffer()
        {
            this.buffer = new byte[32768];
            this.bufferIdx = 0;
        }

        public void PushBytes(ArraySegment<byte> bytes)
        {
            ResizeBuffer(bytes.Count);
            
            Buffer.BlockCopy(bytes.Array, bytes.Offset, buffer, bufferIdx, bytes.Count);
            bufferIdx += bytes.Count;
        }

        public bool TryAcquirePacket(out int packetId, out byte[] packet)
        {
            packet = null;
            packetId = 0;

            if (bufferIdx < 8)
                return false;

            unsafe
            {
                fixed (byte* bPtr = buffer)
                {
                    int* iPtr = (int*)bPtr;
                    int packetSize = *(iPtr + 0);
                    int packetType = *(iPtr + 1);

                    if (bufferIdx < packetSize)
                        return false;

                    //Copy buffer to packet
                    packet = new byte[packetSize - 8];
                    Buffer.BlockCopy(buffer, 8, packet, 0, packetSize - 8);

                    packetId = packetType;

                    //Remove packet from buffer
                    Buffer.BlockCopy(buffer, packetSize, buffer, 0, bufferIdx - packetSize);
                    bufferIdx -= packetSize;

                    return true;
                }
            }
        }

        private void ResizeBuffer(int bytesToAdd)
        {
            while (bufferIdx + bytesToAdd > buffer.Length)
            {
                var newBuffer = new byte[buffer.Length * 2];
                Buffer.BlockCopy(buffer, 0, newBuffer, 0, bufferIdx);

                this.buffer = newBuffer;
            }
        }
    }
}
