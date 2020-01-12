/*
 * Licensed under the MIT license
 * See the LICENSE file in the project root for more information
 */

using System;

namespace Ubiety.Dns.Core.Common
{
    /// <summary>
    ///     Applies a record class type to the enum type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class RecordAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordAttribute"/> class.
        /// </summary>
        /// <param name="record">Record to use.</param>
        public RecordAttribute(Type record)
        {
            RecordType = record;
        }

        /// <summary>
        ///     Gets the record type.
        /// </summary>
        public Type RecordType { get; }
    }
}
