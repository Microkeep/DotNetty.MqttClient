namespace DotNetty.Codec.Mqtt;

public enum AuthenticateReasonCode : byte
{
    Success = 0,
    ContinueAuthentication = 24,
    Reauthenticate = 25
}
