namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.7]
/// </summary>
public sealed class PubCompPacket : PacketWithId
{
    public PubCompPacket(ushort packetId = default)
        : base(new PubVariableHeader<PubCompReasonCode>(packetId)) { }
}
