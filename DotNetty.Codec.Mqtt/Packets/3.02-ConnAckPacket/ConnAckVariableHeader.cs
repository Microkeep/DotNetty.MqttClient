using DotNetty.Buffers;

namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.2.2]
/// </summary>
public class ConnAckVariableHeader : VariableHeader
{
    /// <summary>
    /// [MQTTv5.0-3.2.2]
    /// </summary>
    public ConnAckVariableHeader()
    {
        Properties = new ConnAckVariableProperties();
    }

    /// <summary>
    /// [MQTTv5-3.2.2.1]
    /// </summary>
    public bool SessionPresent { get; set; }

    /// <summary>
    /// [MQTTv5.0-3.2.2.2]
    /// </summary>
    public ConnectReasonCode ReasonCode { get; set; }

    /// <summary>
    /// [MQTTv5.0-3.2.2.3]
    /// </summary>
    public ConnAckVariableProperties Properties { get; }

    public override void Encode(IByteBuffer buffer)
    {
        //[MQTTv5.0-3.2.2.1]
        buffer.WriteByte(SessionPresent ? 0x01 : 0x00);

        //[MQTTv5.0-3.2.2.2]
        buffer.WriteByte((byte)ReasonCode);

        //[MQTTv5.0-3.2.2.3]
        Properties.Encode(buffer);
    }

    public override void Decode(IByteBuffer buffer, ref FixedHeader fixedHeader)
    {
        //TODO
        //[MQTTv5.0-3.2.2.1]
        SessionPresent = (buffer.ReadByte(ref fixedHeader.RemainingLength) & 0x01) == 0x01;

        //[MQTTv5.0-3.2.2.2]
        ReasonCode = (ConnectReasonCode)buffer.ReadByte(ref fixedHeader.RemainingLength);

        //[MQTTv5.0-3.2.2.3]
        Properties.Decode(buffer, ref fixedHeader.RemainingLength);
    }
}
