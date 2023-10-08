namespace DotNetty.Codec.Mqtt;

public enum RetainHandling : byte
{
    SendAtSubscribe = 0,
    SendAtSubscribeIfNewSubscriptionOnly = 1,
    DoNotSendOnSubscribe = 2
}
