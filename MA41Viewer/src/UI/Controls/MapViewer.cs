using MA41Viewer.Data;
using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MA41Viewer.UI.Controls
{
	public partial class MapViewer : UserControl
	{
		protected static readonly Brush BACKBRUSH = new SolidBrush(Color.FromArgb(96, 255, 255, 255));
		protected static readonly Pen CROSSHAIR_PEN = new(Color.GreenYellow, 1); //width will be rounded to 1 in non-SmoothingMode.AntiAlias graphics, leave 1 for consistency

		private readonly object _lock = new();
		public bool DebugMode { get; set; } = false;
		public Action<uint, PointF> OnMapBoundsChanged { get; set; }
		public Action<Point?> OnMouseLocationPxChanged { get; set; }
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

		public void InitializeGeoModel(GeoModel geoModel)
		{
			GeoModel = geoModel;
			OnMapBoundsChanged = null;
			OnMouseLocationPxChanged = null;
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

		public void CrosshairChanged(Point? location)
		{
			Sett.CrosshairLocationPx = location;
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
			OnMapBoundsChanged?.Invoke(Sett.ZoomLevel, Sett.GetCurrentMapCoordCenter());
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
			OnMouseLocationPxChanged?.Invoke(null);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			Sett.MouseLocationPx = e.Location;
			if (DebugMode || Sett.DrawingState == MapSettings.MapDrawingState.MouseMovement)
			{
				Invalidate();
			}
			OnMouseLocationPxChanged?.Invoke(e.Location);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			Sett.DrawingState = MapSettings.MapDrawingState.MouseMovement;
			Sett.LastMouseDown_MapviewLocationPx = e.Location;
			if (DebugMode)
			{
				Invalidate();
			}
			//OnMouseLocationPxChanged?.Invoke(null);
		}


		protected override void OnMouseUp(MouseEventArgs e)
		{
			Sett.MoveMap(e.Location);
			Sett.DrawingState = MapSettings.MapDrawingState.AtRest;
			Invalidate();
			OnMapBoundsChanged?.Invoke(Sett.ZoomLevel, Sett.GetCurrentMapCoordCenter());
			//OnMouseLocationPxChanged?.Invoke(e.Location);
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			CancellationTokenSource cancellationTokenSource = new();
			CancellationToken token = cancellationTokenSource.Token;

			if (Sett.DrawingState == MapSettings.MapDrawingState.ZoomEvent)
			{
				cancellationTokenSource.Cancel();
			}
			Sett.DrawingState = MapSettings.MapDrawingState.ZoomEvent;
			Sett.ZoomLevel = (uint)Math.Max(0, Math.Min(MapSettings.ZOOMS.Length - 1, Sett.ZoomLevel + (+e.Delta / SystemInformation.MouseWheelScrollDelta)));
			Sett.ResetMapAfterZoom(e.Location);
			Invalidate();
			Task.Run(async () =>
			{
				await Task.Delay(1000);
				if (!token.IsCancellationRequested)
				{
					Sett.DrawingState = MapSettings.MapDrawingState.AtRest;
					Invalidate();
					OnMapBoundsChanged?.Invoke(Sett.ZoomLevel, Sett.GetCurrentMapCoordCenter());
				}
			}, token);
		}
		#endregion

		public void SaveCurrentViewToFile(string filename)
		{
			Bitmap bmp = GenerateBitmap(out DebugInfoWrapper _);
			bmp.Save(filename);
		}

		public void SaveCurrentViewsToFile(MapViewer otherMapViewer, string filename)
		{
			Bitmap bmpA = GenerateBitmap(out DebugInfoWrapper _);
			Bitmap bmpB = otherMapViewer.GenerateBitmap(out DebugInfoWrapper _);
			DrawYearOnExportBitmap(bmpA, Sett.Year.Value);
			DrawYearOnExportBitmap(bmpB, otherMapViewer.Sett.Year.Value);

			using Bitmap bmpResult = new(bmpA.Width + bmpB.Width, bmpA.Height);
			using Graphics g = Graphics.FromImage(bmpResult);
			g.CompositingMode = Sett.CurrentQualitySettings.CompositingModeValue;
			g.CompositingQuality = Sett.CurrentQualitySettings.CompositingQualityValue;
			g.InterpolationMode = Sett.CurrentQualitySettings.InterpolationModeValue;
			g.PixelOffsetMode = Sett.CurrentQualitySettings.PixelOffsetModeValue;
			g.SmoothingMode = Sett.CurrentQualitySettings.SmoothingModeValue;
			g.TextRenderingHint = Sett.CurrentQualitySettings.TextRenderingHintValue;
			g.DrawImage(bmpA, 0, 0);
			g.DrawImage(bmpB, bmpA.Width, 0);
			bmpResult.Save(filename);
		}

		public void SaveAllYearsToFile(string filename)
		{
			Sett.SaveState();
			uint[] years = [1938, 1956, 1961, 1971, 1981, 1992, 2014, 2024];
			if (years.Any(year => !GeoModel.Years.Contains(year)))
				years = GeoModel.Years;
			Bitmap bmp = GenerateAllYearsBitmap(years);
			bmp.Save(filename);
			Sett.RestoreState();
		}
	}
}
