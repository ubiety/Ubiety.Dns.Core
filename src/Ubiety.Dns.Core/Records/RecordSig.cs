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

using System.Globalization;

namespace Ubiety.Dns.Core.Records;

/// <summary>
///     DNS signature record.
/// </summary>
public record RecordSig : Record
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="RecordSig"/> class from the specified <see cref="RecordReader"/>.
    /// </summary>
    /// <param name="reader">The <see cref="RecordReader"/> used to read the DNS signature record data.</param>
    public RecordSig(RecordReader reader)
        : base(reader)
    {
        TypeCovered = Reader.ReadUInt16();
        Algorithm = Reader.ReadByte();
        Labels = Reader.ReadByte();
        OriginalTTL = Reader.ReadUInt32();
        SignatureExpiration = Reader.ReadUInt32();
        SignatureInception = Reader.ReadUInt32();
        KeyTag = Reader.ReadUInt16();
        SignersName = Reader.ReadDomainName();
        Signature = Reader.ReadString();
    }

    /// <summary>
    ///     Gets the type of DNS record that is covered by this signature.
    /// </summary>
    public ushort TypeCovered { get; init; }

    /// <summary>
    ///     Gets the algorithm number used to generate the signature.
    /// </summary>
    public byte Algorithm { get; init; }

    /// <summary>
    ///     Gets the number of labels in the original RRSIG owner name.
    /// </summary>
    public byte Labels { get; init; }

    /// <summary>
    ///     Gets the original TTL (time to live) value of the covered record set.
    /// </summary>
    public uint OriginalTTL { get; init; }

    /// <summary>
    ///     Gets the signature expiration time as a UNIX timestamp.
    /// </summary>
    public uint SignatureExpiration { get; init; }

    /// <summary>
    ///     Gets the signature inception time as a UNIX timestamp.
    /// </summary>
    public uint SignatureInception { get; init; }

    /// <summary>
    ///     Gets the key tag value identifying the DNSKEY record that validates this signature.
    /// </summary>
    public ushort KeyTag { get; init; }

    /// <summary>
    ///     Gets the domain name of the signer that generated the signature.
    /// </summary>
    public string SignersName { get; init; }

    /// <summary>
    ///     Gets the cryptographic signature data as a string.
    /// </summary>
    public string Signature { get; init; }

    /// <summary>
    ///     Returns a string representation of the DNS signature record.
    /// </summary>
    /// <returns>A string containing the record fields in display order.</returns>
    public override string ToString() =>
        string.Format(
            CultureInfo.InvariantCulture,
            "{0} {1} {2} {3} {4} {5} {6} {7} \"{8}\"",
            TypeCovered,
            Algorithm,
            Labels,
            OriginalTTL,
            SignatureExpiration,
            SignatureInception,
            KeyTag,
            SignersName,
            Signature);
}
