using DotNetty.Buffers;
using DotNetty.Codecs;

namespace DotNetty.Codec.Mqtt.Packets;

public class PacketIdVariableHeader : VariableHeader
{
    public ushort PacketId { get; set; }

    public PacketIdVariableHeader() { }

    public PacketIdVariableHeader(ushort packetId)
    {
        PacketId = packetId;
    }

    public override void Encode(IByteBuffer buffer)
    {
        buffer.WriteUnsignedShort(PacketId);
    }

    public override void Decode(IByteBuffer buffer, ref FixedHeader fixedHeader)
    {
        PacketId = buffer.ReadUnsignedShort(ref fixedHeader.RemainingLength);
        if (PacketId == 0)
            throw new DecoderException("SUBSCRIBE, UNSUBSCRIBE, and PUBLISH (in cases where QoS > 0) Control Packets MUST contain a non-zero 16-bit Packet Identifier. [MQTT-2.3.1-1]");
    }
}
