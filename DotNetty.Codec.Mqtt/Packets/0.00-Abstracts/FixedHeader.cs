using DotNetty.Buffers;
using DotNetty.Codecs;

namespace DotNetty.Codec.Mqtt.Packets;

public struct FixedHeader
{
    public PacketType PacketType;

    public int Flags;

    public int RemainingLength;

    public void Encode(IByteBuffer buffer, int remainingLength = default)
    {
        if (remainingLength != default)
            RemainingLength = remainingLength;

        var headerFlags = (byte)PacketType << 4;
        headerFlags |= Flags;
        buffer.WriteByte(headerFlags);

        do
        {
            var digit = (byte)(remainingLength % 0x80);
            remainingLength /= 0x80;
            if (remainingLength > 0)
                digit |= 0x80;
            buffer.WriteByte(digit);
        } while (remainingLength > 0);
    }

    public bool Decode(IByteBuffer buffer)
    {
        var headerFlags = buffer.ReadByte();
        PacketType = (PacketType)(headerFlags >> 4);
        Flags = headerFlags & 0x0f;

        if (!TryDecodeRemainingLength(buffer, out RemainingLength) || !buffer.IsReadable(RemainingLength))
            return false;

        return true;
    }

    bool TryDecodeRemainingLength(IByteBuffer buffer, out int value)
    {
        var readable = buffer.ReadableBytes;

        var result = 0;
        var multiplier = 1;
        byte digit;
        var read = 0;
        do
        {
            if (readable < read + 1)
            {
                value = default(int);
                return false;
            }
            digit = buffer.ReadByte();
            result += (digit & 0x7f) * multiplier;
            multiplier <<= 7;
            read++;
        }
        while ((digit & 0x80) != 0 && read < 4);

        if (read == 4 && (digit & 0x80) != 0)
            throw new DecoderException("Remaining length exceeds 4 bytes in length");

        value = result;
        return true;
    }
}
