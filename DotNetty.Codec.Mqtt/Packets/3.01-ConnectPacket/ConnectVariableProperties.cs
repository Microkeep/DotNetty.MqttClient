using DotNetty.Buffers;

namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.1.2.11]
/// </summary>
public class ConnectVariableProperties : Properties
{
    /// <summary>
    /// 17 (0x11)，[MQTTv5.0-3.1.2.11.2]
    /// </summary>
    public uint SessionExpiryInterval { get; set; }

    /// <summary>
    /// 33 (0x21)，[MQTTv5.0-3.1.2.11.3]
    /// </summary>
    public ushort ReceiveMaximum { get; set; }

    /// <summary>
    /// 39 (0x27)，[MQTTv5.0-3.1.2.11.4]
    /// </summary>
    public uint MaximumPacketSize { get; set; }

    /// <summary>
    /// 34 (0x22)，[MQTTv5.0-3.1.2.11.5]
    /// </summary>
    public ushort TopicAliasMaximum { get; set; }

    /// <summary>
    /// 25 (0x19)，[MQTTv5.0-3.1.2.11.6]
    /// </summary>
    public bool RequestResponseInformation { get; set; }

    /// <summary>
    /// 23 (0x17)，[MQTTv5.0-3.1.2.11.7]
    /// </summary>
    public bool RequestProblemInformation { get; set; }

    /// <summary>
    /// 38 (0x26)，[MQTTv5.0-3.1.2.11.8]
    /// </summary>
    /// UserProperties

    /// <summary>
    /// 21 (0x15)，[MQTTv5.0-3.1.2.11.9]
    /// </summary>
    public string AuthenticationMethod { get; set; }

    /// <summary>
    /// 22 (0x16)，[MQTTv5.0-3.1.2.11.10]
    /// </summary>
    public byte[] AuthenticationData { get; set; }

    public override void Encode(IByteBuffer buffer)
    {
        var buf = Unpooled.Buffer();
        //[MQTTv5.0-3.1.2.11.2]
        if (SessionExpiryInterval != 0)
        {
            buf.WriteByte((byte)PropertyId.SessionExpiryInterval);
            buf.WriteAsFourByteInteger(SessionExpiryInterval);
        }


        //[MQTTv5.0-3.1.2.11.3]
        if (ReceiveMaximum != 0)
        {
            buf.WriteByte((byte)PropertyId.ReceiveMaximum);
            buf.WriteUnsignedShort((byte)ReceiveMaximum);
        }

        //[MQTTv5.0-3.1.2.11.4]
        if (MaximumPacketSize != 0)
        {
            buf.WriteByte((byte)PropertyId.MaximumPacketSize);
            buf.WriteAsFourByteInteger(MaximumPacketSize);
        }

        //[MQTTv5.0-3.1.2.11.5]
        if (TopicAliasMaximum != 0)
        {
            buf.WriteByte((byte)PropertyId.TopicAliasMaximum);
            buf.WriteUnsignedShort((byte)TopicAliasMaximum);
        }

        //[MQTTv5.0-3.1.2.11.6]
        if (!RequestResponseInformation)
        {
            buf.WriteByte((byte)PropertyId.RequestResponseInformation);
            buf.WriteByte(0x1);
        }

        //[MQTTv5.0-3.1.2.11.7]
        if (!RequestProblemInformation)
        {
            buf.WriteByte((byte)PropertyId.RequestProblemInformation);
            buf.WriteByte(0x1);
        }

        //[MQTTv5.0-3.1.2.11.8]
        base.Encode(buf); //UserProperties

        //[MQTTv5.0-3.1.2.11.9]
        if (!string.IsNullOrEmpty(AuthenticationMethod))
        {
            buf.WriteByte((byte)PropertyId.AuthenticationMethod);
            buf.WriteString(AuthenticationMethod);
        }

        //[MQTTv5.0-3.1.2.11.10]
        if (AuthenticationData?.Length > 0)
        {
            buf.WriteByte((byte)PropertyId.AuthenticationData);
            buf.WriteBytesArray(AuthenticationData);
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
                //[MQTTv5.0-3.1.2.11.2]
                case PropertyId.SessionExpiryInterval:
                    SessionExpiryInterval = buffer.ReadAsFourByteInteger(ref propertiesLength);
                    break;
                //[MQTTv5.0-3.1.2.11.3]
                case PropertyId.ReceiveMaximum:
                    ReceiveMaximum = buffer.ReadUnsignedShort(ref propertiesLength);
                    break;
                //[MQTTv5.0-3.1.2.11.4]
                case PropertyId.MaximumPacketSize:
                    MaximumPacketSize = buffer.ReadAsFourByteInteger(ref propertiesLength);
                    break;
                //[MQTTv5.0-3.1.2.11.5]
                case PropertyId.TopicAliasMaximum:
                    TopicAliasMaximum = buffer.ReadUnsignedShort(ref propertiesLength);
                    break;
                //[MQTTv5.0-3.1.2.11.6]
                case PropertyId.RequestResponseInformation:
                    RequestResponseInformation = buffer.ReadByte(ref propertiesLength) == 0x01;
                    break;
                //[MQTTv5.0-3.1.2.11.7]
                case PropertyId.RequestProblemInformation:
                    RequestProblemInformation = buffer.ReadByte(ref propertiesLength) == 0x01;
                    break;
                //[MQTTv5.0-3.1.2.11.8]
                case PropertyId.UserProperty:
                    var name = buffer.ReadString(ref propertiesLength);
                    var value = buffer.ReadString(ref propertiesLength);
                    ((IList<UserProperty>)UserProperties).Add(new UserProperty(name, value));
                    break;
                //[MQTTv5.0-3.1.2.11.9]
                case PropertyId.AuthenticationMethod:
                    AuthenticationMethod = buffer.ReadString(ref propertiesLength);
                    break;
                //[MQTTv5.0-3.1.2.11.10]
                case PropertyId.AuthenticationData:
                    AuthenticationData = buffer.ReadBytesArray(ref propertiesLength);
                    break;
            }
        }
    }
}
