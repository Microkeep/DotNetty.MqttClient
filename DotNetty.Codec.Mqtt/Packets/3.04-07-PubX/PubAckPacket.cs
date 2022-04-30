namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.4]
/// </summary>
public sealed class PubAckPacket : PacketWithId
{
    public PubAckPacket(ushort packetId = default)
        : base(new PubVariableHeader<PubAckReasonCode>(packetId)) { }
}
