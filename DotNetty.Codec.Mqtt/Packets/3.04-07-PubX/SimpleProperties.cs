using DotNetty.Buffers;

namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// PUBACK、PUBCOMP、PUBREC、PUBREL、SUBACK、UNSUBACK, Common Properties
/// </summary>
public class SimpleProperties : Properties
{
    /// <summary>
    /// 31 (0x1F)，Reason String
    /// </summary>
    public string ReasonString { get; set; }

    /// <summary>
    /// 38 (0x26)，User Property
    /// </summary>
    ///UserProperties

    public override void Encode(IByteBuffer buffer)
    {
        var buf = Unpooled.Buffer();

        if (!string.IsNullOrEmpty(ReasonString))
        {
            buf.WriteByte((byte)PropertyId.ReasonString);
            buf.WriteString(ReasonString);
        }

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
                case PropertyId.ReasonString:
                    ReasonString = buffer.ReadString(ref propertiesLength);
                    break;
                case PropertyId.UserProperty:
                    var name = buffer.ReadString(ref propertiesLength);
                    var value = buffer.ReadString(ref propertiesLength);
                    ((IList<UserProperty>)UserProperties).Add(new UserProperty(name, value));
                    break;
            }
        }
    }
}
