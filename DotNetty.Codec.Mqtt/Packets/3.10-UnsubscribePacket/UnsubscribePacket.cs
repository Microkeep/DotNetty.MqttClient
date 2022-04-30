namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.10]
/// </summary>
public sealed class UnsubscribePacket : PacketWithId
{
    public UnsubscribePacket()
        : this(new UnsubscribeVariableHeader(), new UnsubscribePayload()) { }
    public UnsubscribePacket(ushort packetId, string[] topics)
        : this(new UnsubscribeVariableHeader(packetId), new UnsubscribePayload(topics)) { }
    public UnsubscribePacket(UnsubscribeVariableHeader variableHeader, UnsubscribePayload payload)
        : base(variableHeader, payload) { }
    public UnsubscribePacket(FixedHeader fixedHeader, UnsubscribeVariableHeader variableHeader, UnsubscribePayload payload)
        : base(fixedHeader, variableHeader, payload) { }

    public IList<string> Topics => ((UnsubscribePayload)Payload).Topics;
}
