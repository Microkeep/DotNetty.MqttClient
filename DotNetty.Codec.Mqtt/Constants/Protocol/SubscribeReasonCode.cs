namespace DotNetty.Codec.Mqtt;

[Flags]
public enum SubscribeReasonCode : byte
{
    AtMostOnce = 0,
    AtLeastOnce = 0x1,
    ExactlyOnce = 0x2,
    Reserved = 0x3,
    Failure = 0x80,
    ImplementationSpecificError = 131,
    NotAuthorized = 135,
    TopicFilterInvalid = 143,
    PacketIdentifierInUse = 145,
    QuotaExceeded = 151,
    SharedSubscriptionsNotSupported = 158,
    SubscriptionIdentifiersNotSupported = 161,
    WildcardSubscriptionsNotSupported = 162
}
