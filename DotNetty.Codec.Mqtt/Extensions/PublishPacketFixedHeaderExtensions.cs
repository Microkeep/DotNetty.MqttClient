namespace DotNetty.Codec.Mqtt.Packets;

internal static class PublishPacketFixedHeaderExtensions
{
    public static MqttQos GetQos(this FixedHeader fixedHeader)
    {
        return (MqttQos)((fixedHeader.Flags & 0x06) >> 1);
    }

    public static bool GetDup(this PublishPacket packet)
    {
        return (packet.FixedHeader.Flags & 0x08) == 0x08;
    }

    public static MqttQos GetQos(this PublishPacket packet)
    {
        return (MqttQos)((packet.FixedHeader.Flags & 0x06) >> 1);
    }

    public static bool GetRetain(this PublishPacket packet)
    {
        return (packet.FixedHeader.Flags & 0x01) > 0;
    }

    public static void SetQos(this PublishPacket packet, MqttQos qos)
    {
        packet.FixedHeader.Flags |= (byte)qos << 1;
    }

    public static void SetDup(this PublishPacket packet, bool dup = false)
    {
        packet.FixedHeader.Flags |= dup.ToByte() << 3;
    }

    public static void SetRetain(this PublishPacket packet, bool retain = false)
    {
        packet.FixedHeader.Flags |= retain.ToByte();
    }

    public static IDictionary<Type, PacketType> PacketTypes = new Dictionary<Type, PacketType>
    {
        { typeof(ConnectPacket), PacketType.CONNECT },
        { typeof(ConnAckPacket), PacketType.CONNACK },
        { typeof(PublishPacket), PacketType.PUBLISH },
        { typeof(PubAckPacket), PacketType.PUBACK },
        { typeof(PubRecPacket), PacketType.PUBREC },
        { typeof(PubRelPacket), PacketType.PUBREL },
        { typeof(PubCompPacket), PacketType.PUBCOMP },
        { typeof(SubscribePacket), PacketType.SUBSCRIBE },
        { typeof(SubAckPacket), PacketType.SUBACK },
        { typeof(UnsubscribePacket), PacketType.UNSUBSCRIBE },
        { typeof(UnsubAckPacket), PacketType.UNSUBACK },
        { typeof(PingReqPacket), PacketType.PINGREQ },
        { typeof(PingRespPacket), PacketType.PINGRESP },
        { typeof(DisconnectPacket), PacketType.DISCONNECT },
        { typeof(AuthPacket), PacketType.AUTH }
    };

    public static PacketType GetPacketType(this Packet packet) 
        => PacketTypes[packet.GetType()];
}
