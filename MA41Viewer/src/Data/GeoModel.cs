using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MA41Viewer.Data
{
	public class GeoModel
	{
		protected Dictionary<TileInfo, WorldFileDataDictionary> WorldFileData { get; private set; }

		public GeoModel()
		{
			WorldFileData = new Dictionary<TileInfo, WorldFileDataDictionary>();

			// static population (necessary laying out), with empty data
			uint maRow = 1, maCol = 2;
			uint appCol = 0, appRow = 0;
			while (WorldFileData.Count < 160) // 16 * 10
			{
				for (uint square = maRow * 10 + maCol, quadrant = 1; quadrant <= 4; quadrant++)
				{
					var tileInfo = new TileInfo(new TileInfo.AppTileInfo(appRow, appCol), new TileInfo.MA41TileInfo(square, quadrant));
					WorldFileData.Add(tileInfo, new WorldFileDataDictionary());

					switch (quadrant)
					{
						case 1:
							appCol++;
							break;
						case 2:
							appCol--;
							appRow++;
							break;
						case 3:
							appCol++;
							break;
						case 4:
							appCol++;
							appRow--;
							break;
						default:
							throw new ApplicationException($"Unexpected case");
					}
				}

				maCol++;
				if (maCol > 9)
				{
					maCol = 2;
					maRow++;
					appCol = 0;
				}
			}

			var s = string.Join(Environment.NewLine, WorldFileData.Keys.Select(x => $"[{x.App.Row:D2}, {x.App.Column:D2}] = {x.MA41.Square}/{x.MA41.Quadrant}"));
		}

		public (TileInfo tileInfo, WorldFileDataDictionary.WorldFileDataYear wfdd)[] GetByYear(uint year)
			=> WorldFileData.Where(pair => pair.Value.Has(year)).Select(pair => (pair.Key, pair.Value.Get(year))).ToArray();

		public WorldFileDataDictionary GetByTileInfo(TileInfo tileInfo)
			=> WorldFileData[tileInfo];

		public WorldFileDataDictionary GetByAppCoordinates(uint row, uint column)
			=> WorldFileData[WorldFileData.Keys.First(tileInfo => tileInfo.App.Row == row && tileInfo.App.Column == column)];

		public WorldFileDataDictionary GetByMA41Coordinates(uint square, uint quadrant)
			=> WorldFileData[WorldFileData.Keys.First(tileInfo => tileInfo.MA41.Square == square && tileInfo.MA41.Quadrant == quadrant)];

		public uint[] Years { get; private set; }

		public RectangleF[] Tiles { get; private set; }

		public PointF CenterCoordinate { get; private set; }

		public void DoPostProcessing(uint[] years)
		{
			Years = years;
			Tiles = WorldFileData.Values.SelectMany(wfdd => wfdd.Tiles).ToArray();
			CenterCoordinate = new PointF(Tiles.Average(tile => tile.Left + tile.Width / 2.0f), Tiles.Average(tile => tile.Top + tile.Height / 2.0f));
		}
	}
}
