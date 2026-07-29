/*
 * Copyright © 2020-2026 Dieter (coder2000) Lunn
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

using System;

namespace Ubiety.Dns.Core.Records;

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
        ArgumentNullException.ThrowIfNull(reader);
        Reader = reader;
    }

    /// <summary>
    ///     Gets or sets the resource record this record is a part of.
    /// </summary>
    /// <value>Resource record of the data.</value>
    /// <remarks>
    ///     Assigned by <see cref="ResourceRecord" /> immediately after the record is read, so it is
    ///     always set by the time a caller can observe the record.
    /// </remarks>
    public ResourceRecord ResourceRecord { get; set; } = null!;

    /// <summary>
    ///     Gets the record reader for the record.
    /// </summary>
    /// <remarks>
    ///     Only the parameterless constructor leaves this unset, and every record in this assembly
    ///     is built through <see cref="Record(RecordReader)" />.
    /// </remarks>
    protected RecordReader Reader { get; } = null!;

    /// <summary>
    ///     String representation of the record.
    /// </summary>
    /// <returns>String version of the data.</returns>
    public override string ToString()
    {
        return $"{GetType().Name} is not-used";
    }
}
