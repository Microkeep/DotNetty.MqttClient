using DotNetty.Codec.Mqtt.Packets;
using DotNetty.Codecs;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;

namespace DotNetty.Codec.Mqtt;

public sealed class MqttEncoder : MessageToMessageEncoder<Packet>
{
    private static readonly Dictionary<uint, MqttEncoder> s_instanceDict = new();
    private static readonly object s_locker = new();

    public static MqttEncoder GetInstance(uint maximumPacketSize)
    {
        if (!s_instanceDict.TryGetValue(maximumPacketSize, out MqttEncoder value))
        {
            lock (s_locker)
            {
                if (!s_instanceDict.TryGetValue(maximumPacketSize, out value))
                {
                    value = new MqttEncoder(maximumPacketSize);
                    s_instanceDict.Add(maximumPacketSize, value);
                }
            }
        }
        return value;
    }

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
