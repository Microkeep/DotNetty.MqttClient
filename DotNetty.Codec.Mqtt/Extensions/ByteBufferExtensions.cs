using DotNetty.Buffers;
using DotNetty.Codecs;
using System.Runtime.CompilerServices;
using System.Text;

namespace DotNetty.Codec.Mqtt.Packets;

internal static class ByteBufferExtensions
{
    public static void WriteString(this IByteBuffer buffer, string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        buffer.WriteUnsignedShort((ushort)bytes.Length);
        buffer.WriteBytes(bytes);
    }

    public static void WriteBytesArray(this IByteBuffer buffer, byte[] value)
    {
        buffer.WriteShort(value.Length);
        if (value.Length > 0)
            buffer.WriteBytes(value);
    }

    public static void WriteAsFourByteInteger(this IByteBuffer buffer, uint value)
    {
        buffer.WriteByte((byte)(value >> 24));
        buffer.WriteByte((byte)(value >> 16));
        buffer.WriteByte((byte)(value >> 8));
        buffer.WriteByte((byte)value);
    }

    public static void WriteAsVariableByteInteger(this IByteBuffer buffer, uint value)
    {
        if (value <= 127)
        {
            buffer.WriteByte((byte)value);
            return;
        }

        if (value > 268435455) //max = 256M, 0xFF,0xFF,0xFF,0x7F
            throw new Exception($"The specified value ({value}) is too large for a variable byte integer.");

        var x = value;
        do
        {
            var @byte = x % 128;
            x /= 128;

            if (x > 0) @byte |= 128; //0x80
            buffer.WriteByte((byte)@byte);

        } while (x > 0);
    }

    public static string ReadString(this IByteBuffer buffer, ref int remainingLength)
    {
        var length = ReadUnsignedShort(buffer, ref remainingLength);
        if (length == 0)
            return string.Empty;

        DecreaseRemainingLength(ref remainingLength, length);

        var value = buffer.ToString(buffer.ReaderIndex, length, Encoding.UTF8);
        buffer.SetReaderIndex(buffer.ReaderIndex + length);

        return value;
    }

    public static byte ReadByte(this IByteBuffer buffer, ref int remainingLength)
    {
        DecreaseRemainingLength(ref remainingLength, 1);
        return buffer.ReadByte();
    }

    public static ushort ReadUnsignedShort(this IByteBuffer buffer, ref int remainingLength)
    {
        DecreaseRemainingLength(ref remainingLength, 2);
        return buffer.ReadUnsignedShort(); ;
    }

    public static uint ReadAsFourByteInteger(this IByteBuffer buffer, ref int remainingLength)
    {
        var byte0 = buffer.ReadByte();
        var byte1 = buffer.ReadByte();
        var byte2 = buffer.ReadByte();
        var byte3 = buffer.ReadByte();

        DecreaseRemainingLength(ref remainingLength, 2);

        return (uint)((byte0 << 24) | (byte1 << 16) | (byte2 << 8) | byte3);
    }

    public static uint ReadAsVariableByteInteger(this IByteBuffer buffer, ref int remainingLength)
    {
        var multiplier = 1;
        var value = 0U;
        var count = 0;
        byte encodedByte;

        do
        {
            encodedByte = buffer.ReadByte();
            value += (uint)((encodedByte & 127) * multiplier);

            if (multiplier > 2097152)
            {
                throw new Exception("Variable length integer is invalid.");
            }

            multiplier *= 128;
            count++;
        } while ((encodedByte & 128) != 0);

        DecreaseRemainingLength(ref remainingLength, count);

        return value;
    }

    public static byte[] ReadBytesArray(this IByteBuffer buffer, ref int remainingLength)
    {
        var length = ReadUnsignedShort(buffer, ref remainingLength);
        if (length == 0)
            return Array.Empty<byte>();

        DecreaseRemainingLength(ref remainingLength, length);
        return buffer.ReadBytes(length).Array;
    }


    public static byte ToByte(this bool input)
    {
        return input ? (byte)1 : (byte)0;
    }

    public static byte[] ReadSliceArray(this IByteBuffer buffer, ref int remainingLength)
    {
        IByteBuffer buf;
        if (remainingLength > 0)
        {
            buf = buffer.ReadSlice(remainingLength);
            buf.Retain();
            remainingLength = 0;
        }
        else
        {
            buf = Unpooled.Empty;
        }

        return ((Span<byte>)buf.Array).Slice(buf.ArrayOffset, buf.ReadableBytes).ToArray();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // we don't care about the method being on exception's stack so it's OK to inline
    static void DecreaseRemainingLength(ref int remainingLength, int minExpectedLength)
    {
        if (remainingLength < minExpectedLength)
            throw new DecoderException($"Current Remaining Length of {remainingLength} is smaller than expected {minExpectedLength}.");

        remainingLength -= minExpectedLength;
    }
}
