using DotNetty.Buffers;

namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.1.2]
/// </summary>
public class ConnectVariableHeader : VariableHeader
{
    /// <summary>
    /// [MQTTv5.0-3.1.2]
    /// </summary>
    public ConnectVariableHeader()
    {
        Properties = new ConnectVariableProperties();
    }

    /// <summary>
    /// [MQTTv5.0-3.1.2.1]
    /// </summary>
    public string ProtocolName { get; set; } = Constants.MqttProtocol.ToUpper();

    /// <summary>
    /// [MQTTv5.0-3.1.2.2]
    /// </summary>
    public byte ProtocolVersion { get; internal set; } = Constants.MqttVersion;

    /// <summary>
    /// [MQTTv5.0-3.1.2.3-9]
    /// </summary>
    public ConnectFlags ConnectFlags;

    /// <summary>
    /// [MQTTv5.0-3.1.2.10]
    /// </summary>
    public ushort KeepAlive { get; set; }

    /// <summary>
    /// [MQTTv5.0-3.1.2.11]
    /// </summary>
    public ConnectVariableProperties Properties { get; }

    public override void Encode(IByteBuffer buffer)
    {
        //[MQTTv5.0-3.1.2.1]
        buffer.WriteString(ProtocolName);

        //[MQTTv5.0-3.1.2.2]
        buffer.WriteByte(ProtocolVersion);

        //[MQTTv5.0-3.1.2.3-9]                         
        var connectFlags = ConnectFlags.UserNameFlag.ToByte() << 7;
        connectFlags |= ConnectFlags.PasswordFlag.ToByte() << 6;
        connectFlags |= ConnectFlags.WillRetain.ToByte() << 5;
        connectFlags |= ((byte)ConnectFlags.WillQos) << 3;
        connectFlags |= ConnectFlags.WillFlag.ToByte() << 2;
        connectFlags |= ConnectFlags.CleanStart.ToByte() << 1;
        buffer.WriteByte(connectFlags);

        //[MQTTv5.0-3.1.2.10]
        buffer.WriteShort(KeepAlive);

        //[MQTTv5.0-3.1.2.11]
        Properties?.Encode(buffer);
    }

    public override void Decode(IByteBuffer buffer, ref FixedHeader fixedHeader)
    {
        //[MQTTv5.0-3.1.2.1]
        ProtocolName = buffer.ReadString(ref fixedHeader.RemainingLength);

        //[MQTTv5.0-3.1.2.2]
        ProtocolVersion = buffer.ReadByte(ref fixedHeader.RemainingLength);

        //[MQTTv5.0-3.1.2.3-9]
        int connectFlags = buffer.ReadByte(ref fixedHeader.RemainingLength);
        ConnectFlags.CleanStart = (connectFlags & 0x02) == 0x02;
        ConnectFlags.WillFlag = (connectFlags & 0x04) == 0x04;
        if (ConnectFlags.WillFlag)
        {
            ConnectFlags.WillQos = (MqttQos)((connectFlags & 0x18) >> 3);
            ConnectFlags.WillRetain = (connectFlags & 0x20) == 0x20;
        }
        ConnectFlags.UserNameFlag = (connectFlags & 0x80) == 0x80;
        ConnectFlags.PasswordFlag = (connectFlags & 0x40) == 0x40;

        //[MQTTv5.0-3.1.2.10]
        KeepAlive = buffer.ReadUnsignedShort(ref fixedHeader.RemainingLength);

        //[MQTTv5.0-3.1.2.11]
        Properties.Decode(buffer, ref fixedHeader.RemainingLength);
    }
}
