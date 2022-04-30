namespace DotNetty.Codec.Mqtt.Packets;

public abstract class PacketWithId : Packet
{
    public PacketWithId()
        : this(new PacketIdVariableHeader()) { }

    public PacketWithId(ushort packetId)
        : this(new PacketIdVariableHeader(packetId)) { }

    protected PacketWithId(PacketIdVariableHeader variableHeader)
        : base(variableHeader) { }

    protected PacketWithId(PacketIdVariableHeader variableHeader, Payload payload)
        : base(variableHeader, payload) { }

    public PacketWithId(FixedHeader fixedHeader, PacketIdVariableHeader variableHeader, Payload payload)
        : base(fixedHeader, variableHeader, payload) { }

    public ushort PacketId
    {
        get => ((PacketIdVariableHeader)VariableHeader).PacketId;
        set => ((PacketIdVariableHeader)VariableHeader).PacketId = value;
    }
}
