using MA41Viewer.Data;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MA41Viewer.UI.Controls
{
	public partial class MapViewer : UserControl
	{
		protected static readonly Brush BACKBRUSH = new SolidBrush(Color.FromArgb(96, 255, 255, 255));

		private readonly object _lock = new();
		public bool DebugMode { get; set; } = false;
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
			if (DebugMode)
			{
				Invalidate();
			}
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			Sett.MouseLocationPx = PointF.Empty;
			if (DebugMode)
			{
				Invalidate();
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			Sett.DrawingState = MapSettings.MapDrawingState.MouseMovement;
			Sett.LastMouseDown_MapviewLocationPx = e.Location;
			if (DebugMode)
			{
				Invalidate();
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			Sett.MouseLocationPx = e.Location;
			if (DebugMode || Sett.DrawingState == MapSettings.MapDrawingState.MouseMovement)
			{
				Invalidate();
			}
		}


		protected override void OnMouseUp(MouseEventArgs e)
		{
			Sett.MoveMap(e.Location);
			Invalidate();
			Sett.DrawingState = MapSettings.MapDrawingState.AtRest;
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			CancellationTokenSource cancellationTokenSource = new ();
			CancellationToken token = cancellationTokenSource.Token;

			if (Sett.DrawingState == MapSettings.MapDrawingState.ZoomEvent)
			{
				cancellationTokenSource.Cancel();
			}
			Sett.DrawingState = MapSettings.MapDrawingState.ZoomEvent;
			Sett.ZoomLevel = (uint)Math.Max(0, Math.Min(MapSettings.ZOOMS.Length - 1, Sett.ZoomLevel + (-e.Delta / SystemInformation.MouseWheelScrollDelta)));
			Sett.ResetMapAfterZoom(e.Location);
			MouseZoomCallback(Sett.ZoomLevel);
			Invalidate();
			Task.Run(async () =>
			{
				await Task.Delay(1000);
				if (!token.IsCancellationRequested)
				{
					Sett.DrawingState = MapSettings.MapDrawingState.AtRest;
					Invalidate();
				}
			}, token);
		}
		#endregion

		public void SaveCurrentViewToFile(string filename)
		{
			var bmp = GenerateBitmap(out DebugInfoWrapper _);
			bmp.Save(filename);
		}

		public void SaveAllYearsToFile(string filename)
		{
			Sett.SaveState();
			var bmp = GenerateAllYearsBitmap(GeoModel.Years);
			bmp.Save(filename);
			Sett.RestoreState();
		}
	}
}
