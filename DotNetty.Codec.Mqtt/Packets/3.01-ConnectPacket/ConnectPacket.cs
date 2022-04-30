namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.1]
/// </summary>
public sealed class ConnectPacket : Packet
{
    public ConnectPacket() : this(new ConnectVariableHeader(), new ConnectPayload()) { }

    public ConnectPacket(ConnectVariableHeader variableHeader, ConnectPayload payload)
        : base(variableHeader, payload) { }
}
