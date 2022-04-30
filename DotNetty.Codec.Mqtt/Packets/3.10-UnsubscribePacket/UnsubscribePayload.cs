using DotNetty.Buffers;
using DotNetty.Codecs;

namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.10.3]
/// </summary>
public class UnsubscribePayload : Payload
{
    public UnsubscribePayload()
        : this(null) { }
    public UnsubscribePayload(string[] topics)
    {
        Topics = topics;
    }

    /// <summary>
    /// [MQTTv5.0-3.10.3]
    /// </summary>
    public string[] Topics { get; set; }

    public override void Encode(IByteBuffer buffer, VariableHeader variableHeader)
    {
        foreach (var topic in Topics)
        {
            buffer.WriteString(topic);
        }
    }

    public override void Decode(IByteBuffer buffer, VariableHeader variableHeader, ref int remainingLength)
    {
        var topics = new List<string>();
        while (remainingLength > 0)
        {
            var topic = buffer.ReadString(ref remainingLength);
            ValidateTopicFilter(topic);
            topics.Add(topic);
        }

        Topics = topics.ToArray();
        if (Topics.Length == 0)
            throw new DecoderException("The Payload of an UNSUBSCRIBE packet MUST contain at least one Topic Filter. An UNSUBSCRIBE packet with no payload is a protocol violation.");
    }
}
