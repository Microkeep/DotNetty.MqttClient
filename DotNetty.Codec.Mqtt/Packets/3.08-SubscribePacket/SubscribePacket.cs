namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.8]
/// </summary>
public sealed class SubscribePacket : PacketWithId
{
    public SubscribePacket()
        : this(new SubscribeVariableHeader(), new SubscribePayload()) { }
    public SubscribePacket(ushort packetId, IReadOnlyCollection<TopicFilter> topicFilters)
        : this(new SubscribeVariableHeader(packetId), new SubscribePayload(topicFilters)) { }
    public SubscribePacket(SubscribeVariableHeader variableHeader, SubscribePayload payload)
        : base(variableHeader, payload) { }
    public SubscribePacket(FixedHeader fixedHeader, SubscribeVariableHeader variableHeader, SubscribePayload payload)
        : base(fixedHeader, variableHeader, payload) { }

    public IReadOnlyCollection<TopicFilter> TopicFilters
    {
        get => ((SubscribePayload)Payload).TopicFilters;
    }
}
