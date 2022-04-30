using DotNetty.Buffers;

namespace DotNetty.Codec.Mqtt.Packets;

public abstract class VariableHeader
{
    public virtual void Encode(IByteBuffer buffer) { }

    public virtual void Encode(IByteBuffer buffer, FixedHeader fixedHeader) => Encode(buffer);

    public virtual void Decode(IByteBuffer buffer, ref FixedHeader fixedHeader) { }
}
