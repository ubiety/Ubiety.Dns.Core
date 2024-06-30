/*
 * Copyright 2020 Dieter Lunn
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections.Generic;

using Ubiety.Dns.Core.Common.Extensions;

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     Abstract record.
    /// </summary>
    public abstract record Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Record" /> class.
        /// </summary>
        protected Record()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Record" /> class.
        /// </summary>
        /// <param name="reader"><see cref="RecordReader" /> for the record data.</param>
        protected Record(RecordReader reader)
        {
            Reader = reader.ThrowIfNull(nameof(reader));
        }

        /// <summary>
        ///     Gets the record data.
        /// </summary>
        /// <value>Byte list of the raw record data.</value>
        public List<byte> RecordData { get; }

        /// <summary>
        ///     Gets or sets the resource record this record is a part of.
        /// </summary>
        /// <value>Resource record of the data.</value>
        public ResourceRecord ResourceRecord { get; set; }

        /// <summary>
        ///     Gets the record reader for the record.
        /// </summary>
        protected RecordReader Reader { get; }

        /// <summary>
        ///     String representation of the record.
        /// </summary>
        /// <returns>String version of the data.</returns>
        public override string ToString()
        {
            return $"{GetType().Name} is not-used";
        }
    }
}
