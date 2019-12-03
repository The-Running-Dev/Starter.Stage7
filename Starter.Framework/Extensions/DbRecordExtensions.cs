using System;
using System.Data;
using System.Diagnostics;

namespace Starter.Framework.Extensions
{
    /// <summary>
    /// Extension methods to the IDataRecord type
    /// </summary>
    public static class DataRecordExtensions
    {
        /// <summary>
        /// Checks if the record contains the column name specified
        /// </summary>
        /// <param name="dataRecord">The record to check</param>
        /// <param name="columnName">The column name to look for</param>
        /// <returns></returns>
        public static bool HasColumn(this IDataRecord dataRecord, string columnName)
        {
            if (dataRecord == null)
            {
                return false;
            }

            for (var i = 0; i < dataRecord.FieldCount; i++)
            {
                if (dataRecord.GetName(i).IsEqualTo(columnName))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Creates an instance of a type based on a data record
        /// </summary>
        /// <typeparam name="T">The type to create</typeparam>
        /// <param name="record">The data record to use to populate the new type</param>
        /// <returns></returns>
        public static T Map<T>(this IDataRecord record)
        {
            var entity = Activator.CreateInstance<T>();

            foreach (var property in typeof(T).GetProperties())
            {
                if (record.HasColumn(property.Name) &&
                    !record.IsDBNull(record.GetOrdinal(property.Name)))
                {
                    property.SetValue(entity, record[property.Name]);
                }
            }

            return entity;
        }
    }
}
