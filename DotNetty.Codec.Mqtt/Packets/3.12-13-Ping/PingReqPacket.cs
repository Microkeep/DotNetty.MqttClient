namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.12]
/// </summary>
public sealed class PingReqPacket : Packet
{
    public static readonly PingReqPacket Instance = new PingReqPacket();
}
