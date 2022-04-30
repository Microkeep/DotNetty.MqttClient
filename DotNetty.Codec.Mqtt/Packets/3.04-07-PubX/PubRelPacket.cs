namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.6]
/// </summary>
public sealed class PubRelPacket : PacketWithId
{
    public PubRelPacket(ushort packetId = default)
        : base(new PubVariableHeader<PubRelReasonCode>(packetId)) { }
}
