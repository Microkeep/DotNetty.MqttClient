namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.14]
/// </summary>
public sealed class DisconnectPacket : Packet
{
    public static readonly DisconnectPacket Instance = new DisconnectPacket();
    public DisconnectPacket() 
        : base(new DisconnectVariableHeader()) { }
}
