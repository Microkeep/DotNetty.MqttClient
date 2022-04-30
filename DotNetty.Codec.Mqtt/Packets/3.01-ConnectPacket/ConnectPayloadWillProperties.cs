using DotNetty.Buffers;

namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.1.3.2]
/// </summary>
public class ConnectPayloadWillProperties : Properties
{
    /// <summary>
    /// 24 (0x18)，[MQTTv5.0-3.1.3.2.2]
    /// </summary>
    public uint WillDelayInterval { get; set; }

    /// <summary>
    /// 1 (0x01)，[MQTTv5.0-3.1.3.2.3]
    /// </summary>
    public PayloadFormatIndicator PayloadFormatIndicator { get; set; } = PayloadFormatIndicator.Unspecified;

    /// <summary>
    /// 2 (0x02)，[MQTTv5.0-3.1.3.2.4]
    /// </summary>
    public uint MessageExpiryInterval { get; set; }

    /// <summary>
    /// 3 (0x03)，[MQTTv5.0-3.1.3.2.5]
    /// </summary>
    public string ContentType { get; set; }

    /// <summary>
    /// 8 (0x08)，[MQTTv5.0-3.1.3.2.6]
    /// </summary> 
    public string ResponseTopic { get; set; }

    /// <summary>
    /// 9 (0x09)，[MQTTv5.0-3.1.3.2.7]
    /// </summary>
    public byte[] CorrelationData { get; set; }

    /// <summary>
    /// 38 (0x26)，[MQTTv5.0-3.1.3.2.8]
    /// </summary>
    /// UserProperties

    public override void Encode(IByteBuffer buffer)
    {
        var buf = Unpooled.Buffer();

        //[MQTTv5.0-3.1.3.2.2]
        if (WillDelayInterval != 0)
        {
            buf.WriteByte((byte)PropertyId.WillDelayInterval);
            buf.WriteAsFourByteInteger(WillDelayInterval);
        }

        //[MQTTv5.0-3.1.3.2.3]
        if (PayloadFormatIndicator != PayloadFormatIndicator.Unspecified)
        {
            buf.WriteByte((byte)PropertyId.PayloadFormatIndicator);
            buf.WriteByte((byte)PayloadFormatIndicator);
        }

        //[MQTTv5.0-3.1.3.2.4]
        if (MessageExpiryInterval != 0)
        {
            buf.WriteByte((byte)PropertyId.MessageExpiryInterval);
            buf.WriteAsFourByteInteger(MessageExpiryInterval);
        }

        //[MQTTv5.0-3.1.3.2.5]
        if (!string.IsNullOrEmpty(ContentType))
            buf.WriteString(ContentType);

        //[MQTTv5.0-3.1.3.2.6]
        if (!string.IsNullOrEmpty(ResponseTopic))
            buf.WriteString(ResponseTopic);

        //[MQTTv5.0-3.1.3.2.7]
        if (CorrelationData?.Length > 0)
            buf.WriteBytesArray(CorrelationData);

        //[MQTTv5.0-3.1.3.2.8]
        base.Encode(buf); //UserProperties

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
                //[MQTTv5.0-3.1.3.2.2]
                case PropertyId.WillDelayInterval:
                    WillDelayInterval = buffer.ReadAsFourByteInteger(ref propertiesLength);
                    break;
                //[MQTTv5.0-3.1.3.2.3]
                case PropertyId.PayloadFormatIndicator:
                    PayloadFormatIndicator = (PayloadFormatIndicator)buffer.ReadByte(ref propertiesLength);
                    break;
                //[MQTTv5.0-3.1.3.2.4]
                case PropertyId.MessageExpiryInterval:
                    MessageExpiryInterval = buffer.ReadAsFourByteInteger(ref propertiesLength);
                    break;
                //[MQTTv5.0-3.1.3.2.5]
                case PropertyId.ContentType:
                    ContentType = buffer.ReadString(ref propertiesLength);
                    break;
                //[MQTTv5.0-3.1.3.2.6]
                case PropertyId.ResponseTopic:
                    ResponseTopic = buffer.ReadString(ref propertiesLength);
                    break;
                //[MQTTv5.0-3.1.3.2.7]
                case PropertyId.CorrelationData:
                    CorrelationData = buffer.ReadBytesArray(ref propertiesLength);
                    break;
                //[MQTTv5.0-3.1.3.2.8]
                case PropertyId.UserProperty:
                    var name = buffer.ReadString(ref propertiesLength);
                    var value = buffer.ReadString(ref propertiesLength);
                    ((IList<UserProperty>)UserProperties).Add(new UserProperty(name, value));
                    break;
            }
        }
    }
}
