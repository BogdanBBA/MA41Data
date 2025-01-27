using MA41.Commons;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MA41Viewer.Data
{
	public static class ImageSizeDictionary
	{
		public static Dictionary<(uint year, uint square, uint quadrant), (uint width, uint height)> Sizes { get; private set; }

		public static void Initialize()
		{
			Sizes = [];
			if (File.Exists(Paths.IMAGE_SIZE_DICTIONARY_FILE))
			{
				string[] lines = File.ReadAllLines(Paths.IMAGE_SIZE_DICTIONARY_FILE);
				foreach (string line in lines)
				{
					uint[] values = line.Split(['=', ',']).Select(uint.Parse).ToArray();
					Sizes[(values[0], values[1], values[2])] = (values[3], values[4]);
				}
			}
		}

		public static void Save()
		{
			File.WriteAllLines(Paths.IMAGE_SIZE_DICTIONARY_FILE, Sizes.Keys.OrderBy(key => key).Select(key => $"{key.year},{key.square},{key.quadrant}={Sizes[key].width},{Sizes[key].height}").ToArray());
		}
	}
}
