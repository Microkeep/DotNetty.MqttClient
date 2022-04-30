namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.5]
/// </summary>
public sealed class PubRecPacket : PacketWithId
{
    public PubRecPacket(ushort packetId = default)
        : base(new PubVariableHeader<PubRecReasonCode>(packetId)) { }
}
