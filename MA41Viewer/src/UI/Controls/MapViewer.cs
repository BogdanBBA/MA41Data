using MA41Viewer.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MA41Viewer.UI.Controls
{
	public partial class MapViewer : UserControl
	{
		public class MapSettings
		{
			public static readonly float[] ZOOMS = { 0.114375f, 0.1525f, 0.22875f, 0.305f, 0.4575f, 0.61035f, 0.915525f, 1.2207f, 1.83105f, 2.4414f, 3.6621f, 4.8828f, 7.3242f, 9.7656f, 14.6484f, 19.5312f, 29.2968f };

			public static readonly uint[] THUMBNAILS_SIZES = { 128, 256, 512, 1024, 2048 };

			public static bool TryGetRecommendedThumbnailSize(float tileLengthPx, out uint recommendedThumbnailSize)
			{
				foreach (uint thumbnailSize in THUMBNAILS_SIZES)
				{
					if (tileLengthPx <= 1.5 * thumbnailSize)
					{
						recommendedThumbnailSize = thumbnailSize;
						return true;
					}
				}
				recommendedThumbnailSize = 0u;
				return false;
			}

			public SizeF MapviewSizePx { get; set; } = SizeF.Empty;
			public PointF MapviewCenterPx => new(MapviewSizePx.Width / 2f, MapviewSizePx.Height / 2f);
			public PointF MouseLocationPx { get; set; } = PointF.Empty;
			public RectangleF CurrentMapCoordBounds { get; set; } = RectangleF.Empty;
			public PointF CurrentMapCoordCenter => new(CurrentMapCoordBounds.Left + CurrentMapCoordBounds.Width / 2, CurrentMapCoordBounds.Top + CurrentMapCoordBounds.Height / 2);
			public uint ZoomLevel { get; set; } = 11;
			public PointF LastMouseDown_MapviewLocationPx { get; set; } = PointF.Empty;
			public uint? Year { get; set; } = null;
			public DateTime LastDrawingTime { get; set; } = DateTime.Now;

			/// <summary>Gets the map coordinate bounds, centered on the given map coordinate. Takes the current zoom level / ratio and mapview size into account.</summary>
			/// <param name="centerCoordinate">the map coordinate to the center the map on</param>
			private RectangleF GetMapCoordBounds(PointF centerCoordinate)
			{
				var zoomRatio = ZOOMS[ZoomLevel];
				return new(centerCoordinate.X - MapviewSizePx.Width * zoomRatio / 2,
					centerCoordinate.Y - MapviewSizePx.Height * zoomRatio / 2,
					MapviewSizePx.Width * zoomRatio,
					MapviewSizePx.Height * zoomRatio);
			}

			/// <summary>Modifies the current map coordiante bounds so that the map is centered around the given map coordinate. Should be called after changes to the mapview settings that do not affect the location on the map.</summary>
			/// <param name="mouseMapviewLocationPx">the map coordinate to the center the map on</param>
			public void CenterMap(PointF newCenterCoordinate)
			{
				CurrentMapCoordBounds = GetMapCoordBounds(newCenterCoordinate);
			}

			/// <summary>Modifies the current map coordiante bounds to allow for a relatively intuitive zooming experience - the map 'zooms in where the mouse is'. Should be called after the zoom level has been changed.</summary>
			/// <param name="mouseMapviewLocationPx">the mapview location of the mouse</param>
			public void ResetMapAfterZoom(PointF mouseMapviewLocationPx)
			{
				var centerPx = MapviewCenterPx;
				var deltaToCenterPx = new SizeF(mouseMapviewLocationPx.X - centerPx.X, mouseMapviewLocationPx.Y - centerPx.Y);
				var zoomRatio = ZOOMS[ZoomLevel];
				var newMapBounds = GetMapCoordBounds(Translate_MapviewLocationPx_To_MapCoordinates(mouseMapviewLocationPx));
				CurrentMapCoordBounds = new RectangleF(newMapBounds.Left - deltaToCenterPx.Width * zoomRatio,
					newMapBounds.Top - deltaToCenterPx.Height * zoomRatio,
					newMapBounds.Width,
					newMapBounds.Height);
			}

			/// <summary>Modifies the current map coordiante bounds to allow panning the map by mouse
			/// . Should be called after a mouse up event on the mapview, with the 'last mouse down mapview location' property set.</summary>
			/// <param name="mouseMapviewLocationPx">the mapview location of the mouse</param>
			public void MoveMap(PointF mouseMapviewLocationPx)
			{
				var delta = new SizeF(LastMouseDown_MapviewLocationPx.X - mouseMapviewLocationPx.X, LastMouseDown_MapviewLocationPx.Y - mouseMapviewLocationPx.Y);
				var zoomRatio = ZOOMS[ZoomLevel];
				CurrentMapCoordBounds = new RectangleF(CurrentMapCoordBounds.Left + delta.Width * zoomRatio,
					CurrentMapCoordBounds.Top + delta.Height * zoomRatio,
					CurrentMapCoordBounds.Width,
					CurrentMapCoordBounds.Height);
				LastMouseDown_MapviewLocationPx = PointF.Empty;
			}

			//

			public float Translate_MapviewX_To_MapCoordinateX(float x)
				=> ((x / MapviewSizePx.Width) * CurrentMapCoordBounds.Width) + CurrentMapCoordBounds.Left;

			public float Translate_MapviewY_To_MapCoordinateY(float y)
				=> ((y / MapviewSizePx.Height) * CurrentMapCoordBounds.Height) + CurrentMapCoordBounds.Top;

			public PointF Translate_MapviewLocationPx_To_MapCoordinates(PointF mapviewLocationPx)
				=> new(Translate_MapviewX_To_MapCoordinateX(mapviewLocationPx.X), Translate_MapviewY_To_MapCoordinateY(mapviewLocationPx.Y));

			//

			public float Translate_MapCoordinateX_To_MapviewX(float x)
				=> (x - CurrentMapCoordBounds.Left) / CurrentMapCoordBounds.Width * MapviewSizePx.Width;

			public float Translate_MapCoordinateY_To_MapviewY(float y)
				=> (y - CurrentMapCoordBounds.Top) / CurrentMapCoordBounds.Height * MapviewSizePx.Height;

			public PointF Translate_MapCoordinates_To_MapviewLocationPx(PointF mapCoords)
				=> new(Translate_MapCoordinateX_To_MapviewX(mapCoords.X), Translate_MapCoordinateY_To_MapviewY(mapCoords.Y));

			public SizeF Translate_MapCoordinateSize_To_MapviewSizePx(SizeF mapCoordSize)
				=> new(mapCoordSize.Width / CurrentMapCoordBounds.Width * MapviewSizePx.Width, mapCoordSize.Height / CurrentMapCoordBounds.Height * MapviewSizePx.Height);

			public RectangleF Translate_MapCoordinateBounds_To_MapviewLocationBoundsPx(RectangleF tileMapCoordBounds)
			{
				//if (!MapCoords.IntersectsWith(tile)) return RectangleF.Empty;
				return new RectangleF(Translate_MapCoordinates_To_MapviewLocationPx(tileMapCoordBounds.Location), Translate_MapCoordinateSize_To_MapviewSizePx(tileMapCoordBounds.Size));
			}
		}

		private const int PAINT_COOLDOWN_MS = 50;
		private const int DEBUG_COORD_X = 10;
		protected static readonly Brush BACKBRUSH = new SolidBrush(Color.FromArgb(96, 255, 255, 255));

		private readonly object _lock = new();
		public bool DebugMode { get; set; } = true;
		protected Action<uint> MouseZoomCallback { get; private set; }
		protected GeoModel GeoModel { get; private set; }
		public MapSettings Sett { get; protected set; }
		protected ThumbnailDictionary ThumbDictionary { get; private set; }
		protected ThumbnailDictionary FullSizeTileDictionary { get; private set; }

		public MapViewer()
		{
			DoubleBuffered = true;
			Sett = new MapSettings();
			ThumbDictionary = new ThumbnailDictionary(2 * ThumbnailDictionary.DEFAULT_MINIMUM_THUMBNAIL_CAPACITY);
			FullSizeTileDictionary = new ThumbnailDictionary(2 * ThumbnailDictionary.DEFAULT_MINIMUM_FULLSIZE_CAPACITY);
		}

		public void InitializeGeoModel(GeoModel geoModel, Action<uint> mouseZoomCallback)
		{
			GeoModel = geoModel;
			MouseZoomCallback = mouseZoomCallback;
			Sett.MapviewSizePx = Size;
			Sett.CenterMap(GeoModel.CenterCoordinate);
			Invalidate();
		}

		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			//base.OnMouseDoubleClick(e);
			Sett.CenterMap(Sett.Translate_MapviewLocationPx_To_MapCoordinates(e.Location));
			Invalidate();
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			//base.OnMouseEnter(e);
			Invalidate();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			//base.OnMouseLeave(e);
			Sett.MouseLocationPx = PointF.Empty;
			Invalidate();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			//base.OnMouseMove(e);
			Sett.MouseLocationPx = e.Location;
			Invalidate();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			//base.OnMouseDown(e);
			Sett.LastMouseDown_MapviewLocationPx = e.Location;
			Invalidate();
		}


		protected override void OnMouseUp(MouseEventArgs e)
		{
			//base.OnMouseUp(e);
			Sett.MoveMap(e.Location);
			Invalidate();
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			//base.OnMouseWheel(e);
			Sett.ZoomLevel = (uint)Math.Max(0, Math.Min(MapSettings.ZOOMS.Length - 1, Sett.ZoomLevel + (-e.Delta / SystemInformation.MouseWheelScrollDelta)));
			Sett.ResetMapAfterZoom(e.Location);
			MouseZoomCallback(Sett.ZoomLevel);
			Invalidate();
		}

		protected override void OnResize(EventArgs e)
		{
			Sett.MapviewSizePx = Size;
			Sett.CenterMap(Sett.CurrentMapCoordCenter);
			base.OnResize(e);
		}

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
				//if (DateTime.Now < Sett.LastDrawingTime.AddMilliseconds(PAINT_COOLDOWN_MS))
				//	return;

				var stopwatch = Stopwatch.StartNew();
				var loadedThumbs = 0u;
				var loadedFullSize = 0u;
				var oldThumbCount = ThumbDictionary.Count;
				var oldFullsizeCount = FullSizeTileDictionary.Count;
				var thumbnailSizes = new HashSet<Size>();

				const bool FORCE_DEBUG = false;
				Graphics g = e.Graphics;
				g.CompositingMode = CompositingMode.SourceOver;
				g.CompositingQuality = FORCE_DEBUG ? CompositingQuality.HighSpeed : CompositingQuality.HighQuality;
				g.InterpolationMode = FORCE_DEBUG ? InterpolationMode.Low : InterpolationMode.HighQualityBicubic;
				g.PixelOffsetMode = FORCE_DEBUG ? PixelOffsetMode.None : PixelOffsetMode.HighQuality;
				g.SmoothingMode = FORCE_DEBUG ? SmoothingMode.HighSpeed : SmoothingMode.HighQuality;
				g.TextRenderingHint = FORCE_DEBUG ? TextRenderingHint.SingleBitPerPixel : TextRenderingHint.AntiAlias;

				if (GeoModel == null)
				{
					g.Clear(Color.LightGray);
					DrawDebugText(g, "not initialized", DEBUG_COORD_X, DEBUG_COORD_X, true);
					return;
				}

				if (Sett.Year == null)
				{
					g.Clear(Color.LightGray);
					DrawDebugText(g, "incorrect settings", DEBUG_COORD_X, DEBUG_COORD_X, true);
					return;
				}

				g.Clear(Color.WhiteSmoke);

				//DrawDebugText(g, DateTime.Now.ToString(), 10, 10, true);
				//return;

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
							//	using (var img = SixLabors.ImageSharp.Image.Load(thumbnailPath))
							//{
							//	return img.

							//}
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
							//using (var f = new ImageFactory(false))
							//{
							//	return f.Load(path).Resize(mouseTile.Size.ToSize()).Image;
							//}
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
				DrawDebugText(g, $"Now: {DateTime.Now:HH:mm:ss.fff}", DEBUG_COORD_X, y += 15, FORCE_DEBUG);
				DrawDebugText(g, $"Year: {Sett.Year} (tiles: {visibleTileWFDY.Length} visible / {yearTileWFDY.Length} year's / {GeoModel.Tiles.Length} total)", DEBUG_COORD_X, y += 15, FORCE_DEBUG);
				DrawDebugText(g, $"Zoom: ratio {MapSettings.ZOOMS[Sett.ZoomLevel]}x, level {Sett.ZoomLevel} (range 0..{MapSettings.ZOOMS.Length - 1})", DEBUG_COORD_X, y += 15, FORCE_DEBUG);
				DrawDebugText(g, $"Map coordinates: XL,YT={Sett.CurrentMapCoordBounds.X:F2},{Sett.CurrentMapCoordBounds.Y:F2}; XR,YB={Sett.CurrentMapCoordBounds.Right:F2},{Sett.CurrentMapCoordBounds.Bottom:F2}; W,H={Sett.CurrentMapCoordBounds.Width:F2},{Sett.CurrentMapCoordBounds.Height:F2}", DEBUG_COORD_X, y += 15, FORCE_DEBUG);
				DrawDebugText(g, Sett.MouseLocationPx == PointF.Empty ? "Mouse out of bounds" : $"Mouse: mapview location {Sett.MouseLocationPx.X:F0}px, {Sett.MouseLocationPx.Y:F0}px / map coordinates {Sett.Translate_MapviewX_To_MapCoordinateX(Sett.MouseLocationPx.X):F1}, {Sett.Translate_MapviewY_To_MapCoordinateY(Sett.MouseLocationPx.Y):F1}", DEBUG_COORD_X, y += 15, FORCE_DEBUG);
				DrawDebugText(g, Sett.LastMouseDown_MapviewLocationPx == PointF.Empty ? "Mouse not down" : $"Mouse was down at {Sett.LastMouseDown_MapviewLocationPx.X:F1}, {Sett.LastMouseDown_MapviewLocationPx.Y:F1} (delta { Sett.LastMouseDown_MapviewLocationPx.X - Sett.MouseLocationPx.X:F1}, { Sett.LastMouseDown_MapviewLocationPx.Y - Sett.MouseLocationPx.Y:F1})", DEBUG_COORD_X, y += 15, FORCE_DEBUG);
				DrawDebugText(g, $"CompositingMode={g.CompositingMode}, CompositingQuality={g.CompositingQuality}, InterpolationMode={g.InterpolationMode}, PixelOffsetMode={g.PixelOffsetMode}, SmoothingMode={g.SmoothingMode}, TextRenderingHint={g.TextRenderingHint}, ", DEBUG_COORD_X, y += 15, FORCE_DEBUG);
				DrawDebugText(g, $"Memory usage: total allocated {GC.GetTotalAllocatedBytes() / 1048576f:F2}MB, total {GC.GetTotalMemory(false) / 1048576f:F2}MB, current thread {GC.GetAllocatedBytesForCurrentThread() / 1048576f:F2}MB", DEBUG_COORD_X, y += 15, FORCE_DEBUG);
				DrawDebugText(g, $"Image dictionaries (count/capacity; +new): thumbnails {ThumbDictionary.Count}/{ThumbDictionary.DictionaryCapacity} (+{ThumbDictionary.Count - oldThumbCount}) / full-size {FullSizeTileDictionary.Count}/{FullSizeTileDictionary.DictionaryCapacity} (+{FullSizeTileDictionary.Count - oldFullsizeCount}) | sizes: {string.Join(", ", thumbnailSizes.OrderByDescending(size => size.Width * size.Height).ThenByDescending(size => size.Width).Select(size => $"{size.Width}x{size.Height}"))}", DEBUG_COORD_X, y += 15, FORCE_DEBUG);
				stopwatch.Stop();
				DrawDebugText(g, $"Drawing took {stopwatch.ElapsedMilliseconds} ms (tiles loaded from file: thumbnails {loadedThumbs}, full-size {loadedFullSize})", DEBUG_COORD_X, Sett.MapviewSizePx.Height - 20, true);
				Sett.LastDrawingTime = DateTime.Now;
				//g.Dispose(); // since Graphics 'g' was not created here, it should NOT be disposed
			}
		}
	}
}
