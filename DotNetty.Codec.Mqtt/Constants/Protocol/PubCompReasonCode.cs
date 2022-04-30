namespace DotNetty.Codec.Mqtt;

[Flags]
public enum PubCompReasonCode : byte
{
    Success = 0,
    PacketIdentifierNotFound = 146
}