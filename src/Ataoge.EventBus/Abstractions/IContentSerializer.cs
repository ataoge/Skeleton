using System;

namespace Ataoge.EventBus.Abstractions
{
    /// <summary>
    /// 消息内容序列化接口
    /// <para>
    /// By default, CAP will use Json as a serializer, and you can customize this interface to achieve serialization of
    /// other methods.
    /// </para>
    /// </summary>
    public interface IContentSerializer
    {
        /// <summary>
        /// Serializes the specified object to a string.
        /// </summary>
        /// <typeparam name="T"> The type of the value being serialized.</typeparam>
        /// <param name="value">The object to serialize.</param>
        /// <returns>A string representation of the object.</returns>
        string Serialize<T>(T value);

        /// <summary>
        /// Deserializes the string to the specified .NET type.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="value">The content string to deserialize.</param>
        /// <returns>The deserialized object from the string.</returns>
        T DeSerialize<T>(string value);

        /// <summary>
        /// Deserializes the string to the specified .NET type.
        /// </summary>
        /// <param name="value">The string to deserialize.</param>
        /// <param name="type">The type of the object to deserialize to.</param>
        /// <returns>The deserialized object from the string.</returns>
        object DeSerialize(string value, Type type);
    }

    
}