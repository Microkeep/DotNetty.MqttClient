namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.15]
/// </summary>
public sealed class AuthPacket : Packet
{
    public static readonly AuthPacket Instance = new AuthPacket();
    public AuthPacket() 
        : base(new AuthVariableHeader()) { }
}
