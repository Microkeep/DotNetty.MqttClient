using DotNetty.Buffers;

namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.14.2]
/// </summary>
public sealed class DisconnectVariableHeader : VariableHeader
{
    /// <summary>
    /// [MQTTv5.0-3.14.2.1]
    /// </summary>
    public DisconnectReasonCode ReasonCode { get; set; }

    /// <summary>
    /// [MQTTv5.0-3.14.2.2]
    /// </summary>
    public DisconnectVariableProperties Properties { get; } = new DisconnectVariableProperties();

    public override void Encode(IByteBuffer buffer)
    {
        //[MQTTv5.0-3.14.2.1]
        buffer.WriteByte((byte)ReasonCode);

        //[MQTTv5.0-3.14.2.2]
        Properties.Encode(buffer);
    }

    public override void Decode(IByteBuffer buffer, ref FixedHeader fixedHeader)
    {
        //[MQTTv5.0-3.14.2.1]
        ReasonCode = (DisconnectReasonCode)buffer.ReadByte(ref fixedHeader.RemainingLength);

        //[MQTTv5.0-3.14.2.2]
        Properties.Decode(buffer, ref fixedHeader.RemainingLength);
    }
}
