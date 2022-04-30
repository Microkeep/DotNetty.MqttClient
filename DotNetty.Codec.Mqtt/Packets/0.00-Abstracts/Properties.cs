using DotNetty.Buffers;

namespace DotNetty.Codec.Mqtt.Packets;

public abstract class Properties
{
    public IReadOnlyCollection<UserProperty> UserProperties { get; set; } = new List<UserProperty>();

    public virtual void Encode(IByteBuffer buffer)
    {
        if (UserProperties != null)
        {
            foreach (var property in UserProperties)
            {
                buffer.WriteByte((byte)PropertyId.UserProperty);
                buffer.WriteString(property.Name);
                buffer.WriteString(property.Value);
            }
        }
    }
}

public class UserProperty
{
    public UserProperty(string name, string value)
    {
        Name = name;
        Value = value;
    }
    public string Name { get; }
    public string Value { get; }
}
