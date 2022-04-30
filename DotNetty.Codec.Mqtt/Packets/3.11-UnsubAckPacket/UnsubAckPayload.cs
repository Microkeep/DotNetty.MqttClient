using DotNetty.Buffers;

namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.11.3]
/// </summary>
public class UnsubAckPayload : Payload
{
    public UnsubAckPayload() 
        : this(new List<UnsubscribeReasonCode>()) { }
    public UnsubAckPayload(IList<UnsubscribeReasonCode> reasonCodes)
    {
        ReasonCodes = reasonCodes;
    }

    /// <summary>
    /// [MQTTv5.0-3.11.3]
    /// </summary>
    public IList<UnsubscribeReasonCode> ReasonCodes { get; set; }

    public override void Encode(IByteBuffer buffer, VariableHeader variableHeader)
    {
        //[MQTTv5.0-3.11.3]
        foreach (var reasonCode in ReasonCodes)
            buffer.WriteByte((byte)reasonCode);
    }

    public override void Decode(IByteBuffer buffer, VariableHeader variableHeader, ref int remainingLength)
    {
        //[MQTTv5.0-3.11.3]
        remainingLength -= buffer.ReadableBytes;
        while (buffer.ReadableBytes > 0)
        {
            var reasonCode = (UnsubscribeReasonCode)buffer.ReadByte();
            ReasonCodes.Add(reasonCode);
        }
    }
}
