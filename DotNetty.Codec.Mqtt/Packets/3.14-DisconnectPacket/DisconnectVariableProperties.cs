using DotNetty.Buffers;

namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.14.2.2]
/// </summary>
public class DisconnectVariableProperties : Properties
{
    /// <summary>
    /// 17 (0x11)，[MQTTv5.0-3.14.2.2.2]
    /// </summary>
    public uint SessionExpiryInterval { get; set; }

    /// <summary>
    /// 31 (0x1F)，[MQTTv5-3.14.2.2.3]
    /// </summary>
    public string ReasonString { get; set; }

    /// <summary>
    /// 38 (0x26)，[MQTTv5-3.14.2.2.4]
    /// </summary>
    ///UserProperties

    /// <summary>
    /// 28 (0x1C)，[MQTTv5-3.14.2.2.5]
    /// </summary>
    public string ServerReference { get; set; }

    public override void Encode(IByteBuffer buffer)
    {
        var buf = Unpooled.Buffer();

        //[MQTTv5.0-3.14.2.2.2]
        if (SessionExpiryInterval != 0)
        {
            buf.WriteByte((byte)PropertyId.SessionExpiryInterval);
            buf.WriteAsFourByteInteger(SessionExpiryInterval);
        }

        //[MQTTv5-3.14.2.2.3]
        if (!string.IsNullOrEmpty(ReasonString))
        {
            buf.WriteByte((byte)PropertyId.ReasonString);
            buf.WriteString(ReasonString);
        }

        //[MQTTv5-3.14.2.2.4]
        base.Encode(buf); //UserProperties

        //[MQTTv5-3.14.2.2.5]
        if (!string.IsNullOrEmpty(ServerReference))
        {
            buf.WriteByte((byte)PropertyId.ServerReference);
            buf.WriteString(ServerReference);
        }

        buffer.WriteAsVariableByteInteger((uint)buf.ReadableBytes);
        buffer.WriteBytes(buf);
    }

    public void Decode(IByteBuffer buffer, ref int propertiesLength)
    {
        while (true)
        {
            if (propertiesLength == 0 || buffer.ReadableBytes == 0)
                return;

            var id = (PropertyId)buffer.ReadByte(ref propertiesLength);
            switch (id)
            {
                //[MQTTv5.0-3.14.2.2.2]
                case PropertyId.SessionExpiryInterval:
                    SessionExpiryInterval = buffer.ReadAsFourByteInteger(ref propertiesLength);
                    break;
                //[MQTTv5-3.14.2.2.3]
                case PropertyId.ReasonString:
                    ReasonString = buffer.ReadString(ref propertiesLength);
                    break;
                //[MQTTv5-3.14.2.2.4]
                case PropertyId.UserProperty:
                    var name = buffer.ReadString(ref propertiesLength);
                    var value = buffer.ReadString(ref propertiesLength);
                    ((IList<UserProperty>)UserProperties).Add(new UserProperty(name, value));
                    break;
                //[MQTTv5-3.14.2.2.5]
                case PropertyId.ServerReference:
                    ServerReference = buffer.ReadString(ref propertiesLength);
                    break;
            }
        }
    }
}
