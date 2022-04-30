using DotNetty.Buffers;

namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.3.2]
/// </summary>
public class PublishVariableHeader : PacketIdVariableHeader
{
    /// <summary>
    /// [MQTTv5.0-3.3.2]
    /// </summary>
    public PublishVariableHeader()
    {
        Properties = new PublishVariableProperties();
    }

    /// <summary>
    /// [MQTTv5.0-3.3.2.1]
    /// </summary>
    public string TopicName { get; set; }

    /// <summary>
    /// [MQTTv5.0-3.3.2.2]
    /// </summary>
    ///PacketId

    /// <summary>
    /// [MQTTv5.0-3.3.2.3]
    /// </summary>
    public PublishVariableProperties Properties { get; set; }

    public override void Encode(IByteBuffer buffer, FixedHeader fixedHeader)
    {
        //[MQTTv5.0-3.3.2.1]
        buffer.WriteString(TopicName);

        //[MQTTv5.0-3.3.2.2]
        if (fixedHeader.GetQos() > MqttQos.AtMostOnce)
            base.Encode(buffer, fixedHeader);  //PacketId

        //[MQTTv5.0-3.3.2.3]
        Properties.Encode(buffer);
    }

    public override void Decode(IByteBuffer buffer, ref FixedHeader fixedHeader)
    {
        //[MQTTv5.0-3.3.2.1]
        TopicName = buffer.ReadString(ref fixedHeader.RemainingLength);

        //[MQTTv5.0-3.3.2.2]
        if (fixedHeader.GetQos() > MqttQos.AtMostOnce)
            base.Decode(buffer, ref fixedHeader); //PacketId

        //[MQTTv5.0-3.3.2.3]
        Properties.Decode(buffer,  ref fixedHeader.RemainingLength);
    }
}
