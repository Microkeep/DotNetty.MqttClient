using DotNetty.Buffers;

namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5-3.2.2.3]
/// </summary>
public class ConnAckVariableProperties : Properties
{
    /// <summary>
    /// 17 (0x11)，[MQTTv5-3.2.2.3.2]
    /// </summary>
    public uint SessionExpiryInterval { get; set; }

    /// <summary>
    /// 33 (0x21)，[MQTTv5-3.2.2.3.3]
    /// </summary>
    public ushort ReceiveMaximum { get; set; }

    /// <summary>
    /// 36 (0x24)，[MQTTv5-3.2.2.3.4]
    /// </summary>
    public MqttQos MaximumQoS { get; set; }

    /// <summary>
    /// 37 (0x25)，[MQTTv5-3.2.2.3.5]
    /// </summary>
    public bool RetainAvailable { get; set; }

    /// <summary>
    /// 39 (0x27)，[MQTTv5-3.2.2.3.6]
    /// </summary>
    public uint MaximumPacketSize { get; set; }

    /// <summary>
    /// 18 (0x12)，[MQTTv5-3.2.2.3.7]
    /// </summary>
    public string AssignedClientIdentifier { get; set; }

    /// <summary>
    /// 34 (0x22)，[MQTTv5-3.2.2.3.8]
    /// </summary>
    public ushort TopicAliasMaximum { get; set; }

    /// <summary>
    /// 31 (0x1F)，[MQTTv5-3.2.2.3.9]
    /// </summary>
    public string ReasonString { get; set; }

    /// <summary>
    /// 38 (0x26)，[MQTTv5-3.2.2.3.10]
    /// </summary>
    /// UserProperties

    /// <summary>
    /// 40 (0x28)，[MQTTv5-3.2.2.3.11]
    /// </summary>
    public bool WildcardSubscriptionAvailable { get; set; }

    /// <summary>
    /// 41 (0x29)，[MQTTv5-3.2.2.3.12]
    /// </summary>
    public bool SubscriptionIdentifiersAvailable { get; set; }

    /// <summary>
    /// 42 (0x2A)，[MQTTv5-3.2.2.3.13]
    /// </summary>
    public bool SharedSubscriptionAvailable { get; set; }

    /// <summary>
    /// 19 (0x13)，[MQTTv5-3.2.2.3.14]
    /// </summary>
    public ushort ServerKeepAlive { get; set; }

    /// <summary>
    /// 26 (0x1A)，[MQTTv5-3.2.2.3.15]
    /// </summary>
    public string ResponseInformation { get; set; }

    /// <summary>
    /// 28 (0x1C)，[MQTTv5-3.2.2.3.16]
    /// </summary>
    public string ServerReference { get; set; }

    /// <summary>
    /// 21 (0x15)，[MQTTv5-3.2.2.3.17]
    /// </summary>
    public string AuthenticationMethod { get; set; }

    /// <summary>
    /// 22 (0x16)，[MQTTv5-3.2.2.3.18]
    /// </summary>
    public byte[] AuthenticationData { get; set; }

    public override void Encode(IByteBuffer buffer)
    {
        var buf = Unpooled.Buffer();

        //[MQTTv5-3.2.2.3.2]
        if (SessionExpiryInterval != 0)
        {
            buf.WriteByte((byte)PropertyId.SessionExpiryInterval);
            buf.WriteAsFourByteInteger(SessionExpiryInterval);
        }

        //[MQTTv5-3.2.2.3.3]
        if (ReceiveMaximum != 0)
        {
            buf.WriteByte((byte)PropertyId.ReceiveMaximum);
            buf.WriteByte((byte)ReceiveMaximum);
        }

        //[MQTTv5-3.2.2.3.4]
        if (MaximumQoS != MqttQos.ExactlyOnce)
        {
            buf.WriteByte((byte)PropertyId.MaximumQoS);
            buf.WriteByte((int)MaximumQoS);
        }

        //[MQTTv5-3.2.2.3.5]
        if (RetainAvailable)
        {
            buf.WriteByte((byte)PropertyId.RetainAvailable);
            buf.WriteByte(0x01);
        }

        //[MQTTv5-3.2.2.3.6]
        if (MaximumPacketSize != 0)
        {
            buf.WriteByte((byte)PropertyId.MaximumPacketSize);
            buf.WriteAsFourByteInteger(MaximumPacketSize);
        }

        //[MQTTv5-3.2.2.3.7]
        if (!string.IsNullOrEmpty(AssignedClientIdentifier))
        {
            buf.WriteByte((byte)PropertyId.AssignedClientIdentifier);
            buf.WriteString(AssignedClientIdentifier);
        }

        //[MQTTv5-3.2.2.3.8]
        if (TopicAliasMaximum != 0)
        {
            buf.WriteByte((byte)PropertyId.TopicAliasMaximum);
            buf.WriteUnsignedShort(TopicAliasMaximum);
        }

        //[MQTTv5-3.2.2.3.9]
        if (!string.IsNullOrEmpty(ReasonString))
        {
            buf.WriteByte((byte)PropertyId.ReasonString);
            buf.WriteString(ReasonString);
        }

        //[MQTTv5-3.2.2.3.10]
        base.Encode(buf); //UserProperties

        //[MQTTv5-3.2.2.3.11]
        if (WildcardSubscriptionAvailable)
        {
            buf.WriteByte((byte)PropertyId.WildcardSubscriptionAvailable);
            buf.WriteByte(0x01);
        }

        //[MQTTv5-3.2.2.3.12]
        if (SubscriptionIdentifiersAvailable)
        {
            buf.WriteByte((byte)PropertyId.SubscriptionIdentifiersAvailable);
            buf.WriteByte(0x01);
        }

        //[MQTTv5-3.2.2.3.13]
        if (SharedSubscriptionAvailable)
        {
            buf.WriteByte((byte)PropertyId.SharedSubscriptionAvailable);
            buf.WriteByte(0x01);
        }

        //[MQTTv5-3.2.2.3.14]
        if (ServerKeepAlive != 0)
        {
            buf.WriteByte((byte)PropertyId.ServerKeepAlive);
            buf.WriteUnsignedShort(ServerKeepAlive);
        }

        //[MQTTv5-3.2.2.3.15]
        if (!string.IsNullOrEmpty(ResponseInformation))
        {
            buf.WriteByte((byte)PropertyId.ResponseInformation);
            buf.WriteString(ResponseInformation);
        }

        //[MQTTv5-3.2.2.3.16]
        if (!string.IsNullOrEmpty(ServerReference))
        {
            buf.WriteByte((byte)PropertyId.ServerReference);
            buf.WriteString(ServerReference);
        }

        //[MQTTv5-3.2.2.3.17]
        if (!string.IsNullOrEmpty(AuthenticationMethod))
        {
            buf.WriteByte((byte)PropertyId.AuthenticationMethod);
            buf.WriteString(AuthenticationMethod);
        }

        //[MQTTv5-3.2.2.3.18]
        if (AuthenticationData?.Length > 0)
        {
            buf.WriteByte((byte)PropertyId.AuthenticationData);
            buf.WriteByte((byte)AuthenticationData.Length);
            buf.WriteBytes(AuthenticationData);
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
                //[MQTTv5-3.2.2.3.2]
                case PropertyId.SessionExpiryInterval:
                    SessionExpiryInterval = buffer.ReadAsFourByteInteger(ref propertiesLength);
                    break;
                //[MQTTv5-3.2.2.3.3]
                case PropertyId.ReceiveMaximum:
                    ReceiveMaximum = buffer.ReadUnsignedShort(ref propertiesLength);
                    break;
                //[MQTTv5-3.2.2.3.4]
                case PropertyId.MaximumQoS:
                    MaximumQoS = (MqttQos)buffer.ReadByte(ref propertiesLength);
                    break;
                //[MQTTv5-3.2.2.3.5]
                case PropertyId.RetainAvailable:
                    RetainAvailable = buffer.ReadByte(ref propertiesLength) == 0x01;
                    break;
                //[MQTTv5-3.2.2.3.6]
                case PropertyId.MaximumPacketSize:
                    MaximumPacketSize = buffer.ReadAsFourByteInteger(ref propertiesLength);
                    break;
                //[MQTTv5-3.2.2.3.7]
                case PropertyId.AssignedClientIdentifier:
                    AssignedClientIdentifier = buffer.ReadString(ref propertiesLength);
                    break;
                //[MQTTv5-3.2.2.3.8]
                case PropertyId.TopicAliasMaximum:
                    TopicAliasMaximum = buffer.ReadUnsignedShort(ref propertiesLength);
                    break;
                //[MQTTv5-3.2.2.3.9]
                case PropertyId.ReasonString:
                    ReasonString = buffer.ReadString(ref propertiesLength);
                    break;
                //[MQTTv5-3.2.2.3.10]
                case PropertyId.UserProperty:
                    var name = buffer.ReadString(ref propertiesLength);
                    var value = buffer.ReadString(ref propertiesLength);
                    ((IList<UserProperty>)UserProperties).Add(new UserProperty(name, value));
                    break;
                //[MQTTv5-3.2.2.3.11]
                case PropertyId.WildcardSubscriptionAvailable:
                    WildcardSubscriptionAvailable = buffer.ReadByte(ref propertiesLength) == 0x01;
                    break;
                //[MQTTv5-3.2.2.3.12]
                case PropertyId.SubscriptionIdentifiersAvailable:
                    SubscriptionIdentifiersAvailable = buffer.ReadByte(ref propertiesLength) == 0x01;
                    break;
                //[MQTTv5-3.2.2.3.13]
                case PropertyId.SharedSubscriptionAvailable:
                    SharedSubscriptionAvailable = buffer.ReadByte(ref propertiesLength) == 0x01;
                    break;
                //[MQTTv5-3.2.2.3.14]
                case PropertyId.ServerKeepAlive:
                    ServerKeepAlive = buffer.ReadUnsignedShort(ref propertiesLength);
                    break;
                //[MQTTv5-3.2.2.3.15]
                case PropertyId.ResponseInformation:
                    ResponseInformation = buffer.ReadString(ref propertiesLength);
                    break;
                //[MQTTv5-3.2.2.3.16]
                case PropertyId.ServerReference:
                    ServerReference = buffer.ReadString(ref propertiesLength);
                    break;
                //[MQTTv5-3.2.2.3.17]
                case PropertyId.AuthenticationMethod:
                    AuthenticationMethod = buffer.ReadString(ref propertiesLength);
                    break;
                //[MQTTv5-3.2.2.3.18]
                case PropertyId.AuthenticationData:
                    AuthenticationData = buffer.ReadBytesArray(ref propertiesLength);
                    break;
            }
        }
    }
}
