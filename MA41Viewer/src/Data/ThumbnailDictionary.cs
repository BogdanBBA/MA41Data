using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

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
			TupleList = [];
		}

		public int Count
			=> TupleList.Count;

		private int IndexOfThumb(uint year, uint square, uint quadrant, uint thumbnailSize)
		{
			(uint year, uint square, uint quadrant, uint thumbnailSize) key = (year, square, quadrant, thumbnailSize);
			for (int index = 0; index < TupleList.Count; index++)
			{
				if (TupleList[index].Key == key)
					return index;
			}
			return -1;
		}

		public Image GetThumb(uint year, uint square, uint quadrant, uint thumbnailSize, Func<Image> getThumbIfItDoesntExistFunction)
		{
			int index = IndexOfThumb(year, square, quadrant, thumbnailSize);
			if (index == -1)
			{
				Image image = getThumbIfItDoesntExistFunction();
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

		/// <summary>Resize the image to the specified width and height. From https://stackoverflow.com/a/24199315/10263 </summary>
		/// <param name="image">The image to resize.</param>
		/// <param name="width">The width to resize to.</param>
		/// <param name="height">The height to resize to.</param>
		/// <returns>The resized image.</returns>
		public static Bitmap ResizeImage(Image image, int width, int height)
		{
			Rectangle destRect = new(0, 0, width, height);
			Bitmap destImage = new(width, height);

			destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			using (Graphics graphics = Graphics.FromImage(destImage))
			{
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				using (ImageAttributes wrapMode = new())
				{
					wrapMode.SetWrapMode(WrapMode.TileFlipXY);
					graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}

			return destImage;
		}
	}
}
