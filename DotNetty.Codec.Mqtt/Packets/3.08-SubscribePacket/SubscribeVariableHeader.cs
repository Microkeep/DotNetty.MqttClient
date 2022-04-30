using DotNetty.Buffers;

namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.8.2]
/// </summary>
public class SubscribeVariableHeader : PacketIdVariableHeader
{
    public SubscribeVariableHeader() 
        : this(0) { }
    public SubscribeVariableHeader(ushort packetId)
        : base(packetId)
    {
        Properties = new SubscribeVariableProperties();
    }

    /// <summary>
    /// [MQTTv5.0-3.8.2.1]
    /// </summary>
    public SubscribeVariableProperties Properties { get; set; }

    public override void Encode(IByteBuffer buffer, FixedHeader fixedHeader)
    {
        //PacketId
        base.Encode(buffer, fixedHeader);

        //[MQTTv5.0-3.8.2.1]
        Properties.Encode(buffer);
    }

    public override void Decode(IByteBuffer buffer, ref FixedHeader fixedHeader)
    {
        //PacketId
        base.Decode(buffer, ref fixedHeader);

        //[MQTTv5.0-3.8.2.1]
        Properties.Decode(buffer, ref fixedHeader.RemainingLength);
    }
}
