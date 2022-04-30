namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.11]
/// </summary>
public sealed class UnsubAckPacket : PacketWithId
{
    public UnsubAckPacket()
        : this(new UnsubAckVariableHeader(), new UnsubAckPayload()) { }
    public UnsubAckPacket(ushort packetId, params UnsubscribeReasonCode[] reasonCodes)
        : this(new UnsubAckVariableHeader(packetId), new UnsubAckPayload(reasonCodes)) { }
    public UnsubAckPacket(UnsubAckVariableHeader variableHeader, UnsubAckPayload payload)
        : base(variableHeader, payload) { }
}
