namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.1.2.3]
/// </summary>
public struct ConnectFlags
{
    /// <summary>
    /// [MQTTv5.0-3.1.2.4]
    /// </summary>
    public bool CleanStart;

    /// <summary>
    /// [MQTTv5.0-3.1.2.5]
    /// </summary>
    public bool WillFlag;

    /// <summary>
    /// [MQTTv5.0-3.1.2.6]
    /// </summary>
    public MqttQos WillQos;

    /// <summary>
    /// [MQTTv5.0-3.1.2.7]
    /// </summary>
    public bool WillRetain;        

    /// <summary>
    /// [MQTTv5.0-3.1.2.8]
    /// </summary>
    public bool UserNameFlag;

    /// <summary>
    /// [MQTTv5.0-3.1.2.9]
    /// </summary>
    public bool PasswordFlag;
}
