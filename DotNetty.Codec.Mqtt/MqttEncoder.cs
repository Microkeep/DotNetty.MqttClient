using DotNetty.Codec.Mqtt.Packets;
using DotNetty.Codecs;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;

namespace DotNetty.Codec.Mqtt;

public sealed class MqttEncoder : MessageToMessageEncoder<Packet>
{
    public static MqttEncoder Instance(uint maximumPacketSize) => new MqttEncoder(maximumPacketSize);

    private readonly uint _maximumPacketSize;
    public override bool IsSharable => true;

    public MqttEncoder(uint maximumPacketSize)
    {
        _maximumPacketSize = maximumPacketSize;
    }

    protected override void Encode(IChannelHandlerContext context, Packet packet, List<object> output)
    {
        var buffer = context.Allocator.Buffer();
        try
        {
            if (_maximumPacketSize > 0 && buffer.ReadableBytes > _maximumPacketSize)
                throw new EncoderException($"Maximum packet size exceeded: {buffer.ReadableBytes} > {_maximumPacketSize}");

            packet.Encode(buffer);
            output.Add(buffer);
            buffer = null;
        }
        finally
        {
            buffer?.SafeRelease();
        }
    }
}
