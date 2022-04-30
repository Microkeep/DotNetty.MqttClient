namespace DotNetty.Codec.Mqtt;

[Flags]
public enum PubRelReasonCode : byte
{
    Success = 0,
    PacketIdentifierNotFound = 146
}
