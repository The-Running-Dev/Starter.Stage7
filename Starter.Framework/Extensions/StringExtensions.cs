using System;

using Newtonsoft.Json;

namespace Starter.Framework.Extensions
{
    /// <summary>
    /// Extension methods to the String type
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Compares two strings for equality, ignoring case
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool IsEqualTo(this string first, string second)
        {
            return string.Compare(first, second, StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        /// <summary>
        /// Checks if string is empty, null or white space
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Checks if string is not empty, null or white space
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNotEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Converts JSON to an entity
        /// </summary>
        /// <param name="json">The JSON to convert</param>
        /// <returns></returns>
        public static T FromJson<T>(this string json)
        {
            var jss = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            return JsonConvert.DeserializeObject<T>(json, jss);
        }
    }
}