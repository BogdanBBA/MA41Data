using MA41Viewer.Data;
using Commons;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Text;

namespace MA41Viewer.UI.Controls
{
	public partial class MapViewer
	{
		public struct DebugInfoWrapper
		{
			public bool KaboomExplosionNotOk;
			public int VisibleTileCount;
			public int YearTileCount;
			public int OldThumbnailCount;
			public int OldFullsizeCount;
			public HashSet<Size> ThumbnailSizes;
			public int LoadedThumbnailCount;
			public int LoadedFullsizeCount;
			public long DrawingDurationMs;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			const bool forceDebug = false;
			lock (_lock)
			{
				DebugInfoWrapper debugInfo = new() { ThumbnailSizes = [] };
				Stopwatch stopwatch = Stopwatch.StartNew();
				Graphics g = e.Graphics;
				switch (Sett.DrawingState)
				{
					case MapSettings.MapDrawingState.AtRest:
						{
							Bitmap bmp = GenerateBitmap(out debugInfo);
							Sett.LastFrame = new KeyValuePair<RectangleF, Bitmap>(Sett.CurrentMapCoordBounds, bmp);
							g.DrawImageUnscaled(bmp, Point.Empty);
						}
						break;
					case MapSettings.MapDrawingState.MouseMovement:
						{
							if (!Sett.LastFrame.HasValue)
							{
								DrawDebugText(g, "There is no 'last frame' available", 10, 10, true);
								break;
							}
							Point location = new((int)(Sett.MouseLocationPx.X - Sett.LastMouseDown_MapviewLocationPx.X), (int)(Sett.MouseLocationPx.Y - Sett.LastMouseDown_MapviewLocationPx.Y));
							g.DrawImageUnscaled(Sett.LastFrame.Value.Value, location);
						}
						break;
					case MapSettings.MapDrawingState.ZoomEvent:
						{
							if (!Sett.LastFrame.HasValue)
							{
								DrawDebugText(g, "There is no 'last frame' available", 10, 10, true);
								break;
							}
							RectangleF mapviewBounds = Sett.Translate_MapCoordinateBounds_To_MapviewLocationBoundsPx(Sett.LastFrame.Value.Key);
							g.DrawImage(Sett.LastFrame.Value.Value, mapviewBounds);
							DrawDebugText(g, $"current map coords:    {Sett.CurrentMapCoordBounds.Left:F0}, {Sett.CurrentMapCoordBounds.Top:F0} / {Sett.CurrentMapCoordBounds.Width:F0} x {Sett.CurrentMapCoordBounds.Height:F0}", 10, -65, forceDebug);
							DrawDebugText(g, $"last frame map coords: {Sett.LastFrame?.Key.Left:F0}, {Sett.LastFrame?.Key.Top:F0} / {Sett.LastFrame?.Key.Width:F0} x {Sett.LastFrame?.Key.Height:F0}", 10, -50, forceDebug);
							DrawDebugText(g, $"last frame bounds px:  {mapviewBounds.Left:F0}, {mapviewBounds.Top:F0} / {mapviewBounds.Width:F0} x {mapviewBounds.Height:F0}", 10, -35, forceDebug);
						}
						break;
					default:
						{
							DrawDebugText(g, $"Unknown drawing state: {Sett.DrawingState}", 10, 10, true);
						}
						break;
				}
				stopwatch.Stop();
				debugInfo.DrawingDurationMs = stopwatch.ElapsedMilliseconds;
				if (!debugInfo.KaboomExplosionNotOk)
					DrawDebugInformation(g, debugInfo, forceDebug);
				Sett.LastDrawingTime = DateTime.Now;
			}
		}

		protected void DrawDebugText(Graphics g, string text, float relX, float relY = 10.0f, bool forceDebug = false)
		{
			if (!DebugMode && !forceDebug) return;
			Font font = new("Ubuntu Mono", 10f, FontStyle.Bold);
			SizeF size = g.MeasureString(text, font);
			float x = relX >= 0 ? relX : Width + relX - size.Width;
			float y = relY >= 0 ? relY : Height + relY - size.Height;
			g.FillRectangle(BACKBRUSH, new RectangleF(new PointF(x, y), size));
			g.DrawString(text, font, Brushes.Black, x, y);
		}

		protected void DrawSquareInfo(Graphics g, RectangleF rect, uint year, (TileInfo tileInfo, WorldFileDataDictionary.WorldFileDataYear wfdy) info, Size imageSize, bool detailedInfo, bool forceDebug = false)
		{
			if (!DebugMode && !forceDebug) return;
			string text = $"Year {year}{Environment.NewLine}Sq/Q {info.tileInfo.MA41.Square}/{info.tileInfo.MA41.Quadrant}{Environment.NewLine}Tile {info.tileInfo.App.Row},{info.tileInfo.App.Column}";
			Font font = new("Ubuntu Mono", 12.0f, FontStyle.Bold);
			SizeF size = g.MeasureString(text, font);
			PointF location = new(rect.Left + rect.Width / 2 - size.Width / 2, rect.Top + rect.Height / 2 - size.Height / 2);
			g.FillRectangle(BACKBRUSH, location.X, location.Y, size.Width, size.Height);
			g.DrawString(text, font, Brushes.Purple, location);
			if (detailedInfo)
			{
				text = new StringBuilder()
					.AppendLine($"  Tile zoom: {rect.Width / imageSize.Width:P1}")
					.AppendLine($" Image size: {imageSize.Width}px x {imageSize.Height}px")
					.AppendLine($"          - Mapview -")
					.AppendLine($"   Location: {rect.Left:F2}px, {rect.Top:F2}px")
					.AppendLine($"       Size: {rect.Width:F2}px x {rect.Height:F2}px")
					.AppendLine($"         - MapCoords -")
					.AppendLine($"   Location: {info.wfdy.TopLeftX:F0}m, {info.wfdy.TopLeftY:F0}m")
					.AppendLine($"       Size: {info.wfdy.TileMapCoordinateBounds.Width:F0}m x {info.wfdy.TileMapCoordinateBounds.Height:F0}m")
					.ToString();
				Font newFont = new("Ubuntu Mono", 10, FontStyle.Bold);
				SizeF newSize = g.MeasureString(text, newFont);
				PointF newLocation = new(rect.Left + rect.Width / 2 - newSize.Width / 2, location.Y + size.Height);
				g.FillRectangle(BACKBRUSH, newLocation.X, newLocation.Y, newSize.Width, newSize.Height);
				g.DrawString(text, newFont, Brushes.Black, newLocation);
			}
		}

		private void DrawDebugInformation(Graphics g, DebugInfoWrapper debugInfo, bool forceDebug)
		{
			int edgePx = 10, y = -5;
			if (DebugMode || forceDebug)
			{
				DrawDebugText(g, $"Now: {DateTime.Now:yyyy/MM/dd HH:mm:ss.fff}", edgePx, y += 15, forceDebug);
				DrawDebugText(g, $"Year: {Sett.Year} (tiles: {debugInfo.VisibleTileCount} visible / {debugInfo.YearTileCount} year's / {GeoModel.Tiles.Length} total)", edgePx, y += 15, forceDebug);
				DrawDebugText(g, $"Zoom: ratio {MapSettings.ZOOMS[Sett.ZoomLevel]}x, level {Sett.ZoomLevel} (range 0..{MapSettings.ZOOMS.Length - 1})", edgePx, y += 15, forceDebug);
				DrawDebugText(g, $"Map coordinates: XL,YT={Sett.CurrentMapCoordBounds.X:F2},{Sett.CurrentMapCoordBounds.Y:F2}; XR,YB={Sett.CurrentMapCoordBounds.Right:F2},{Sett.CurrentMapCoordBounds.Bottom:F2}; W,H={Sett.CurrentMapCoordBounds.Width:F2},{Sett.CurrentMapCoordBounds.Height:F2}", edgePx, y += 15, forceDebug);
				if (Sett.CurrentDebugInfoShown.MouseCursorInfo)
				{
					DrawDebugText(g, Sett.MouseLocationPx == PointF.Empty ? "Mouse out of bounds" : $"Mouse: mapview location {Sett.MouseLocationPx.X:F0}px, {Sett.MouseLocationPx.Y:F0}px / map coordinates {Sett.Translate_MapviewX_To_MapCoordinateX(Sett.MouseLocationPx.X):F1}, {Sett.Translate_MapviewY_To_MapCoordinateY(Sett.MouseLocationPx.Y):F1}", edgePx, y += 15, forceDebug);
					DrawDebugText(g, Sett.LastMouseDown_MapviewLocationPx == PointF.Empty ? "Mouse not down" : $"Mouse was down at {Sett.LastMouseDown_MapviewLocationPx.X:F1}, {Sett.LastMouseDown_MapviewLocationPx.Y:F1} (delta {Sett.LastMouseDown_MapviewLocationPx.X - Sett.MouseLocationPx.X:F1}, {Sett.LastMouseDown_MapviewLocationPx.Y - Sett.MouseLocationPx.Y:F1})", edgePx, y += 15, forceDebug);
				}
				if (Sett.CurrentDebugInfoShown.DrawingQualityInfo)
				{
					DrawDebugText(g, $"CompositingMode={g.CompositingMode}, CompositingQuality={g.CompositingQuality}, InterpolationMode={g.InterpolationMode}, PixelOffsetMode={g.PixelOffsetMode}, SmoothingMode={g.SmoothingMode}, TextRenderingHint={g.TextRenderingHint}, ", edgePx, y += 15, forceDebug);
				}
				if (Sett.CurrentDebugInfoShown.MemoryAllocationInfo)
				{
					DrawDebugText(g, $"Memory usage: total allocated {GC.GetTotalAllocatedBytes() / 1048576f:F2}MB, total {GC.GetTotalMemory(false) / 1048576f:F2}MB, current thread {GC.GetAllocatedBytesForCurrentThread() / 1048576f:F2}MB", edgePx, y += 15, forceDebug);
					DrawDebugText(g, $"Image dictionaries (count/capacity; +new): thumbnails {ThumbDictionary.Count}/{ThumbDictionary.DictionaryCapacity} (+{ThumbDictionary.Count - debugInfo.OldThumbnailCount}) / full-size {FullSizeTileDictionary.Count}/{FullSizeTileDictionary.DictionaryCapacity} (+{FullSizeTileDictionary.Count - debugInfo.OldFullsizeCount}) | sizes: {string.Join(", ", debugInfo.ThumbnailSizes.OrderByDescending(size => size.Width * size.Height).ThenByDescending(size => size.Width).Select(size => $"{size.Width}x{size.Height}"))}", edgePx, y += 15, forceDebug);
					DrawDebugText(g, $"Tiles loaded from file: thumbnails {debugInfo.LoadedThumbnailCount}, full-size {debugInfo.LoadedFullsizeCount})", edgePx, y += 15, forceDebug);
				}
			}
			DrawDebugText(g, $"Drawing took {debugInfo.DrawingDurationMs} ms", edgePx, -edgePx, true);
			DrawDebugText(g, $"Drawing state: {Sett.DrawingState}", -edgePx, -edgePx, true);
		}

		private static void DrawYearOnExportBitmap(Bitmap frame, uint year)
		{
			using (Graphics g = Graphics.FromImage(frame))
			{
				int tenthOfSmallerSide = Math.Min(frame.Width, frame.Height) / 10;
				RectangleF yearArea = new(6, 6, 2 * tenthOfSmallerSide, tenthOfSmallerSide);
				string text = $"{year}";
				Font font = g.GetAdjustedFont(text, new Font("Ubuntu Mono", 10, FontStyle.Bold), yearArea.Width, 100, 6);
				SizeF fontSize = g.MeasureString(text, font);
				g.TextRenderingHint = TextRenderingHint.AntiAlias;
				g.FillRectangle(Brushes.Black, yearArea);
				g.DrawString(text, font, Brushes.White, yearArea.GetXForCenteredText(fontSize.Width), yearArea.GetYForCenteredText(fontSize.Height));
			}
		}

		public Bitmap GenerateAllYearsBitmap(uint[] years)
		{
			years = years.Where(year => year < 2019 || year > 2020).ToArray(); // meh
			bool vertical = Size.Width >= Size.Height;
			Size size = vertical ? new Size(Size.Width, years.Length * Size.Height) : new Size(years.Length * Size.Width, Size.Height);
			int addX = vertical ? 0 : Size.Width;
			int addY = vertical ? Size.Height : 0;
			Point location = Point.Empty;
			Bitmap result = new(size.Width, size.Height);
			using (Graphics g = Graphics.FromImage(result))
			{
				foreach (uint year in years)
				{
					Sett.Year = year;
					Bitmap frame = GenerateBitmap(out DebugInfoWrapper _);
					DrawYearOnExportBitmap(frame, year);
					g.DrawImageUnscaled(frame, location);
					location.Offset(addX, addY);
				}
			}
			return result;
		}

		public Bitmap GenerateBitmap(out DebugInfoWrapper oDebugInfo)
		{
			DebugInfoWrapper debugInfo = new() { KaboomExplosionNotOk = true, ThumbnailSizes = [] };
			Bitmap result = new(Size.Width, Size.Height);
			using (Graphics g = Graphics.FromImage(result))
			{
				g.CompositingMode = Sett.CurrentQualitySettings.CompositingModeValue;
				g.CompositingQuality = Sett.CurrentQualitySettings.CompositingQualityValue;
				g.InterpolationMode = Sett.CurrentQualitySettings.InterpolationModeValue;
				g.PixelOffsetMode = Sett.CurrentQualitySettings.PixelOffsetModeValue;
				g.SmoothingMode = Sett.CurrentQualitySettings.SmoothingModeValue;
				g.TextRenderingHint = Sett.CurrentQualitySettings.TextRenderingHintValue;

				if (GeoModel == null)
				{
					g.Clear(Color.LightGray);
					DrawDebugText(g, "not initialized", 10, 10, true);
					oDebugInfo = debugInfo;
					return result;
				}

				if (Sett.Year == null)
				{
					g.Clear(Color.LightGray);
					DrawDebugText(g, "incorrect settings", 10, 10, true);
					oDebugInfo = debugInfo;
					return result;
				}

				g.Clear(Color.WhiteSmoke);

				(TileInfo tileInfo, WorldFileDataDictionary.WorldFileDataYear wfdd)[] yearTileWFDY = GeoModel.GetByYear(Sett.Year.Value);
				debugInfo.YearTileCount = yearTileWFDY.Length;

				(TileInfo tileInfo, WorldFileDataDictionary.WorldFileDataYear wfdd)[] visibleTileWFDY = yearTileWFDY.Where(wfdd => wfdd.wfdd.TileMapCoordinateBounds.IntersectsWith(Sett.CurrentMapCoordBounds)).ToArray();
				debugInfo.VisibleTileCount = visibleTileWFDY.Length;

				debugInfo.OldThumbnailCount = ThumbDictionary.Count;
				debugInfo.OldFullsizeCount = FullSizeTileDictionary.Count;

				foreach ((TileInfo tileInfo, WorldFileDataDictionary.WorldFileDataYear wfdd) info in visibleTileWFDY)
				{
					RectangleF mouseTile = Sett.Translate_MapCoordinateBounds_To_MapviewLocationBoundsPx(info.wfdd.TileMapCoordinateBounds);
					Image image;

					// extend destination rectangle for tile outward, to avoid line artefacts at edges of tile image caused by floating-point resizing in g.DrawImage()
					mouseTile = new RectangleF(mouseTile.X - 1, mouseTile.Y - 1, mouseTile.Width + 2, mouseTile.Height + 2);

					if (MapSettings.TryGetRecommendedThumbnailSize(Math.Max(mouseTile.Width, mouseTile.Height), Sett.CurrentQualitySettings.ThumbnailQualityLevel, out uint recommendedThumbnailSize))
					{
						image = ThumbDictionary.GetThumb(Sett.Year.Value, info.tileInfo.MA41.Square, info.tileInfo.MA41.Quadrant, recommendedThumbnailSize, () =>
						{
							debugInfo.LoadedThumbnailCount++;
							string thumbnailPath = GeoData.GetJpgThumbnailPath(Sett.Year.Value, info.tileInfo.MA41.Square, info.tileInfo.MA41.Quadrant, recommendedThumbnailSize);
							return Image.FromFile(thumbnailPath);
						});
					}
					else
					{
						// TODO (maybe?): image must be added to (and retrieved from) dictionary resized, not full-size
						image = FullSizeTileDictionary.GetThumb(Sett.Year.Value, info.tileInfo.MA41.Square, info.tileInfo.MA41.Quadrant, uint.MinValue, () =>
						{
							debugInfo.LoadedFullsizeCount++;
							string path = GeoData.GetJpgPath(Sett.Year.Value, info.tileInfo.MA41.Square, info.tileInfo.MA41.Quadrant);
							return Image.FromFile(path);
						});
					}
					debugInfo.ThumbnailSizes.Add(image.Size);

					g.DrawImage(image, mouseTile);

					if (DebugMode)
					{
						g.DrawRectangle(Pens.Purple, mouseTile.Left, mouseTile.Top, mouseTile.Width, mouseTile.Height);
						DrawSquareInfo(g, mouseTile, Sett.Year.Value, info, image.Size, Sett.CurrentDebugInfoShown.DetailedTileInfo);
					}
				}

				debugInfo.KaboomExplosionNotOk = false;
				oDebugInfo = debugInfo;
				return result;
			}
		}
	}
}
