using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Codec.Mqtt.Packets;
using DotNetty.Transport.Channels;
using System.ComponentModel;

namespace DotNetty.Codec.Mqtt;

public sealed class MqttDecoder : ReplayingDecoder<MqttDecoder.ParseState>
{
    private readonly bool _isServer;
    private readonly uint _maximumPacketSize;

    public enum ParseState { Ready, Failed }

    public MqttDecoder(bool isServer, uint maximumPacketSize)
        : base(ParseState.Ready)
    {
        _isServer = isServer;
        _maximumPacketSize = maximumPacketSize;
    }

    protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
    {
        try
        {
            switch (State)
            {
                case ParseState.Ready:
                    if (!TryDecodePacket(context, input, out Packet packet))
                    {
                        RequestReplay();
                        return;
                    }

                    output.Add(packet);
                    Checkpoint();
                    break;
                case ParseState.Failed:
                    // read out data until connection is closed
                    input.SkipBytes(input.ReadableBytes);
                    return;
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
        catch (DecoderException ex)
        {
            Console.WriteLine(ex);
            input.SkipBytes(input.ReadableBytes);
            Checkpoint(ParseState.Failed);
            throw;
        }
    }

    private bool TryDecodePacket(IChannelHandlerContext context, IByteBuffer buffer, out Packet packet)
    {
        try
        {
            if (!buffer.IsReadable(2))
            {
                packet = default;
                return false;
            }

            var fixedHeader = default(FixedHeader);
            if (!fixedHeader.Decode(buffer))
            {
                packet = null;
                return false;
            }

            if (_maximumPacketSize > 0 && fixedHeader.RemainingLength + 2 > _maximumPacketSize) // 2 = Header Flags
            {
                packet = null;
                throw new DecoderException($"Maximum packet size exceeded: {fixedHeader.RemainingLength + 2} > {_maximumPacketSize}");
            }

            switch (fixedHeader.PacketType)
            {
                case PacketType.PINGREQ:
                case PacketType.SUBSCRIBE:
                case PacketType.UNSUBSCRIBE:
                case PacketType.CONNECT:
                case PacketType.DISCONNECT:
                    ValidateServerPacketExpected(fixedHeader.PacketType);
                    break;
                case PacketType.CONNACK:
                case PacketType.SUBACK:
                case PacketType.UNSUBACK:
                case PacketType.PINGRESP:
                    ValidateClientPacketExpected(fixedHeader.PacketType);
                    break;
            }

            packet = fixedHeader.PacketType switch
            {
                PacketType.CONNECT => new ConnectPacket(),
                PacketType.CONNACK => new ConnAckPacket(),
                PacketType.DISCONNECT => new DisconnectPacket(),
                PacketType.PINGREQ => new PingReqPacket(),
                PacketType.PINGRESP => new PingRespPacket(),
                PacketType.PUBACK => new PubAckPacket(),
                PacketType.PUBCOMP => new PubCompPacket(),
                PacketType.PUBLISH => new PublishPacket(),
                PacketType.PUBREC => new PubRecPacket(),
                PacketType.PUBREL => new PubRelPacket(),
                PacketType.SUBSCRIBE => new SubscribePacket(),
                PacketType.SUBACK => new SubAckPacket(),
                PacketType.UNSUBSCRIBE => new UnsubscribePacket(),
                PacketType.UNSUBACK => new UnsubscribePacket(),
                PacketType.AUTH => new AuthPacket(),
                _ => throw new DecoderException("Unsupported Message Type"),
            };
            packet.FixedHeader = fixedHeader;
            packet.Decode(buffer);

            return true;
        }
        catch (DecoderException ex)
        {
            Console.WriteLine(ex);
            packet = default;
            return false;
        }
    }

    void ValidateServerPacketExpected(PacketType packetType)
    {
        if (!_isServer)
            throw new DecoderException($"Expected server packet, but received {packetType.ToString()}");
    }

    void ValidateClientPacketExpected(PacketType packetType)
    {
        if (_isServer)
            throw new DecoderException($"Expected client packet, but received {packetType.ToString()}");
    }
}
