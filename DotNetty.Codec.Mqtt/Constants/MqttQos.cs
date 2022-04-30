namespace DotNetty.Codec.Mqtt;

[Flags]
public enum MqttQos : byte
{
    AtMostOnce = 0x00,
    AtLeastOnce = 0x01,
    ExactlyOnce = 0x02,
    Failure = 0x80
}
