using DotNetty.Buffers;

namespace DotNetty.Codec.Mqtt.Packets;

/// <summary>
/// [MQTTv5.0-3.1.3]
/// </summary>
public class ConnectPayload : Payload
{
    /// <summary>
    /// [MQTTv5.0-3.1.3]
    /// </summary>        
    public ConnectPayload()
    {
        WillProperties = new ConnectPayloadWillProperties();
    }

    /// <summary>
    /// [MQTTv5.0-3.1.3.1]
    /// </summary>
    public string ClientId { get; set; }

    /// <summary>
    /// [MQTTv5.0-3.1.3.2]
    /// </summary>
    public ConnectPayloadWillProperties WillProperties { get; }

    /// <summary>
    /// [MQTTv5.0-3.1.3.3]
    /// </summary>
    public string WillTopic { get; set; }

    /// <summary>
    /// [MQTTv5.0-3.1.3.4]
    /// </summary>
    public byte[] WillPayload { get; set; }

    /// <summary>
    /// [MQTTv5.0-3.1.3.5]
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// [MQTTv5.0-3.1.3.6]
    /// </summary>
    public byte[] Password { get; set; }

    public override void Encode(IByteBuffer buffer, VariableHeader variableHeader)
    {
        var connectVariableHeader = (ConnectVariableHeader)variableHeader;

        //[MQTTv5.0-3.1.3.1]
        buffer.WriteString(ClientId);

        if (connectVariableHeader.ConnectFlags.WillFlag)
        {
            //[MQTTv5.0-3.1.3.2]
            WillProperties.Encode(buffer);

            //[MQTTv5.0-3.1.3.3]
            buffer.WriteString(WillTopic);

            //[MQTTv5.0-3.1.3.4]
            buffer.WriteBytesArray(WillPayload);
        }

        //[MQTTv5.0-3.1.3.5]
        if (connectVariableHeader.ConnectFlags.UserNameFlag)
            buffer.WriteString(UserName);

        //[MQTTv5.0-3.1.3.6]
        if (connectVariableHeader.ConnectFlags.PasswordFlag)
            buffer.WriteBytesArray(Password);
    }

    public override void Decode(IByteBuffer buffer, VariableHeader variableHeader, ref int remainingLength)
    {
        var connectVariableHeader = (ConnectVariableHeader)variableHeader;

        //[MQTTv5.0-3.1.3.1]            
        ClientId = buffer.ReadString(ref remainingLength);
        if (connectVariableHeader.ConnectFlags.WillFlag)
        {
            //[MQTTv5.0-3.1.3.1]
            WillProperties.Decode(buffer, ref remainingLength);

            //[MQTTv5.0-3.1.3.3]
            WillTopic = buffer.ReadString(ref remainingLength);

            //[MQTTv5.0-3.1.3.4]
            WillPayload = buffer.ReadBytesArray(ref remainingLength);
        }

        //[MQTTv5.0-3.1.3.5]
        if (connectVariableHeader.ConnectFlags.UserNameFlag)
            UserName = buffer.ReadString(ref remainingLength);

        //[MQTTv5.0-3.1.3.6]
        if (connectVariableHeader.ConnectFlags.PasswordFlag)
            Password = buffer.ReadBytesArray(ref remainingLength);
    }
}
