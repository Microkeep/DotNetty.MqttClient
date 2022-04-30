using DotNetty.Buffers;

namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5-3.8.3]
/// </summary>
public class SubscribePayload : Payload
{
    public SubscribePayload()
        : this(new List<TopicFilter>()) { }

    public SubscribePayload(IReadOnlyCollection<TopicFilter> filters)
    {
        TopicFilters = filters;
    }

    /// <summary>
    /// [MQTTv5-3.8.3.1]
    /// </summary>
    public IReadOnlyCollection<TopicFilter> TopicFilters { get; set; }

    public override void Encode(IByteBuffer buffer, VariableHeader variableHeader)
    {
        //[MQTTv5-3.8.3.1]
        if (TopicFilters?.Count > 0)
        {
            foreach (var topicFilter in TopicFilters)
            {
                buffer.WriteString(topicFilter.TopicName);

                var qos = (byte)topicFilter.Qos;
                if (topicFilter.NoLocal)
                    qos |= 1 << 2;

                if (topicFilter.RetainAsPublished)
                    qos |= 1 << 3;

                if (topicFilter.RetainHandling != RetainHandling.SendAtSubscribe)
                    qos |= (byte)((byte)topicFilter.RetainHandling << 4);

                buffer.WriteByte(qos);
            }
        }
    }

    public override void Decode(IByteBuffer buffer, VariableHeader variableHeader, ref int remainingLength)
    {
        //[MQTTv5-3.8.3.1]
        while (remainingLength > 0)
        {
            var topic = buffer.ReadString(ref remainingLength);
            ValidateTopicFilter(topic);

            var options = buffer.ReadByte(ref remainingLength);

            var qos = (MqttQos)(options & 3);
            var noLocal = (options & (1 << 2)) > 0;
            var retainAsPublished = (options & (1 << 3)) > 0;
            var retainHandling = (RetainHandling)((options >> 4) & 3);

            var topics = new List<TopicFilter>();
            topics.Add(new TopicFilter()
            {
                TopicName = topic,
                Qos = qos,
                NoLocal = noLocal,
                RetainAsPublished = retainAsPublished,
                RetainHandling = retainHandling
            });

            TopicFilters = topics;
        }
    }
}
