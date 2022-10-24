namespace MA41.Commons
{
	public class StringWithTag<TAG_TYPE>
	{
		public TAG_TYPE Tag { get; }
		public string String { get; }
		public string Description { get; }

		public StringWithTag(TAG_TYPE tag, string @string, string description = null)
		{
			Tag = tag;
			String = @string;
			Description = description;
		}
	}
}
