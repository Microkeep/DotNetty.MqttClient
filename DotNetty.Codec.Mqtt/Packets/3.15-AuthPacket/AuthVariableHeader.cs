using DotNetty.Buffers;

namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.15.2]
/// QoS level = 1
/// </summary>
public sealed class AuthVariableHeader : VariableHeader
{
    /// <summary>
    /// [MQTTv5.0-3.15.2.1]
    /// </summary>
    public AuthenticateReasonCode ReasonCode { get; set; }


    /// <summary>
    /// [MQTTv5.0-3.15.2.2]
    /// </summary>
    public AuthVariableProperties Properties { get; } = new AuthVariableProperties();

    public override void Encode(IByteBuffer buffer)
    {
        //[MQTTv5.0-3.15.2.1]
        buffer.WriteByte((byte)ReasonCode);

        //[MQTTv5.0-3.15.2.2]
        Properties.Encode(buffer);
    }

    public override void Decode(IByteBuffer buffer, ref FixedHeader fixedHeader)
    {
        //[MQTTv5.0-3.15.2.1]
        ReasonCode = (AuthenticateReasonCode)buffer.ReadByte(ref fixedHeader.RemainingLength);

        //[MQTTv5.0-3.15.2.2]
        Properties.Decode(buffer, ref fixedHeader.RemainingLength);
    }
}
