using DotNetty.Buffers;

namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// PUBACK、PUBCOMP、PUBREC、PUBREL, Common Variable Header
/// </summary>
public sealed class PubVariableHeader<T> : PacketIdVariableHeader
    where T : Enum
{
    public PubVariableHeader(ushort packetId = default) : base(packetId)
    {
        Properties = new SimpleProperties();
    }

    /// <summary>
    /// ReasonCode
    /// </summary>
    public T ReasonCode { get; set; }

    /// <summary>
    /// Properties
    /// </summary>
    public SimpleProperties Properties { get; }

    public override void Encode(IByteBuffer buffer, FixedHeader fixedHeader)
    {
        //PacketId
        base.Encode(buffer, fixedHeader);

        //ReasonCode
        var reasonCode = Enum.Parse(typeof(T), ReasonCode.ToString());
        buffer.WriteByte((byte)reasonCode);

        //Properties
        Properties.Encode(buffer);
    }

    public override void Decode(IByteBuffer buffer, ref FixedHeader fixedHeader)
    {
        //PacketId
        base.Decode(buffer, ref fixedHeader);

        //ReasonCode
        var reasonCode = buffer.ReadByte(ref fixedHeader.RemainingLength);
        ReasonCode = (T)Enum.ToObject(typeof(T), reasonCode);

        //Properties
        Properties.Decode(buffer, ref fixedHeader.RemainingLength);
    }
}
