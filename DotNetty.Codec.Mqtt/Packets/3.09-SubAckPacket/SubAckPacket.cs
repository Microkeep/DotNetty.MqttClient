namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.9]
/// </summary>
public class SubAckPacket : PacketWithId
{
    public SubAckPacket()
        : this(new SubAckVariableHeader(), new SubAckPayload()) { }
    public SubAckPacket(SubAckVariableHeader variableHeader, SubAckPayload payload)
        : base(variableHeader, payload) { }

    public IList<SubscribeReasonCode> ReasonCodes
    {
        get => ((SubAckPayload)Payload).ReasonCodes;
    }
}
