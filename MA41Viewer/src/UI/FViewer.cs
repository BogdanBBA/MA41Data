using MA41.Commons;
using MA41Viewer.Data;
using MA41Viewer.src.UI.Controls;
using MA41Viewer.UI.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MA41Viewer.UI
{
	public partial class FViewer : Form
	{
		protected static readonly List<Tuple<uint, float, float>> DEFAULT_LOCATIONS = new() // order as per UI // X, Y, Zoom
		{
			new(16, 3750, -12250),
			new(9, 3000, -11111),
			new(6, 3350, -8500),
			new(2, 2390, -9160),
			new(2, 6460, -19570),
			new(7, -3400, -9250),
			new(7, 10400, -16100)
		};

		public FViewer()
		{
			InitializeComponent();
		}

		private void FViewer_Load(object sender, EventArgs e)
		{
			FViewer_Resize(sender, e);

			string initializeResult = GeoData.Initialize();
			if (initializeResult != string.Empty)
			{
				MessageBox.Show(initializeResult, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			InitializeMyComponents();
		}

		private void FViewer_Resize(object sender, EventArgs e)
		{
			const int hPadd = 10, wZoom = 80, hYear = 45;
			Rectangle controlArea = new(5, 25, Width - 22, Height - 70);

			_zoomControl.SetBounds(controlArea.Left, controlArea.Top, wZoom, controlArea.Height);
			_zoomControl.Invalidate();

			_YearControlLeft.SetBounds(_zoomControl.Right + hPadd, controlArea.Top, (controlArea.Width - _zoomControl.Right - hPadd) / 2 - hPadd / 2, hYear);
			_YearControlLeft.Invalidate();

			_MapViewerLeft.SetBounds(_YearControlLeft.Left, _YearControlLeft.Bottom, _YearControlLeft.Width, controlArea.Height - _YearControlLeft.Height);
			_MapViewerLeft.Invalidate();

			_YearControlRight.SetBounds(_YearControlLeft.Right + hPadd, _YearControlLeft.Top, _YearControlLeft.Width, _YearControlLeft.Height);
			_YearControlRight.Invalidate();

			_MapViewerRight.SetBounds(_YearControlRight.Left, _YearControlRight.Bottom, _YearControlRight.Width, _MapViewerLeft.Height);
			_MapViewerRight.Invalidate();
		}

		private void InitializeMyComponents()
		{
			_MapViewerLeft.InitializeGeoModel(GeoData.GeoModel);
			_MapViewerLeft.Sett.LoadFromFile(Paths.SETTINGS_FILE_LEFT);
			_MapViewerLeft.OnMapBoundsChanged = (zoom, center) =>
			{
				_MapViewerRight.CenterAndZoom(zoom, center.X, center.Y);
				_zoomControl.SelectedItemIndex = (int)zoom;
				_zoomControl.Invalidate();
			};

			_MapViewerRight.InitializeGeoModel(GeoData.GeoModel);
			_MapViewerRight.Sett.LoadFromFile(Paths.SETTINGS_FILE_RIGHT);
			_MapViewerRight.OnMapBoundsChanged = (zoom, center) =>
			{
				_MapViewerLeft.CenterAndZoom(zoom, center.X, center.Y);
				_zoomControl.SelectedItemIndex = (int)zoom;
				_zoomControl.Invalidate();
			};

			_zoomControl.SetColors(ItemListControl.BackgroundColorsBlue, ItemListControl.TextColorsBlue);
			_YearControlLeft.SetColors(ItemListControl.BackgroundColorsRed, ItemListControl.TextColorsRed);
			_YearControlRight.SetColors(ItemListControl.BackgroundColorsRed, ItemListControl.TextColorsRed);

			_zoomControl.Items = MapSettings.ZOOMS.Select(zoom => new StringWithTag<uint>(zoom.Level, $"Zoom {zoom.Level}", GetZoomDescription(zoom))).ToList();
			List<StringWithTag<uint>> yearItemList = GeoData.GeoModel.Years
				.OrderBy(year => year)
				.Select(year => new StringWithTag<uint>(year, year.ToString(), GetYearDescription(year)))
				.ToList();
			_YearControlLeft.Items = yearItemList;
			_YearControlRight.Items = yearItemList;

			_zoomControl.SelectedItemIndex = (int)_MapViewerLeft.Sett.ZoomLevel;
			_YearControlLeft.SelectedItemIndex = yearItemList.FindIndex(item => item.Tag == _MapViewerLeft.Sett.Year);
			_YearControlRight.SelectedItemIndex = yearItemList.FindIndex(item => item.Tag == _MapViewerRight.Sett.Year);

			_zoomControl.OnSelectedItemChanged += (zoom) =>
			{
				_MapViewerLeft.Sett.ZoomLevel = zoom - 1;
				_MapViewerRight.Sett.ZoomLevel = zoom - 1;

				_MapViewerLeft.Sett.CenterMap(_MapViewerLeft.Sett.GetCurrentMapCoordCenter());
				_MapViewerRight.Sett.CenterMap(_MapViewerRight.Sett.GetCurrentMapCoordCenter());

				_MapViewerLeft.Invalidate();
				_MapViewerRight.Invalidate();
			};
			_YearControlLeft.OnSelectedItemChanged += (year) =>
			{
				_MapViewerLeft.Sett.Year = year;
				_MapViewerLeft.Invalidate();
			};
			_YearControlRight.OnSelectedItemChanged += (year) =>
			{
				_MapViewerRight.Sett.Year = year;
				_MapViewerRight.Invalidate();
			};

			// default drawing quality setting should be HIGH
			SetDrawingQualityToolstripItemChecked(highToolStripMenuItem);
			debugONOFFToolStripMenuItem.Checked = _MapViewerLeft.DebugMode;
			mouseCursorInfoToolStripMenuItem.Checked = _MapViewerLeft.Sett.CurrentDebugInfoShown.MouseCursorInfo;
			drawingQualityInfoToolStripMenuItem.Checked = _MapViewerLeft.Sett.CurrentDebugInfoShown.DrawingQualityInfo;
			memoryAllocationInfoToolStripMenuItem.Checked = _MapViewerLeft.Sett.CurrentDebugInfoShown.MemoryAllocationInfo;
			detailedTileInfoToolStripMenuItem.Checked = _MapViewerLeft.Sett.CurrentDebugInfoShown.DetailedTileInfo;
		}

		public string GetYearDescription(uint year)
		{
			return (int)year switch
			{
				1938 or 1976 or 1992 or 2014 or 2019 or 2020 => "summer",
				1956 or 2021 => "winter",
				_ => null
			};
		}

		public string GetZoomDescription(MapSettings.Zoom zoom)
		{
			return $"{zoom.Ratio:F2}x";
		}

		private void FViewer_KeyDown(object sender, KeyEventArgs e)
		{
			//MessageBox.Show("Not yet implemented.");

			//YearHeaderL.Text = e.KeyData.ToString();
			//if ((e.KeyData & Keys.N) == Keys.N)
			//{
			//	var oldIndex = Enumerable.Range(0, YearFLP.Controls.Count).Where(index => (YearFLP.Controls[index] as RadioButton).Checked).First();
			//	var newIndex = oldIndex + (((e.KeyData & Keys.Shift) == Keys.Shift) ? -1 : 1);
			//	newIndex = newIndex < 0 ? YearFLP.Controls.Count - 1 : (newIndex >= YearFLP.Controls.Count ? 0 : newIndex);
			//	//(YearFLP.Controls[oldIndex] as RadioButton).Checked = false;
			//	DebugModeChB_CheckedChanged(YearFLP.Controls[newIndex], e);
			//}
		}

		protected override void OnClosed(EventArgs e)
		{
			_MapViewerLeft.Sett.SaveToFile(Paths.SETTINGS_FILE_LEFT);
			_MapViewerRight.Sett.SaveToFile(Paths.SETTINGS_FILE_RIGHT);
			base.OnClosed(e);
		}

		#region Main menu events
		private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show($"by BogdanBBA{Environment.NewLine}{Environment.NewLine}December 2021 - January 2022{Environment.NewLine}October 2022");
		}

		private void ShowInExplorerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("explorer.exe", Paths.ROOT_FOLDER);
		}

		private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void Location_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var toolstripItemTag = int.Parse((sender as ToolStripMenuItem).Tag as string);
			var location = DEFAULT_LOCATIONS[toolstripItemTag];
			_MapViewerLeft.CenterAndZoom(location.Item1, location.Item2, location.Item3);
			_MapViewerRight.CenterAndZoom(location.Item1, location.Item2, location.Item3);
		}

		private void DebugONOFFToolStripMenuItem_Click(object sender, EventArgs e)
		{
			debugONOFFToolStripMenuItem.Checked = !debugONOFFToolStripMenuItem.Checked;
			_MapViewerLeft.DebugMode = debugONOFFToolStripMenuItem.Checked;
			_MapViewerRight.DebugMode = debugONOFFToolStripMenuItem.Checked;
			_MapViewerLeft.Invalidate();
			_MapViewerRight.Invalidate();
		}

		private void SetDrawingQualityToolstripItemChecked(ToolStripItem item)
		{
			foreach (var iItem in new[] { lowToolStripMenuItem, mediumToolStripMenuItem, highToolStripMenuItem })
			{
				iItem.Checked = iItem.Equals(item);
			}
		}

		private void LowToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetDrawingQualityToolstripItemChecked(sender as ToolStripItem);
			_MapViewerLeft.Sett.CurrentQualitySettings.SetFrom(MapSettings.QualitySettings.LOW);
			_MapViewerRight.Sett.CurrentQualitySettings.SetFrom(MapSettings.QualitySettings.LOW);
			_MapViewerLeft.Invalidate();
			_MapViewerRight.Invalidate();
		}

		private void MediumToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetDrawingQualityToolstripItemChecked(sender as ToolStripItem);
			_MapViewerLeft.Sett.CurrentQualitySettings.SetFrom(MapSettings.QualitySettings.MEDIUM);
			_MapViewerRight.Sett.CurrentQualitySettings.SetFrom(MapSettings.QualitySettings.MEDIUM);
			_MapViewerLeft.Invalidate();
			_MapViewerRight.Invalidate();
		}

		private void HighToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetDrawingQualityToolstripItemChecked(sender as ToolStripItem);
			_MapViewerLeft.Sett.CurrentQualitySettings.SetFrom(MapSettings.QualitySettings.HIGH);
			_MapViewerRight.Sett.CurrentQualitySettings.SetFrom(MapSettings.QualitySettings.HIGH);
			_MapViewerLeft.Invalidate();
			_MapViewerRight.Invalidate();
		}

		private void MouseCursorInfoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			mouseCursorInfoToolStripMenuItem.Checked = !mouseCursorInfoToolStripMenuItem.Checked;
			_MapViewerLeft.Sett.CurrentDebugInfoShown.MouseCursorInfo = mouseCursorInfoToolStripMenuItem.Checked;
			_MapViewerRight.Sett.CurrentDebugInfoShown.MouseCursorInfo = mouseCursorInfoToolStripMenuItem.Checked;
			_MapViewerLeft.Invalidate();
			_MapViewerRight.Invalidate();
		}

		private void DrawingQualityInfoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			drawingQualityInfoToolStripMenuItem.Checked = !drawingQualityInfoToolStripMenuItem.Checked;
			_MapViewerLeft.Sett.CurrentDebugInfoShown.DrawingQualityInfo = drawingQualityInfoToolStripMenuItem.Checked;
			_MapViewerRight.Sett.CurrentDebugInfoShown.DrawingQualityInfo = drawingQualityInfoToolStripMenuItem.Checked;
			_MapViewerLeft.Invalidate();
			_MapViewerRight.Invalidate();
		}

		private void MemoryAllocationInfoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			memoryAllocationInfoToolStripMenuItem.Checked = !memoryAllocationInfoToolStripMenuItem.Checked;
			_MapViewerLeft.Sett.CurrentDebugInfoShown.MemoryAllocationInfo = memoryAllocationInfoToolStripMenuItem.Checked;
			_MapViewerRight.Sett.CurrentDebugInfoShown.MemoryAllocationInfo = memoryAllocationInfoToolStripMenuItem.Checked;
			_MapViewerLeft.Invalidate();
			_MapViewerRight.Invalidate();
		}

		private void DetailedTileInfoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			detailedTileInfoToolStripMenuItem.Checked = !detailedTileInfoToolStripMenuItem.Checked;
			_MapViewerLeft.Sett.CurrentDebugInfoShown.DetailedTileInfo = detailedTileInfoToolStripMenuItem.Checked;
			_MapViewerRight.Sett.CurrentDebugInfoShown.DetailedTileInfo = detailedTileInfoToolStripMenuItem.Checked;
			_MapViewerLeft.Invalidate();
			_MapViewerRight.Invalidate();
		}

		private void CurrentViewToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string filename = @$"{Paths.EXPORTS_FOLDER}\{DateTime.Now:yyyyMMdd_HHmmss}.png";
			_MapViewerLeft.SaveCurrentViewToFile(filename);
			Process.Start("explorer.exe", $"/select, \"{filename}\"");
		}

		private void CurrentViewallYearsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string filename = @$"{Paths.EXPORTS_FOLDER}\{DateTime.Now:yyyyMMdd_HHmmss}.png";
			_MapViewerLeft.SaveAllYearsToFile(filename);
			Process.Start("explorer.exe", $"/select, \"{filename}\"");
		}
		#endregion
	}
}
