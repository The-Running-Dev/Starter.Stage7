using System;
using System.ComponentModel;

namespace Starter.Framework.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Get the description of an Enum
        /// </summary>
        /// <param name="value">The Enum value</param>
        /// <returns>The string value of the Enum</returns>
        public static string GetDescription(this Enum value)
        {
            // Get the type
            var type = value.GetType();

            // Get field info for this type
            var fi = type.GetField(value.ToString());

            // Get the description attributes
            var attributes = new DescriptionAttribute[0];

            if (fi != null && fi.IsDefined(typeof(DescriptionAttribute), true))
            {
                attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            }

            // Return the first if there was a match
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}