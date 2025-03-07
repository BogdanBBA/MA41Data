using MA41.Commons;
using MA41Viewer.Data;
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
		protected static readonly List<Tuple<uint, float, float>> DEFAULT_LOCATIONS = // order as per UI // X, Y, Zoom		
		[
			new(16, 3750, -12250),
			new(9, 3000, -11111),
			new(6, 3350, -8500),
			new(2, 2390, -9160),
			new(2, 6460, -19570),
			new(7, -3400, -9250),
			new(7, 10400, -16100)
		];

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
			const int zoomCtrlWidth = 60, yearCtrlHeight = 50, paddH = 10;
			Rectangle allControlsArea = new(5, 25, Width - 22, Height - 70);

			_zoomControl.SetBounds(allControlsArea.Left, allControlsArea.Top, zoomCtrlWidth, allControlsArea.Height);

			if (ToggleSecondViewToolStripMenuItem.Checked)
			{
				_YearControlLeft.SetBounds(_zoomControl.Right + paddH, allControlsArea.Top, (allControlsArea.Width - _zoomControl.Right - paddH) / 2 - paddH / 2, yearCtrlHeight);
				_MapViewerLeft.SetBounds(_YearControlLeft.Left, _YearControlLeft.Bottom, _YearControlLeft.Width, allControlsArea.Height - _YearControlLeft.Height);
				_YearControlRight.SetBounds(_YearControlLeft.Right + paddH, _YearControlLeft.Top, _YearControlLeft.Width, _YearControlLeft.Height);
				_MapViewerRight.SetBounds(_YearControlRight.Left, _YearControlRight.Bottom, _YearControlRight.Width, _MapViewerLeft.Height);
				_YearControlRight.Visible = true;
				_MapViewerRight.Visible = true;
			}
			else
			{
				_YearControlLeft.SetBounds(_zoomControl.Right + paddH, allControlsArea.Top, allControlsArea.Width - _zoomControl.Right - paddH, yearCtrlHeight);
				_MapViewerLeft.SetBounds(_YearControlLeft.Left, _YearControlLeft.Bottom, _YearControlLeft.Width, allControlsArea.Height - _YearControlLeft.Height);
				_YearControlRight.Visible = false;
				_MapViewerRight.Visible = false;
			}

			_zoomControl.Invalidate();
			_YearControlLeft.Invalidate();
			_MapViewerLeft.Invalidate();
			_YearControlRight.Invalidate();
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
			_MapViewerLeft.OnMouseLocationPxChanged = location =>
			{
				if (!_MapViewerRight.Visible) return; // check only here!
				if (!CrosshairToolStripMenuItem.Checked) return;
				if (_MapViewerLeft.Sett.DrawingState == MapSettings.MapDrawingState.AtRest)
					_MapViewerRight.CrosshairChanged(location);
			};
			_MapViewerLeft.OnScrollWithModifierKey = (key, forwards) =>
			{
				_YearControlLeft.SelectedItemIndex += forwards ? 1 : -1;
			};

			_MapViewerRight.InitializeGeoModel(GeoData.GeoModel);
			_MapViewerRight.Sett.LoadFromFile(Paths.SETTINGS_FILE_RIGHT);
			_MapViewerRight.OnMapBoundsChanged = (zoom, center) =>
			{
				_MapViewerLeft.CenterAndZoom(zoom, center.X, center.Y);
				_zoomControl.SelectedItemIndex = (int)zoom;
				_zoomControl.Invalidate();
			};
			_MapViewerRight.OnMouseLocationPxChanged = location =>
			{
				if (!CrosshairToolStripMenuItem.Checked) return;
				if (_MapViewerRight.Sett.DrawingState == MapSettings.MapDrawingState.AtRest)
					_MapViewerLeft.CrosshairChanged(location);
			};
			_MapViewerRight.OnScrollWithModifierKey = (key, forwards) =>
			{
				_YearControlRight.SelectedItemIndex += forwards ? 1 : -1;
			};

			_zoomControl.SetColors(ItemListControl.BackgroundColorsBlue, ItemListControl.TextColorsBlue);
			_YearControlLeft.SetColors(ItemListControl.BackgroundColorsRed, ItemListControl.TextColorsRed);
			_YearControlRight.SetColors(ItemListControl.BackgroundColorsRed, ItemListControl.TextColorsRed);

			_zoomControl.Items = MapSettings.ZOOMS.Select(zoom => new StringWithTag<uint>(zoom.Level, $"Zoom {zoom.Level}", GetZoomDescription(zoom))).ToList();
			List<StringWithTag<uint>> yearItemList = GeoData.GeoModel.Years
				.OrderBy(year => year)
				.Select(year => new StringWithTag<uint>(year, $"{year}", Datasets.GetInfoByYear(year).YearDescription))
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

		public static string GetZoomDescription(MapSettings.Zoom zoom)
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
			TaskDialog.ShowDialog(new()
			{
				Caption = $"About MA41Data Viewer",
				Heading = "by BogdanBBA",
				Text = $"December 2021 - January 2022{Environment.NewLine}October 2022{Environment.NewLine}January - February 2025",
				Buttons = { new("Awesome!") }
			}); // https://www.wien.gv.at/stadtentwicklung/stadtvermessung/service/luftarchiv.html
		}

		private void ShowInExplorerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Process.Start("explorer.exe", Paths.ROOT_FOLDER);
		}

		private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void ToggleSecondViewToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ToggleSecondViewToolStripMenuItem.Checked = !ToggleSecondViewToolStripMenuItem.Checked;
			FViewer_Resize(sender, e);
		}

		private void CrosshairToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CrosshairToolStripMenuItem.Checked = !CrosshairToolStripMenuItem.Checked;
			if (!CrosshairToolStripMenuItem.Checked)
			{
				_MapViewerLeft.CrosshairChanged(null);
				_MapViewerRight.CrosshairChanged(null);
			}
		}

		private void Location_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int toolstripItemTag = int.Parse((sender as ToolStripMenuItem).Tag as string);
			Tuple<uint, float, float> location = DEFAULT_LOCATIONS[toolstripItemTag];
			_MapViewerLeft.CenterAndZoom(location.Item1, location.Item2, location.Item3);
			_MapViewerRight.CenterAndZoom(location.Item1, location.Item2, location.Item3);
		}

		private void DatasetInfoleftToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ShowDatasetInfo(_MapViewerLeft.Sett.Year.GetValueOrDefault(0));
		}

		private void DatasetInforightToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ShowDatasetInfo(_MapViewerRight.Sett.Year.GetValueOrDefault(0));
		}

		private static void ShowDatasetInfo(uint year)
		{
			Datasets.DatasetInfo data = Datasets.GetInfoByYear(year);

			TaskDialogButton openUrlButton = new("Open URL");
			openUrlButton.Click += (_, _) => Process.Start(new ProcessStartInfo { FileName = data.URL, UseShellExecute = true });
			TaskDialog.ShowDialog(new()
			{
				Caption = $"Dataset info {data.Year}",
				Heading = $"Year: {data.Year} ({data.YearDescription})",
				Text = $"{data.DatasetDescription}{Environment.NewLine}{Environment.NewLine}{data.URL}",
				Buttons = { openUrlButton, new("Close") }
			});
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
			foreach (ToolStripMenuItem iItem in new[] { lowToolStripMenuItem, mediumToolStripMenuItem, highToolStripMenuItem })
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
			new FExport().Export_OneView_OneYear(this, _MapViewerLeft, $@"{Paths.EXPORTS_FOLDER}\{DateTime.Now:yyyyMMdd_HHmmss}.png");
		}

		private void CurrentViewallYearsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new FExport().Export_OneView_AllYears(this, _MapViewerLeft, @$"{Paths.EXPORTS_FOLDER}\{DateTime.Now:yyyyMMdd_HHmmss}.png");
		}

		private void CurrentViewsbothToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new FExport().Export_TwoViews_OneYear(this, _MapViewerLeft, _MapViewerRight, @$"{Paths.EXPORTS_FOLDER}\{DateTime.Now:yyyyMMdd_HHmmss}.png");
		}
		#endregion

	}
}
