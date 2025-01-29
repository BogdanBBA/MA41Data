namespace MA41Viewer.Data
{
	public struct TileInfo(TileInfo.AppTileInfo appTileInfo, TileInfo.MA41TileInfo mA41TileInfo)
	{
		public struct AppTileInfo(uint row, uint column)
		{
			public uint Row = row; // 0-15
			public uint Column = column; // 0-9

			public override readonly string ToString()
				=> $"r{Row}/c{Column}";
		}

		public struct MA41TileInfo(uint square, uint quadrant)
		{
			public uint Square = square;
			public uint Quadrant = quadrant;

			public override readonly string ToString()
				=> $"{Square}/{Quadrant}";
		}

		public AppTileInfo App = appTileInfo;
		public MA41TileInfo MA41 = mA41TileInfo;

		public override readonly string ToString()
			=> $"{App} = {MA41}";
	}
}
