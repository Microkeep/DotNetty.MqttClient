namespace DotNetty.Codec.Mqtt;

[Flags]
public enum AuthenticateReasonCode : byte
{
    Success = 0,
    ContinueAuthentication = 24,
    Reauthenticate = 25
}
