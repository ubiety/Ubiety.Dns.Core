namespace Ubiety.Dns.Core.Records.NotUsed
{
	/// <summary>
	///     SSH fingerprint record.
	/// </summary>
	public class RecordSshfp : Record
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="RecordSshfp" /> class.
		/// </summary>
		/// <param name="rr"><see cref="RecordReader" /> for the record data.</param>
		public RecordSshfp(RecordReader rr)
			: base(rr)
		{
		}
	}
}