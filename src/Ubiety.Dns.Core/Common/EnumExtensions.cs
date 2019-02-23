using System;
using System.Reflection;
using Ubiety.Dns.Core.Records;

namespace Ubiety.Dns.Core.Common
{
    /// <summary>
    ///     Enumeration extension methods
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        ///     Gets the record for the record type
        /// </summary>
        /// <param name="type">Type of record to get</param>
        /// <param name="reader">Resource reader to create record with</param>
        /// <returns>Record</returns>
        public static Record GetRecord(this RecordType type, RecordReader reader)
        {
            var fieldInfo = type.GetType().GetField(type.ToString());
            var recordAttr = (RecordAttribute[])fieldInfo.GetCustomAttributes(typeof(RecordAttribute));

            if (recordAttr is null)
            {
                return default;
            }

            return (Record)Activator.CreateInstance(recordAttr[0].RecordType, reader);
        }
    }
}
