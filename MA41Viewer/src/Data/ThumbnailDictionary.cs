using System;
using System.Collections.Generic;
using System.Drawing;

namespace MA41Viewer.Data
{
	public class ThumbnailDictionary
	{
		public const uint DEFAULT_MINIMUM_THUMBNAIL_CAPACITY = 160; // the theoretical size of a full map
		public const uint DEFAULT_MINIMUM_FULLSIZE_CAPACITY = 4; // an 4-way intersection

		public uint DictionaryCapacity { get; private set; }

		protected List<((uint Year, uint Square, uint Quadrant, uint ThumbnailSize) Key, Image Image)> TupleList { get; private set; }

		public ThumbnailDictionary(uint capacity)
		{
			if (capacity <= 0)
				throw new ArgumentException($"The dictionary queue capacity needs to be positive (not {capacity})!");

			DictionaryCapacity = capacity;
			TupleList = new List<((uint, uint, uint, uint), Image)>();
		}

		public int Count
			=> TupleList.Count;

		private int IndexOfThumb(uint year, uint square, uint quadrant, uint thumbnailSize)
		{
			var key = (year, square, quadrant, thumbnailSize);
			for (int index = 0; index < TupleList.Count; index++)
			{
				if (TupleList[index].Key == key)
					return index;
			}
			return -1;
		}

		public Image GetThumb(uint year, uint square, uint quadrant, uint thumbnailSize, Func<Image> getThumbIfItDoesntExistFunction)
		{
			var index = IndexOfThumb(year, square, quadrant, thumbnailSize);
			if (index == -1)
			{
				var image = getThumbIfItDoesntExistFunction();
				TupleList.Insert(0, ((year, square, quadrant, thumbnailSize), image));
				index = 0;
				while (TupleList.Count > DictionaryCapacity)
				{
					TupleList[^1].Image.Dispose();
					TupleList.RemoveAt(TupleList.Count - 1);
				}
			}
			return TupleList[index].Image;
		}
	}
}
