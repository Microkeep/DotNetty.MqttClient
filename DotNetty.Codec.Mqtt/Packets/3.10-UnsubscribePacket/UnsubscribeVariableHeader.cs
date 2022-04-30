using DotNetty.Buffers;

namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.10.2]
/// </summary>
public sealed class UnsubscribeVariableHeader : PacketIdVariableHeader
{
    public UnsubscribeVariableHeader() 
        : this(0) { }
    public UnsubscribeVariableHeader(ushort packetId)
        : base(packetId)
    {
        Properties = new UnsubscribeVariableProperties();
    }

    /// <summary>
    /// [MQTTv5.0-3.10.2.1.2]
    /// </summary>
    public UnsubscribeVariableProperties Properties { get; }

    public override void Encode(IByteBuffer buffer, FixedHeader fixedHeader)
    {
        //PacketId
        base.Encode(buffer, fixedHeader);

        //[MQTTv5.0-3.10.2.1.2]
        Properties.Encode(buffer);
    }

    public override void Decode(IByteBuffer buffer, ref FixedHeader fixedHeader)
    {
        //PacketId
        base.Decode(buffer, ref fixedHeader);

        //[MQTTv5.0-3.10.2.1.2]
        Properties.Decode(buffer, ref fixedHeader.RemainingLength);
    }
}
