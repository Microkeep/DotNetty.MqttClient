namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5-3.4]
/// </summary>
public sealed class ConnAckPacket : Packet
{
    public ConnAckPacket() 
        : this(new ConnAckVariableHeader()) { }
    public ConnAckPacket(ConnAckVariableHeader variableHeader)
        : base(variableHeader) { }
}
