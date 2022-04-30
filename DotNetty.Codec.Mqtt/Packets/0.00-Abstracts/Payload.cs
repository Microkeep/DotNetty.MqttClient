using DotNetty.Buffers;
using DotNetty.Codecs;

namespace DotNetty.Codec.Mqtt.Packets;

public abstract class Payload
{
    public virtual void Encode(IByteBuffer buffer, VariableHeader variableHeader) { }

    public virtual void Decode(IByteBuffer buffer, VariableHeader variableHeader, ref int remainingLength) { }

    internal static void ValidateTopicFilter(string topicFilter)
    {
        if (string.IsNullOrEmpty(topicFilter))
            throw new DecoderException("All Topic Names and Topic Filters MUST be at least one character long.");

        var length = topicFilter.Length;
        for (var i = 0; i < length; i++)
        {
            var c = topicFilter[i];
            switch (c)
            {
                case '+':
                    if ((i > 0 && topicFilter[i - 1] != '/') || (i < length - 1 && topicFilter[i + 1] != '/'))
                        throw new DecoderException($"Invalid topic filter: {topicFilter}");

                    break;
                case '#':
                    if (i < length - 1 || (i > 0 && topicFilter[i - 1] != '/'))
                        throw new DecoderException($"Invalid topic filter: {topicFilter}");

                    break;
            }
        }
    }
}
