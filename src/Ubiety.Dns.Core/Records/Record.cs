namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     Abstract record
    /// </summary>
    public abstract class Record
    {
        /// <summary>
        ///     Gets or sets the resource record this record is a part of
        /// </summary>
        public ResourceRecord ResourceRecord { get; set; }
    }
}
