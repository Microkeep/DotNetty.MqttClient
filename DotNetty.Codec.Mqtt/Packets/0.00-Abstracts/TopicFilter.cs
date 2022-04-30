using System.Text.RegularExpressions;

namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5-3.8.3.1]
/// </summary>
public sealed class TopicFilter
{
    public string TopicName { get; set; }

    public bool RetainAsPublished { get; set; }

    public bool NoLocal { get; set; }

    public MqttQos Qos { get; set; }

    public RetainHandling RetainHandling { get; set; }
}
