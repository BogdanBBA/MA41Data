using MA41Viewer.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MA41Viewer.UI.Controls
{
	public partial class MapViewer
	{
		protected const bool FORCE_DEBUG = false;

		protected void DrawDebugText(Graphics g, string text, float x, float y = 10.0f, bool forceDebug = false)
		{
			if (!DebugMode && !forceDebug) return;
			var font = new Font("Ubuntu Mono", 10f, FontStyle.Bold);
			g.FillRectangle(BACKBRUSH, new RectangleF(new PointF(x, y), g.MeasureString(text, font)));
			g.DrawString(text, font, Brushes.Black, x, y);
		}

		protected void DrawSquareInfo(Graphics g, RectangleF rect, uint year, (TileInfo tileInfo, WorldFileDataDictionary.WorldFileDataYear wfdy) info, Size imageSize, bool ignoreDebugMode = false)
		{
			if (!DebugMode && !ignoreDebugMode) return;
			var text = $"Year {year}{Environment.NewLine}Sq/Q {info.tileInfo.MA41.Square}/{info.tileInfo.MA41.Quadrant}{Environment.NewLine}Tile {info.tileInfo.App.Row},{info.tileInfo.App.Column}";
			var font = new Font("Ubuntu Mono", 12.0f, FontStyle.Bold);
			var size = g.MeasureString(text, font);
			var location = new PointF(rect.Left + rect.Width / 2 - size.Width / 2, rect.Top + rect.Height / 2 - size.Height / 2);
			g.FillRectangle(BACKBRUSH, location.X, location.Y, size.Width, size.Height);
			g.DrawString(text, font, Brushes.Purple, location);
			return;
			text = new StringBuilder()
				.AppendLine($"  Tile zoom: {rect.Width / imageSize.Width:P1}")
				.AppendLine($" Image size: {imageSize.Width}px x {imageSize.Height}px")
				.AppendLine($"        - Mapview -")
				.AppendLine($"   Location: {rect.Left:F2}px, {rect.Top:F2}px")
				.AppendLine($"       Size: {rect.Width:F2}px x {rect.Height:F2}px")
				.AppendLine($"       - MapCoords -")
				.AppendLine($"   Location: {info.wfdy.TopLeftX:F0}m, {info.wfdy.TopLeftY:F0}m")
				.AppendLine($"       Size: {info.wfdy.TileMapCoordinateBounds.Width:F0}m x {info.wfdy.TileMapCoordinateBounds.Height:F0}m")
				.ToString();
			var newFont = new Font("Ubuntu Mono", 10, FontStyle.Bold);
			var newSize = g.MeasureString(text, newFont);
			var newLocation = new PointF(rect.Left + rect.Width / 2 - newSize.Width / 2, location.Y + size.Height);
			g.FillRectangle(BACKBRUSH, newLocation.X, newLocation.Y, newSize.Width, newSize.Height);
			g.DrawString(text, newFont, Brushes.Black, newLocation);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			lock (_lock)
			{
				var bmp = GenerateBitmap();
				var g = e.Graphics;
				g.DrawImageUnscaled(bmp, Point.Empty);
			}
		}

		public Bitmap GenerateAllYearsBitmap(uint[] years)
		{
			years = years.Where(year => year < 2019 || year > 2020).ToArray(); // meh
			var vertical = Size.Width >= Size.Height;
			var size = vertical ? new Size(Size.Width, years.Length * Size.Height) : new Size(years.Length * Size.Width, Size.Height);
			var addX = vertical ? 0 : Size.Width;
			var addY = vertical ? Size.Height : 0;
			var location = Point.Empty;
			var result = new Bitmap(size.Width, size.Height);
			using (var g = Graphics.FromImage(result))
			{
				foreach (var year in years)
				{
					Sett.Year = year;
					var frame = GenerateBitmap();
					g.DrawImageUnscaled(frame, location);
					location.Offset(addX, addY);
				}
			}
			return result;
		}

		private Bitmap GenerateBitmap()
		{
			var result = new Bitmap(Size.Width, Size.Height);
			using (var g = Graphics.FromImage(result))
			{
				var stopwatch = Stopwatch.StartNew();
				var loadedThumbs = 0u;
				var loadedFullSize = 0u;
				var oldThumbCount = ThumbDictionary.Count;
				var oldFullsizeCount = FullSizeTileDictionary.Count;
				var thumbnailSizes = new HashSet<Size>();

				g.CompositingMode = Sett.CurrentQualitySettings.CompositingModeValue;
				g.CompositingQuality = Sett.CurrentQualitySettings.CompositingQualityValue;
				g.InterpolationMode = Sett.CurrentQualitySettings.InterpolationModeValue;
				g.PixelOffsetMode = Sett.CurrentQualitySettings.PixelOffsetModeValue;
				g.SmoothingMode = Sett.CurrentQualitySettings.SmoothingModeValue;
				g.TextRenderingHint = Sett.CurrentQualitySettings.TextRenderingHintValue;

				if (GeoModel == null)
				{
					g.Clear(Color.LightGray);
					DrawDebugText(g, "not initialized", DEBUG_COORD_X, DEBUG_COORD_X, true);
					return result;
				}

				if (Sett.Year == null)
				{
					g.Clear(Color.LightGray);
					DrawDebugText(g, "incorrect settings", DEBUG_COORD_X, DEBUG_COORD_X, true);
					return result;
				}

				g.Clear(Color.WhiteSmoke);

				var yearTileWFDY = GeoModel.GetByYear(Sett.Year.Value);
				var visibleTileWFDY = yearTileWFDY.Where(wfdd => wfdd.wfdd.TileMapCoordinateBounds.IntersectsWith(Sett.CurrentMapCoordBounds)).ToArray();

				foreach (var info in visibleTileWFDY)
				{
					var mouseTile = Sett.Translate_MapCoordinateBounds_To_MapviewLocationBoundsPx(info.wfdd.TileMapCoordinateBounds);
					Image image;

					if (MapSettings.TryGetRecommendedThumbnailSize(Math.Max(mouseTile.Width, mouseTile.Height), out uint recommendedThumbnailSize))
					{
						image = ThumbDictionary.GetThumb(Sett.Year.Value, info.tileInfo.MA41.Square, info.tileInfo.MA41.Quadrant, recommendedThumbnailSize, () =>
						{
							loadedThumbs++;
							var thumbnailPath = GeoData.GetJpgThumbnailPath(Sett.Year.Value, info.tileInfo.MA41.Square, info.tileInfo.MA41.Quadrant, recommendedThumbnailSize);
							return Image.FromFile(thumbnailPath);
						});
					}
					else
					{
						// image must be added to (and retrieved from) dictionary resized, not full-size
						image = FullSizeTileDictionary.GetThumb(Sett.Year.Value, info.tileInfo.MA41.Square, info.tileInfo.MA41.Quadrant, uint.MinValue, () =>
						{
							loadedFullSize++;
							var path = GeoData.GetJpgPath(Sett.Year.Value, info.tileInfo.MA41.Square, info.tileInfo.MA41.Quadrant);
							return Image.FromFile(path);
						});
					}
					thumbnailSizes.Add(image.Size);

					g.DrawImage(image, mouseTile);

					if (DebugMode)
					{
						g.DrawRectangle(Pens.Purple, mouseTile.Left, mouseTile.Top, mouseTile.Width, mouseTile.Height);
						DrawSquareInfo(g, mouseTile, Sett.Year.Value, info, image.Size);
					}
				}

				int y = -5;
				DrawDebugText(g, $"Now: {DateTime.Now:yyyy/MM/dd HH:mm:ss.fff}", DEBUG_COORD_X, y += 15, FORCE_DEBUG);
				DrawDebugText(g, $"Year: {Sett.Year} (tiles: {visibleTileWFDY.Length} visible / {yearTileWFDY.Length} year's / {GeoModel.Tiles.Length} total)", DEBUG_COORD_X, y += 15, FORCE_DEBUG);
				DrawDebugText(g, $"Zoom: ratio {MapSettings.ZOOMS[Sett.ZoomLevel]}x, level {Sett.ZoomLevel} (range 0..{MapSettings.ZOOMS.Length - 1})", DEBUG_COORD_X, y += 15, FORCE_DEBUG);
				DrawDebugText(g, $"Map coordinates: XL,YT={Sett.CurrentMapCoordBounds.X:F2},{Sett.CurrentMapCoordBounds.Y:F2}; XR,YB={Sett.CurrentMapCoordBounds.Right:F2},{Sett.CurrentMapCoordBounds.Bottom:F2}; W,H={Sett.CurrentMapCoordBounds.Width:F2},{Sett.CurrentMapCoordBounds.Height:F2}", DEBUG_COORD_X, y += 15, FORCE_DEBUG);
				if (Sett.CurrentDebugInfoShown.MouseCursorInfo)
				{
					DrawDebugText(g, Sett.MouseLocationPx == PointF.Empty ? "Mouse out of bounds" : $"Mouse: mapview location {Sett.MouseLocationPx.X:F0}px, {Sett.MouseLocationPx.Y:F0}px / map coordinates {Sett.Translate_MapviewX_To_MapCoordinateX(Sett.MouseLocationPx.X):F1}, {Sett.Translate_MapviewY_To_MapCoordinateY(Sett.MouseLocationPx.Y):F1}", DEBUG_COORD_X, y += 15, FORCE_DEBUG);
					DrawDebugText(g, Sett.LastMouseDown_MapviewLocationPx == PointF.Empty ? "Mouse not down" : $"Mouse was down at {Sett.LastMouseDown_MapviewLocationPx.X:F1}, {Sett.LastMouseDown_MapviewLocationPx.Y:F1} (delta { Sett.LastMouseDown_MapviewLocationPx.X - Sett.MouseLocationPx.X:F1}, { Sett.LastMouseDown_MapviewLocationPx.Y - Sett.MouseLocationPx.Y:F1})", DEBUG_COORD_X, y += 15, FORCE_DEBUG);
				}
				if (Sett.CurrentDebugInfoShown.DrawingQualityInfo)
				{
					DrawDebugText(g, $"CompositingMode={g.CompositingMode}, CompositingQuality={g.CompositingQuality}, InterpolationMode={g.InterpolationMode}, PixelOffsetMode={g.PixelOffsetMode}, SmoothingMode={g.SmoothingMode}, TextRenderingHint={g.TextRenderingHint}, ", DEBUG_COORD_X, y += 15, FORCE_DEBUG);
				}
				if (Sett.CurrentDebugInfoShown.MemoryAllocationInfo)
				{
					DrawDebugText(g, $"Memory usage: total allocated {GC.GetTotalAllocatedBytes() / 1048576f:F2}MB, total {GC.GetTotalMemory(false) / 1048576f:F2}MB, current thread {GC.GetAllocatedBytesForCurrentThread() / 1048576f:F2}MB", DEBUG_COORD_X, y += 15, FORCE_DEBUG);
					DrawDebugText(g, $"Image dictionaries (count/capacity; +new): thumbnails {ThumbDictionary.Count}/{ThumbDictionary.DictionaryCapacity} (+{ThumbDictionary.Count - oldThumbCount}) / full-size {FullSizeTileDictionary.Count}/{FullSizeTileDictionary.DictionaryCapacity} (+{FullSizeTileDictionary.Count - oldFullsizeCount}) | sizes: {string.Join(", ", thumbnailSizes.OrderByDescending(size => size.Width * size.Height).ThenByDescending(size => size.Width).Select(size => $"{size.Width}x{size.Height}"))}", DEBUG_COORD_X, y += 15, FORCE_DEBUG);
				}
				stopwatch.Stop();
				DrawDebugText(g, $"Drawing took {stopwatch.ElapsedMilliseconds} ms (tiles loaded from file: thumbnails {loadedThumbs}, full-size {loadedFullSize})", DEBUG_COORD_X, Sett.MapviewSizePx.Height - 20, true);
				Sett.LastDrawingTime = DateTime.Now;
				return result;
			}
		}
	}
}
