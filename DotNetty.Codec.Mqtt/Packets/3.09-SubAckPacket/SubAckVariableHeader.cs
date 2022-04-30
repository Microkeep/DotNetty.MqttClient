using DotNetty.Buffers;

namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.9.2]
/// </summary>
public sealed class SubAckVariableHeader : PacketIdVariableHeader
{
    public SubAckVariableHeader() 
        : this(0) { }
    public SubAckVariableHeader(ushort packetId)
        : base(packetId)
    {
        Properties = new SimpleProperties();
    }

    /// <summary>
    /// [MQTTv5.0-3.9.2.1]
    /// </summary>
    public SimpleProperties Properties { get; } = new SimpleProperties();

    public override void Encode(IByteBuffer buffer, FixedHeader fixedHeader)
    {
        //PacketId
        base.Encode(buffer, fixedHeader);

        //[MQTTv5.0-3.9.2.1]
        Properties.Encode(buffer);
    }

    public override void Decode(IByteBuffer buffer, ref FixedHeader fixedHeader)
    {
        //PacketId
        base.Decode(buffer, ref fixedHeader);

        //[MQTTv5.0-3.9.2.1]
        Properties.Decode(buffer, ref fixedHeader.RemainingLength);
    }
}
