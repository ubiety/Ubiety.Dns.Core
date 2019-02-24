namespace Ubiety.Dns.Core.Records.NotUsed
{
	/// <summary>
	///     Historic IPv6 record lookup.
	/// </summary>
	/// <remarks>
	///     > [!WARNING]
	///     > AAAA is preferred over A6
	///     # [Description](#tab/description)
	///     A6 records are defined to map a domain name to an IPv6 address.
	///     # [RFC](#tab/rfc)
	///     ```
	///     The RDATA portion of the A6 record contains two or three fields
	///     +-----------+------------------+-------------------+
	///     |Prefix len.|  Address suffix  |     Prefix name   |
	///     | (1 octet) |  (0..16 octets)  |   (0..255 octets) |
	///     +-----------+------------------+-------------------+
	///     * A prefix length, encoded as an eight-bit unsigned integer with
	///     value between 0 and 128 inclusive
	///     * An IPv6 address suffix, encoded in network order (high-order octet
	///     first). There MUST be exactly enough octets in this field to
	///     contain a number of bits equal to 128 minus prefix length, with 0
	///     to 7 leading pad bits to make this field an integral number of
	///     octets. Pad bits, if present, MUST be set to zero when loading a
	///     zone file and ignored.
	///     ```.
	/// </remarks>
	public class RecordA6 : Record
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="RecordA6" /> class.
		/// </summary>
		/// <param name="reader"><see cref="RecordReader" /> for the record data.</param>
		public RecordA6(RecordReader reader)
			: base(reader)
		{
		}
	}
}