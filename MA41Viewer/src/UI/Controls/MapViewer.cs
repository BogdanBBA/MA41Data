using MA41Viewer.Data;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MA41Viewer.UI.Controls
{
	public partial class MapViewer : UserControl
	{
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

		public void CenterAndZoom(uint zoomLevel, float centerCoordinateX, float centerCoordinateY)
		{
			Sett.ZoomLevel = zoomLevel;
			Sett.CenterMap(new PointF(centerCoordinateX, centerCoordinateY));
			Invalidate();
		}

		protected override void OnResize(EventArgs e)
		{
			Sett.MapviewSizePx = Size;
			Sett.CenterMap(Sett.GetCurrentMapCoordCenter());
			base.OnResize(e);
		}

		#region Mouse events
		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			Sett.CenterMap(Sett.Translate_MapviewLocationPx_To_MapCoordinates(e.Location));
			Invalidate();
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			Invalidate();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			Sett.MouseLocationPx = PointF.Empty;
			Invalidate();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			Sett.MouseLocationPx = e.Location;
			Invalidate();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			Sett.LastMouseDown_MapviewLocationPx = e.Location;
			Invalidate();
		}


		protected override void OnMouseUp(MouseEventArgs e)
		{
			Sett.MoveMap(e.Location);
			Invalidate();
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			Sett.ZoomLevel = (uint)Math.Max(0, Math.Min(MapSettings.ZOOMS.Length - 1, Sett.ZoomLevel + (-e.Delta / SystemInformation.MouseWheelScrollDelta)));
			Sett.ResetMapAfterZoom(e.Location);
			MouseZoomCallback(Sett.ZoomLevel);
			Invalidate();
		}
		#endregion
	}
}
