using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace MA41Viewer.Data
{
	public class WorldFileDataDictionary
	{
		public class WorldFileDataYear
		{
			public float IncrementX { get; private set; }
			public float IncrementY { get; private set; }
			public float TopLeftX { get; private set; }
			public float TopLeftY { get; private set; }
			public SizeF TileImageSize { get; set; }
			public RectangleF TileMapCoordinateBounds { get; set; }

			public WorldFileDataYear(string filePath)
				: this(File.ReadAllLines(filePath)) { }

			public WorldFileDataYear(string[] lines)
			{
				IncrementX = float.Parse(lines[0]);
				IncrementY = float.Parse(lines[3]);
				TopLeftX = float.Parse(lines[4]);
				TopLeftY = -(float.Parse(lines[5]) - 330000);
				TileImageSize = SizeF.Empty;
				TileMapCoordinateBounds = RectangleF.Empty;
			}

			public void SetImageSize((uint width, uint height) size)
			{
				TileImageSize = new SizeF(size.width, size.height);
				TileMapCoordinateBounds = new RectangleF(TopLeftX, TopLeftY, Math.Abs(IncrementX) * TileImageSize.Width, Math.Abs(IncrementY) * TileImageSize.Height);
			}

			public override string ToString()
				=> $"{TopLeftX},{TopLeftY} (+{IncrementX},+{IncrementY})";
		}

		protected Dictionary<uint, WorldFileDataYear> Years { get; private set; }

		public WorldFileDataDictionary()
		{
			Years = [];
		}

		public void Add(uint year, WorldFileDataYear data)
			=> Years.Add(year, data);

		public bool Has(uint year)
			=> Years.ContainsKey(year);

		public WorldFileDataYear Get(uint year)
			=> Years[year];

		public IEnumerable<RectangleF> Tiles
			=> Years.Values.Select(year => year.TileMapCoordinateBounds);

		public override string ToString()
			=> $"{Years.Count} years ({string.Join(",", Years.Keys)})";
	}
}
