using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace MA41Viewer.UI.Controls
{
	public partial class MapSettings
	{
		protected class ObjectState
		{
			public uint Year { get; set; }
		}

		public enum MapDrawingState
		{
			AtRest,
			MouseMovement,
			ZoomEvent
		}

		// static members and methods

		public class Zoom(uint level, float ratio)
		{
			public uint Level { get; private set; } = level;
			public float Ratio { get; private set; } = ratio;
		}

		public static readonly float[] zoomRatios = [0.114375f, 0.1525f, 0.22875f, 0.305f, 0.4575f, 0.61035f, 0.915525f, 1.2207f, 1.83105f, 2.4414f, 3.6621f, 4.8828f, 7.3242f, 9.7656f, 14.6484f, 19.5312f, 29.2968f];

		public static readonly Zoom[] ZOOMS;

		static MapSettings()
		{
			ZOOMS = Enumerable.Range(1, zoomRatios.Length)
				.Select(zoomLevel => new Zoom((uint)zoomLevel, zoomRatios[^zoomLevel]))
				.ToArray();
		}

		// instance members

		public QualitySettings CurrentQualitySettings { get; private set; } = new();
		public DebugInfoShown CurrentDebugInfoShown { get; private set; } = new DebugInfoShown();
		public MapDrawingState DrawingState { get; set; } = MapDrawingState.AtRest;
		public SizeF MapviewSizePx { get; set; } = SizeF.Empty;
		public PointF MouseLocationPx { get; set; } = PointF.Empty;
		public Point? CrosshairLocationPx { get; set; } = null;
		public RectangleF CurrentMapCoordBounds { get; set; } = RectangleF.Empty;
		public KeyValuePair<RectangleF, Bitmap>? LastFrame { get; set; } = null;
		public uint ZoomLevel { get; set; } = 11;
		public PointF LastMouseDown_MapviewLocationPx { get; set; } = PointF.Empty;
		public uint? Year { get; set; } = null;
		public DateTime LastDrawingTime { get; set; } = DateTime.Now;
		protected ObjectState BackupState { get; private set; } = null;

		/// <summary>Gets the map coordinate bounds, centered on the given map coordinate. Takes the current zoom level / ratio and mapview size into account.</summary>
		/// <param name="centerCoordinate">the map coordinate to the center the map on</param>
		private RectangleF GetMapCoordBounds(PointF centerCoordinate)
		{
			float zoomRatio = ZOOMS[ZoomLevel].Ratio;
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
		/// <param name="alsoUpdateLastFrameBounds">indicated whether the bounds of the last (saved) frame are to be updated to reflect the new map bounds after this zoom event</param>
		public void ResetMapAfterZoom(PointF mouseMapviewLocationPx)
		{
			PointF centerPx = GetMapviewCenterPx();
			SizeF deltaToCenterPx = new(mouseMapviewLocationPx.X - centerPx.X, mouseMapviewLocationPx.Y - centerPx.Y);
			float zoomRatio = ZOOMS[ZoomLevel].Ratio;
			//RectangleF oldMapBounds = CurrentMapCoordBounds;
			RectangleF newMapBounds = GetMapCoordBounds(Translate_MapviewLocationPx_To_MapCoordinates(mouseMapviewLocationPx));
			CurrentMapCoordBounds = new RectangleF(
				newMapBounds.Left - deltaToCenterPx.Width * zoomRatio,
				newMapBounds.Top - deltaToCenterPx.Height * zoomRatio,
				newMapBounds.Width,
				newMapBounds.Height);
		}

		/// <summary>Modifies the current map coordiante bounds to allow panning the map by mouse
		/// . Should be called after a mouse up event on the mapview, with the 'last mouse down mapview location' property set.</summary>
		/// <param name="mouseMapviewLocationPx">the mapview location of the mouse</param>
		public void MoveMap(PointF mouseMapviewLocationPx)
		{
			SizeF delta = new(LastMouseDown_MapviewLocationPx.X - mouseMapviewLocationPx.X, LastMouseDown_MapviewLocationPx.Y - mouseMapviewLocationPx.Y);
			float zoomRatio = ZOOMS[ZoomLevel].Ratio;
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
			float[] parts = lines[2].Split(',').Select(float.Parse).ToArray();
			CenterMap(new PointF(parts[0], parts[1]));
			// default drawing quality setting should be HIGH
			//int[] qs = lines[3].Split(',').Select(part => int.Parse(part)).ToArray();
			//CurrentQualitySettings = new QualitySettings(qs[0], qs[1], qs[2], qs[3], qs[4], qs[5], ...);
			List<bool> vals = lines[3].Split(',').Select(bool.Parse).ToList();
			if (vals.Count < 4) throw new ArgumentException($"Unexpected settings vals list: '{lines[3]}'");
			CurrentDebugInfoShown.SetFrom(vals[0], vals[1], vals[2], vals[3]);
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
