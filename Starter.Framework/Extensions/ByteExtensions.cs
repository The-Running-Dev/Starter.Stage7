using System.Text;

namespace Starter.Framework.Extensions
{
    /// <summary>
    /// Extension methods to the Byte type
    /// </summary>
    public static class ByteExtensions
    {
        /// <summary>
        /// Converts a byte array of a JSON string to an entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonBytes">The JSON bytes representing the entity</param>
        /// <returns></returns>
        public static T FromJsonBytes<T>(this byte[] jsonBytes)
        {
            return Encoding.UTF8.GetString(jsonBytes).FromJson<T>();
        }
    }
}