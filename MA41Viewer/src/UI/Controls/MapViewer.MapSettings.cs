using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;

namespace MA41Viewer.UI.Controls
{
	public class MapSettings
	{
		// inner classes

		public class QualitySettings
		{
			public static readonly QualitySettings LOW = new(CompositingMode.SourceOver, CompositingQuality.HighSpeed, InterpolationMode.Low, PixelOffsetMode.HighSpeed, SmoothingMode.HighSpeed, TextRenderingHint.SingleBitPerPixel);
			public static readonly QualitySettings MEDIUM = new(CompositingMode.SourceOver, CompositingQuality.GammaCorrected, InterpolationMode.NearestNeighbor, PixelOffsetMode.HighQuality, SmoothingMode.HighQuality, TextRenderingHint.AntiAlias);
			public static readonly QualitySettings HIGH = new(CompositingMode.SourceOver, CompositingQuality.HighQuality, InterpolationMode.HighQualityBicubic, PixelOffsetMode.Half, SmoothingMode.AntiAlias, TextRenderingHint.ClearTypeGridFit);

			public CompositingMode CompositingModeValue { get; private set; }
			public CompositingQuality CompositingQualityValue { get; private set; }
			public InterpolationMode InterpolationModeValue { get; private set; }
			public PixelOffsetMode PixelOffsetModeValue { get; private set; }
			public SmoothingMode SmoothingModeValue { get; private set; }
			public TextRenderingHint TextRenderingHintValue { get; private set; }

			public QualitySettings(int cmv, int cqv, int imv, int pomv, int smv, int trhv)
				: this((CompositingMode)cmv, (CompositingQuality)cqv, (InterpolationMode)imv, (PixelOffsetMode)pomv, (SmoothingMode)smv, (TextRenderingHint)trhv) { }

			public QualitySettings(CompositingMode compositingModeValue, CompositingQuality compositingQualityValue, InterpolationMode interpolationModeValue, PixelOffsetMode pixelOffsetModeValue, SmoothingMode smoothingModeValue, TextRenderingHint textRenderingHintValue)
			{
				CompositingModeValue = compositingModeValue;
				CompositingQualityValue = compositingQualityValue;
				InterpolationModeValue = interpolationModeValue;
				PixelOffsetModeValue = pixelOffsetModeValue;
				SmoothingModeValue = smoothingModeValue;
				TextRenderingHintValue = textRenderingHintValue;
			}

			public void SetFrom(QualitySettings settings)
			{
				CompositingModeValue = settings.CompositingModeValue;
				CompositingQualityValue = settings.CompositingQualityValue;
				InterpolationModeValue = settings.InterpolationModeValue;
				PixelOffsetModeValue = settings.PixelOffsetModeValue;
				SmoothingModeValue = settings.SmoothingModeValue;
				TextRenderingHintValue = settings.TextRenderingHintValue;
			}

			public override string ToString()
				=> $"{(int)CompositingModeValue},{(int)CompositingQualityValue},{(int)InterpolationModeValue},{(int)PixelOffsetModeValue},{(int)SmoothingModeValue},{(int)TextRenderingHintValue}";
		}

		public class DebugInfoShown
		{
			public bool MouseCursorInfo { get; set; } = true;
			public bool DrawingQualityInfo { get; set; } = true;
			public bool MemoryAllocationInfo { get; set; } = true;

			public DebugInfoShown()
				: this(true, true, true) { }

			public DebugInfoShown(bool mouseCursorInfo, bool drawingQualityInfo, bool memoryAllocationInfo)
			{
				MouseCursorInfo = mouseCursorInfo;
				DrawingQualityInfo = drawingQualityInfo;
				MemoryAllocationInfo = memoryAllocationInfo;
			}

			public void SetFrom(bool mouseCursorInfo, bool drawingQualityInfo, bool memoryAllocationInfo)
			{
				MouseCursorInfo = mouseCursorInfo;
				DrawingQualityInfo = drawingQualityInfo;
				MemoryAllocationInfo = memoryAllocationInfo;
			}

			public override string ToString()
				=> $"{MouseCursorInfo},{DrawingQualityInfo},{MemoryAllocationInfo}";
		}

		protected class ObjectState
		{
			public uint Year { get; set; }
		}

		// static members and methods

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

		// instance members

		public QualitySettings CurrentQualitySettings { get; private set; } = QualitySettings.HIGH;
		public DebugInfoShown CurrentDebugInfoShown { get; private set; } = new DebugInfoShown();
		public SizeF MapviewSizePx { get; set; } = SizeF.Empty;
		public PointF MouseLocationPx { get; set; } = PointF.Empty;
		public RectangleF CurrentMapCoordBounds { get; set; } = RectangleF.Empty;
		public uint ZoomLevel { get; set; } = 11;
		public PointF LastMouseDown_MapviewLocationPx { get; set; } = PointF.Empty;
		public uint? Year { get; set; } = null;
		public DateTime LastDrawingTime { get; set; } = DateTime.Now;
		protected ObjectState BackupState { get; private set; } = null;

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
			var centerPx = GetMapviewCenterPx();
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

		public PointF GetMapviewCenterPx()
			=> new(MapviewSizePx.Width / 2f, MapviewSizePx.Height / 2f);

		public PointF GetCurrentMapCoordCenter()
			=> new(CurrentMapCoordBounds.Left + CurrentMapCoordBounds.Width / 2, CurrentMapCoordBounds.Top + CurrentMapCoordBounds.Height / 2);

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
			=> new(Translate_MapCoordinates_To_MapviewLocationPx(tileMapCoordBounds.Location), Translate_MapCoordinateSize_To_MapviewSizePx(tileMapCoordBounds.Size));

		//

		public void LoadFromFile(string path)
		{
			string[] lines = File.ReadAllLines(path);
			Year = uint.Parse(lines[0]);
			ZoomLevel = uint.Parse(lines[1]);
			float[] parts = lines[2].Split(',').Select(part => float.Parse(part)).ToArray();
			CenterMap(new PointF(parts[0], parts[1]));
			// default drawing quality setting should be HIGH
			//int[] qs = lines[3].Split(',').Select(part => int.Parse(part)).ToArray();
			//CurrentQualitySettings = new QualitySettings(qs[0], qs[1], qs[2], qs[3], qs[4], qs[5]);
			bool[] vals = lines[3].Split(',').Select(part => bool.Parse(part)).ToArray();
			CurrentDebugInfoShown.SetFrom(vals[0], vals[1], vals[2]);
		}

		public void SaveToFile(string path)
		{
			PointF mapCenter = GetCurrentMapCoordCenter();
			File.WriteAllText(path, new StringBuilder()
				.AppendLine($"{Year}")
				.AppendLine($"{ZoomLevel}")
				.AppendLine($"{mapCenter.X:F},{mapCenter.Y:F}")
				// default drawing quality setting should be HIGH
				//.AppendLine(CurrentQualitySettings.ToString())
				.AppendLine(CurrentDebugInfoShown.ToString())
				.ToString());
		}

		public void SaveState()
		{
			BackupState = new ObjectState()
			{
				Year = Year.Value
			};
		}

		public void RestoreState()
		{
			Year = BackupState.Year;
			BackupState = null;
		}
	}
}
