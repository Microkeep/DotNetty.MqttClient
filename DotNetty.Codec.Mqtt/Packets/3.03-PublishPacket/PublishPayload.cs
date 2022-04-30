using DotNetty.Buffers;

namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.3.3]
/// </summary>
public class PublishPayload : Payload
{
    public byte[] Body { get; set; }

    /// <summary>
    /// [MQTTv5.0-3.3.3]
    /// </summary>
    public PublishPayload() { }

    /// <summary>
    /// [MQTTv5.0-3.3.3]
    /// </summary>
    public PublishPayload(byte[] payload)
    {
        Body = payload;
    }

    public override void Encode(IByteBuffer buffer, VariableHeader variableHeader)
    {
        if (Body != null)
            buffer.WriteBytes(Body);
    }

    public override void Decode(IByteBuffer buffer, VariableHeader variableHeader, ref int remainingLength)
    {
        Body = buffer.ReadSliceArray(ref remainingLength);
    }
}
