using DotNetty.Buffers;

namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.15.2.2]
/// </summary>
public class AuthVariableProperties : Properties
{
    /// <summary>
    /// 21 (0x15)，[MQTTv5.0-3.15.2.2.2]
    /// </summary>
    public string AuthenticationMethod { get; set; }

    /// <summary>
    /// 22 (0x16)，[MQTTv5.0-3.15.2.2.3]
    /// </summary>
    public byte[] AuthenticationData { get; set; }

    /// <summary>
    /// 31 (0x1F)，[MQTTv5.0-3.15.2.2.4]
    /// </summary>
    public string ReasonString { get; set; }

    /// <summary>
    /// 38 (0x26)，[MQTTv5-3.3.15.2.2.5]
    /// </summary>
    ///UserProperties

    public override void Encode(IByteBuffer buffer)
    {
        var buf = Unpooled.Buffer();

        //[MQTTv5.0-3.15.2.2.2]
        if (!string.IsNullOrEmpty(AuthenticationMethod))
        {
            buf.WriteByte((byte)PropertyId.AuthenticationMethod);
            buf.WriteString(AuthenticationMethod);
        }

        //[MQTTv5.0-3.15.2.2.3]
        if (AuthenticationData?.Length > 0)
        {
            buf.WriteByte((byte)PropertyId.AuthenticationData);
            buf.WriteBytesArray(AuthenticationData);
        }

        //[MQTTv5.0-3.15.2.2.4]
        if (!string.IsNullOrEmpty(ReasonString))
        {
            buf.WriteByte((byte)PropertyId.ReasonString);
            buf.WriteString(ReasonString);
        }

        //[MQTTv5.0-3.15.2.2.5]
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
                //[MQTTv5.0-3.15.2.2.2]
                case PropertyId.AuthenticationMethod:
                    AuthenticationMethod = buffer.ReadString(ref propertiesLength);
                    break;
                //[MQTTv5.0-3.15.2.2.3]
                case PropertyId.AuthenticationData:
                    AuthenticationData = buffer.ReadBytesArray(ref propertiesLength);
                    break;
                //[MQTTv5.0-3.15.2.2.4]
                case PropertyId.ReasonString:
                    ReasonString = buffer.ReadString(ref propertiesLength);
                    break;
                //[MQTTv5.0-3.15.2.2.5]
                case PropertyId.UserProperty:
                    var name = buffer.ReadString(ref propertiesLength);
                    var value = buffer.ReadString(ref propertiesLength);
                    ((IList<UserProperty>)UserProperties).Add(new UserProperty(name, value));
                    break;
            }
        }
    }
}
