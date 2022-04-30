using DotNetty.Buffers;
using DotNetty.Common.Utilities;

namespace DotNetty.Codec.Mqtt.Packets;

public abstract class Packet
{
    public FixedHeader FixedHeader;

    public VariableHeader VariableHeader;

    public Payload Payload;

    protected Packet()
    {
        FixedHeader.PacketType = this.GetPacketType();
    }

    protected Packet(VariableHeader variableHeader)
        : this(variableHeader, default) { }

    protected Packet(VariableHeader variableHeader, Payload payload)
        : this()
    {
        VariableHeader = variableHeader;
        Payload = payload;
    }

    protected Packet(FixedHeader fixedHeader, VariableHeader variableHeader, Payload payload)
        : this()
    {
        FixedHeader = fixedHeader;
        VariableHeader = variableHeader;
        Payload = payload;
    }

    public virtual void Encode(IByteBuffer buffer)
    {
        var buf = Unpooled.Buffer();
        try
        {
            VariableHeader?.Encode(buf, FixedHeader);
            Payload?.Encode(buf, VariableHeader);
            FixedHeader.Encode(buffer, buf.ReadableBytes);

            buffer.WriteBytes(buf);
        }
        finally
        {
            buf?.SafeRelease();
        }
    }

    public virtual void Decode(IByteBuffer buffer)
    {
        VariableHeader?.Decode(buffer, ref FixedHeader);
        Payload?.Decode(buffer, VariableHeader, ref FixedHeader.RemainingLength);
    }
}
