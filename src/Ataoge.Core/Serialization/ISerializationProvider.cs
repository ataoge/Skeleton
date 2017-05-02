namespace Ataoge.Serialization
{
    public interface ISerializationProvider
    {
        byte[] Serialize<T>(T value);
        T DeSerialize<T>(byte[] bytes);
    }
}