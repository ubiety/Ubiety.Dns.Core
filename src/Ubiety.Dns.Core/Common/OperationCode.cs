namespace Ubiety.Dns.Core.Common
{
	/// <summary>
	///     DNS Record OpCode.
	/// </summary>
	public enum OperationCode
	{
		/// <summary>
		///     Standard DNS Query
		/// </summary>
		Query = 0,

		/// <summary>
		///     Retired IQUERY code
		/// </summary>
		IQUERY = 1,

		/// <summary>
		///     Server status request
		/// </summary>
		Status = 2,

		/// <summary>
		///     Reserved OpCode
		/// </summary>
		RESERVED3 = 3,

		/// <summary>
		///     Notify OpCode
		/// </summary>
		Notify = 4,

		/// <summary>
		///     Update OpCode
		/// </summary>
		Update = 5,

		/// <summary>
		///     Reserved
		/// </summary>
		RESERVED6 = 6,

		/// <summary>
		///     Reserved
		/// </summary>
		RESERVED7 = 7,

		/// <summary>
		///     Reserved
		/// </summary>
		RESERVED8 = 8,

		/// <summary>
		///     Reserved
		/// </summary>
		RESERVED9 = 9,

		/// <summary>
		///     Reserved
		/// </summary>
		RESERVED10 = 10,

		/// <summary>
		///     Reserved
		/// </summary>
		RESERVED11 = 11,

		/// <summary>
		///     Reserved
		/// </summary>
		RESERVED12 = 12,

		/// <summary>
		///     Reserved
		/// </summary>
		RESERVED13 = 13,

		/// <summary>
		///     Reserved
		/// </summary>
		RESERVED14 = 14,

		/// <summary>
		///     Reserved
		/// </summary>
		RESERVED15 = 15,
	}
}