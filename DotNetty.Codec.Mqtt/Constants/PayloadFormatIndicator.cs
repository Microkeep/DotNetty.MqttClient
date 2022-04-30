namespace DotNetty.Codec.Mqtt;

[Flags]
public enum PayloadFormatIndicator : byte
{
    Unspecified = 0x00,
    CharacterData = 0x01
}
