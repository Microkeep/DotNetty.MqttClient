namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.3]
/// </summary>
public sealed class PublishPacket : PacketWithId
{
    /// <summary>
    /// [MQTTv5.0-3.3]
    /// </summary>
    public PublishPacket()
        : this(new PublishVariableHeader(), new PublishPayload()) { }

    /// <summary>
    /// [MQTTv5.0-3.3]
    /// </summary>
    public PublishPacket(PublishVariableHeader variableHeader, PublishPayload payload)
        : base(variableHeader, payload) { }

    /// <summary>
    /// [MQTTv5.0-3.3]
    /// </summary>
    public PublishPacket(FixedHeader fixedHeader, PublishVariableHeader variableHeader, PublishPayload payload)
        : base(fixedHeader, variableHeader, payload) { }

    /// <summary>
    /// [MQTTv5.0-3.3.1.1]
    /// </summary>
    public bool Dup
    {
        get => this.GetDup();
        set => this.SetDup(value);
    }

    /// <summary>
    /// [MQTTv5.0-3.3.1.2]
    /// </summary>
    public MqttQos Qos
    {
        get => this.GetQos();
        set => this.SetQos(value);
    }

    /// <summary>
    /// [MQTTv5.0-3.3.1.3]
    /// </summary>
    public bool Retain
    {
        get => this.GetRetain();
        set => this.SetRetain(value);
    }

    /// <summary>
    /// [MQTTv5.0-3.3.2.1]
    /// </summary>
    public string TopicName
    {
        get => ((PublishVariableHeader)VariableHeader).TopicName;
        set => ((PublishVariableHeader)VariableHeader).TopicName = value;
    }

    /// <summary>
    /// [MQTTv5.0-3.3.2.2]
    /// </summary>
    ///PacketId

    /// <summary>
    /// [MQTTv5.0-3.3.2.3]
    /// </summary>
    public PublishVariableProperties Properties
    {
        get => ((PublishVariableHeader)VariableHeader).Properties;
    }
}
