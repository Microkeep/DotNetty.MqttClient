using DotNetty.Buffers;

namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.3.2.3]
/// </summary>
public class PublishVariableProperties : Properties
{
    /// <summary>
    /// [MQTTv5.0-3.3.2.3]
    /// </summary>        
    public PublishVariableProperties()
    {
        SubscriptionIdentifiers = new List<uint>();
    }

    /// <summary>
    /// 1 (0x01)，[MQTTv5.0-3.3.2.3.2]
    /// </summary>
    public PayloadFormatIndicator PayloadFormatIndicator { get; set; } = PayloadFormatIndicator.Unspecified;

    /// <summary>
    /// 2 (0x02)，[MQTTv5.0-3.3.2.3.3]
    /// </summary>
    public uint MessageExpiryInterval { get; set; }

    /// <summary>
    /// 35 (0x23)，[MQTTv5.0-3.3.2.3.4]
    /// </summary>
    public ushort TopicAlias { get; set; }

    /// <summary>
    /// 8 (0x08)，[MQTTv5.0-3.3.2.3.5]
    /// </summary>
    public string ResponseTopic { get; set; }

    /// <summary>
    /// 9 (0x09)，[MQTTv5.0-3.3.2.3.6]
    /// </summary>
    public byte[] CorrelationData { get; set; }

    /// <summary>
    /// 38 (0x26)，[MQTTv5.0-3.3.2.3.7]
    /// </summary>
    ///UserProperties

    /// <summary>
    /// 11 (0x0B)，[MQTTv5.0-3.3.2.3.8]
    /// </summary>
    public IList<uint> SubscriptionIdentifiers { get; set; }

    /// <summary>
    /// 3 (0x03)， [MQTTv5.0-3.3.2.3.9]
    /// </summary>
    public string ContentType { get; set; }

    public override void Encode(IByteBuffer buffer)
    {
        var buf = Unpooled.Buffer();

        //[MQTTv5.0-3.3.2.3.2]
        if (PayloadFormatIndicator != PayloadFormatIndicator.Unspecified)
        {
            buf.WriteByte((byte)PropertyId.PayloadFormatIndicator);
            buf.WriteByte((byte)PayloadFormatIndicator);
        }

        //[MQTTv5.0-3.3.2.3.3]
        if (MessageExpiryInterval != 0)
        {
            buf.WriteByte((byte)PropertyId.MessageExpiryInterval);
            buf.WriteAsFourByteInteger(MessageExpiryInterval);
        }

        //[MQTTv5.0-3.3.2.3.4]
        if (TopicAlias != 0)
        {
            buf.WriteByte((byte)PropertyId.TopicAlias);
            buf.WriteUnsignedShort(TopicAlias);
        }

        //[MQTTv5.0-3.3.2.3.5]
        if (!string.IsNullOrEmpty(ResponseTopic))
        {
            buf.WriteByte((byte)PropertyId.ResponseTopic);
            buf.WriteString(ResponseTopic);
        }

        //[MQTTv5.0-3.3.2.3.6]
        if (CorrelationData?.Length > 0)
        {
            buf.WriteByte((byte)PropertyId.CorrelationData);
            buf.WriteBytesArray(CorrelationData);
        }

        //[MQTTv5.0-3.3.2.3.7]
        base.Encode(buf); //UserProperties

        //[MQTTv5.0-3.3.2.3.8]
        if (SubscriptionIdentifiers?.Count > 0)
        {
            foreach (var subscriptionIdentifier in SubscriptionIdentifiers)
            {
                buf.WriteByte((byte)PropertyId.SubscriptionIdentifier);
                buf.WriteAsFourByteInteger(subscriptionIdentifier);
            }
        }

        //[MQTTv5.0-3.3.2.3.9]
        if (!string.IsNullOrEmpty(ContentType))
        {
            buf.WriteByte((byte)PropertyId.ContentType);
            buf.WriteString(ContentType);
        }

        buffer.WriteAsVariableByteInteger((uint)buf.ReadableBytes);
        buffer.WriteBytes(buf);
    }

    public void Decode(IByteBuffer buffer, ref int remainingLength)
    {
        var propertiesLength = (int)buffer.ReadAsVariableByteInteger(ref remainingLength);
        remainingLength -= propertiesLength;
        while (true)
        {
            if (propertiesLength == 0 || buffer.ReadableBytes == 0)
                return;

            var id = (PropertyId)buffer.ReadByte(ref propertiesLength);
            switch (id)
            {
                //[MQTTv5.0-3.3.2.3.2]
                case PropertyId.PayloadFormatIndicator:
                    PayloadFormatIndicator = (PayloadFormatIndicator)buffer.ReadByte(ref propertiesLength);
                    break;
                //[MQTTv5.0-3.3.2.3.3]
                case PropertyId.MessageExpiryInterval:
                    MessageExpiryInterval = buffer.ReadAsFourByteInteger(ref propertiesLength);
                    break;
                //[MQTTv5.0-3.3.2.3.4]
                case PropertyId.TopicAlias:
                    TopicAlias = buffer.ReadUnsignedShort(ref propertiesLength);
                    break;
                //[MQTTv5.0-3.3.2.3.5]
                case PropertyId.ResponseTopic:
                    ResponseTopic = buffer.ReadString(ref propertiesLength);
                    break;
                //[MQTTv5.0-3.3.2.3.6]
                case PropertyId.CorrelationData:
                    CorrelationData = buffer.ReadBytesArray(ref propertiesLength);
                    break;
                //[MQTTv5.0-3.3.2.3.7]
                case PropertyId.UserProperty:
                    var name = buffer.ReadString(ref propertiesLength);
                    var value = buffer.ReadString(ref propertiesLength);
                    ((IList<UserProperty>)UserProperties).Add(new UserProperty(name, value));
                    break;
                //[MQTTv5.0-3.3.2.3.8]
                case PropertyId.SubscriptionIdentifier:
                    var sid = buffer.ReadAsVariableByteInteger(ref propertiesLength);
                    SubscriptionIdentifiers.Add(sid);
                    break;
                //[MQTTv5.0-3.3.2.3.9]
                case PropertyId.ContentType:
                    ContentType = buffer.ReadString(ref propertiesLength);
                    break;
            }
        }
    }
}
