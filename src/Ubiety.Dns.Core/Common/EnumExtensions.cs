/*
 * Licensed under the MIT license
 * See the LICENSE file in the project root for more information
 */

using System;
using System.Reflection;
using Ubiety.Dns.Core.Records;

namespace Ubiety.Dns.Core.Common
{
    /// <summary>
    ///     Enumeration extension methods.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        ///     Gets the record for the record type.
        /// </summary>
        /// <param name="type">Type of record to get.</param>
        /// <param name="reader">Resource reader to create record with.</param>
        /// <param name="length">Length of the record.</param>
        /// <returns>Record.</returns>
        public static Record GetRecord(this RecordType type, RecordReader reader, int length = 0)
        {
            var fieldInfo = type.GetType().GetField(type.ToString());
            var recordAttr = fieldInfo.GetCustomAttribute<RecordAttribute>();

            if (type == RecordType.TXT)
            {
                return (Record)Activator.CreateInstance(recordAttr.RecordType, reader, length);
            }

            return (Record)Activator.CreateInstance(recordAttr.RecordType, reader);
        }
    }
}
