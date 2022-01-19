namespace MA41Viewer.Data
{
	public struct TileInfo
	{
		public struct AppTileInfo
		{
			public uint Row; // 0-15
			public uint Column; // 0-9

			public AppTileInfo(uint row, uint column)
			{
				Row = row;
				Column = column;
			}

			public override string ToString()
				=> $"r{Row}/c{Column}";
		}

		public struct MA41TileInfo
		{
			public uint Square;
			public uint Quadrant;

			public MA41TileInfo(uint square, uint quadrant)
			{
				Square = square;
				Quadrant = quadrant;
			}

			public override string ToString()
				=> $"{Square}/{Quadrant}";
		}

		public AppTileInfo App;
		public MA41TileInfo MA41;

		public TileInfo(AppTileInfo appTileInfo, MA41TileInfo mA41TileInfo)
		{
			App = appTileInfo;
			MA41 = mA41TileInfo;
		}

		public override string ToString()
			=> $"{App} = {MA41}";
	}
}
