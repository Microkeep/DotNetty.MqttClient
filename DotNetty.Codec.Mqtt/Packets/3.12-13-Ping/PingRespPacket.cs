namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.13]
/// </summary>
public class PingRespPacket : Packet
{
    public static readonly PingRespPacket Instance = new PingRespPacket();
}
