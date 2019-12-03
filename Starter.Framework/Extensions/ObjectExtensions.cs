using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Starter.Framework.Extensions
{
    /// <summary>
    /// Extension methods to the Object type
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Compares two objects for equality, accounting for null
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool IsEqualTo(this object first, object second)
        {
            return second != null && second.Equals(first);
        }

        /// <summary>
        /// Converts an entity to JSON
        /// </summary>
        /// <param name="entity">The entity to convert</param>
        /// <param name="format">Should the JSON be formatted</param>
        /// <returns></returns>
        public static string ToJson(this object entity, bool format = false)
        {
            var jss = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            var formatting = (format) ? Formatting.Indented : Formatting.None;

            return JsonConvert.SerializeObject(entity, formatting, jss);
        }

        /// <summary>
        /// Convert an entity to a binary array of it's JSON representation
        /// </summary>
        /// <param name="entity">The entity to convert</param>
        /// <returns></returns>
        public static byte[] ToJsonBytes(this object entity)
        {
            return Encoding.UTF8.GetBytes(entity.ToJson());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<object> ToNameValueList(this Type type)
        {
            var pairs =
                Enum.GetValues(type).Cast<object>()
                    .Select(value => new
                    {
                        Name = ((Enum)value).GetDescription(),
                        Value = (int)value
                    }).ToList();

            pairs.Append(new
            {
                Name = string.Empty,
                Value = -1
            });

            return pairs.OrderBy(pair => pair.Name);
        }
    }
}