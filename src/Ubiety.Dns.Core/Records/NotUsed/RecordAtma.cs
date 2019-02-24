namespace Ubiety.Dns.Core.Records.NotUsed
{
	/// <summary>
	///     Asynchronous transfer mode address resource record.
	/// </summary>
	public class RecordAtma : Record
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="RecordAtma" /> class.
		/// </summary>
		/// <param name="rr"><see cref="RecordReader" /> for the record data.</param>
		public RecordAtma(RecordReader rr)
			: base(rr)
		{
		}
	}
}