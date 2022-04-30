using DotNetty.Buffers;

namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.8.2.1]
/// </summary>
public class SubscribeVariableProperties : Properties
{
    /// <summary>
    /// 11 (0x0B)，[MQTTv5.0-3.8.2.1.2]
    /// </summary>
    public uint SubscriptionIdentifier { get; set; }

    /// <summary>
    /// 38 (0x26)，[MQTTv5.0-3.8.2.1.3]
    /// </summary>
    ///UserProperties

    public override void Encode(IByteBuffer buffer)
    {
        var buf = Unpooled.Buffer();

        //[MQTTv5.0-3.8.2.1.2]
        if (SubscriptionIdentifier > 0)
        {
            buf.WriteByte((byte)PropertyId.SubscriptionIdentifier);
            buf.WriteAsVariableByteInteger(SubscriptionIdentifier);
        }

        //[MQTTv5.0-3.8.2.1.3]
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
                //[MQTTv5.0-3.8.2.1.2]
                case PropertyId.SubscriptionIdentifier:
                    SubscriptionIdentifier = buffer.ReadAsVariableByteInteger(ref propertiesLength);
                    break;
                //[MQTTv5.0-3.8.2.1.3]
                case PropertyId.UserProperty:
                    var name = buffer.ReadString(ref propertiesLength);
                    var value = buffer.ReadString(ref propertiesLength);
                    ((IList<UserProperty>)UserProperties).Add(new UserProperty(name, value));
                    break;
            }
        }
    }
}
