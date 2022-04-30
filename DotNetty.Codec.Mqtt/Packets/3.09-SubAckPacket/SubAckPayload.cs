using DotNetty.Buffers;

namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.9.3]
/// </summary>
public class SubAckPayload : Payload
{
    public SubAckPayload() 
        : this(new List<SubscribeReasonCode>()) { }
    public SubAckPayload(IList<SubscribeReasonCode> reasonCodes)
    {
        ReasonCodes = reasonCodes;
    }

    /// <summary>
    /// [MQTTv5.0-3.9.3]
    /// </summary>
    public IList<SubscribeReasonCode> ReasonCodes { get; set; }

    public override void Encode(IByteBuffer buffer, VariableHeader variableHeader)
    {
        //[MQTTv5.0-3.9.3]
        foreach (var reasonCode in ReasonCodes)
        {
            buffer.WriteByte((byte)reasonCode);
        }
    }

    public override void Decode(IByteBuffer buffer, VariableHeader variableHeader, ref int remainingLength)
    {
        //[MQTTv5.0-3.9.3]
        while (remainingLength > 0)
        {
            var reasonCode = (SubscribeReasonCode)buffer.ReadByte(ref remainingLength);
            ReasonCodes.Add(reasonCode);
        }
    }
}
