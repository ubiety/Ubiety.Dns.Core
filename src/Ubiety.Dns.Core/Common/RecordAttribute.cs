using System;

namespace Ubiety.Dns.Core.Common
{
    /// <summary>
    ///     Record attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class RecordAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordAttribute"/> class.
        /// </summary>
        /// <param name="record">Record to use</param>
        public RecordAttribute(Type record)
        {
            RecordType = record;
        }

        /// <summary>
        ///     Gets the record type
        /// </summary>
        public Type RecordType { get; }
    }
}
