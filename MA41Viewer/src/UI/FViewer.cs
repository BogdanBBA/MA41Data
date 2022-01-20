using MA41.Commons;
using MA41Viewer.Data;
using MA41Viewer.UI.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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

			_MapViewer.InitializeGeoModel(GeoData.GeoModel, zoomLevel => ZoomLevelTrB.Value = (int)zoomLevel);
			_MapViewer.Sett.LoadFromFile(Paths.SETTINGS_FILE);
			InitializeMyComponents();
		}

		private void FViewer_Resize(object sender, EventArgs e)
		{
			_MapViewer.Size = new Size(Width - _MapViewer.Left - 30, Height - _MapViewer.Top - 55);

			YearHeaderL.Location = new Point(10, menuStrip1.Bottom + 10);
			YearFLP.Location = new Point(10, YearHeaderL.Bottom + 10);
			ZoomHeaderL.Location = new Point(10, YearFLP.Bottom + 10);
			ZoomLevelTrB.Bounds = new Rectangle(YearHeaderL.Left + YearFLP.Width / 2 - ZoomLevelTrB.Width / 2 - 10, ZoomHeaderL.Bottom + 3, ZoomLevelTrB.Width, _MapViewer.Bottom - ZoomHeaderL.Bottom);

			_MapViewer.Invalidate();
		}

		private void InitializeMyComponents()
		{
			debugONOFFToolStripMenuItem.Checked = _MapViewer.DebugMode;

			void yearRB_ClickHandler(object sender, EventArgs e)
			{
				_MapViewer.Sett.Year = (uint)(sender as RadioButton).Tag;
				_MapViewer.Invalidate();
			}

			for (int iYear = GeoData.GeoModel.Years.Length - 1; iYear >= 0; iYear--)
			{
				uint year = GeoData.GeoModel.Years[iYear];
				var rb = new RadioButton()
				{
					Name = $"year{year}",
					Text = $"{year}",
					Cursor = Cursors.Hand,
					Font = new Font("Segoe UI", 12, FontStyle.Bold),
					Tag = year,
					AutoSize = true,
					Checked = year == _MapViewer.Sett.Year
				};
				rb.Click += yearRB_ClickHandler;
				YearFLP.Controls.Add(rb);
			}

			ZoomLevelTrB.Maximum = MapSettings.ZOOMS.Length - 1;
			ZoomLevelTrB.Value = (int)_MapViewer.Sett.ZoomLevel;

			// default drawing quality setting should be HIGH
			setDrawingQualityToolstripItemChecked(highToolStripMenuItem);

			mouseCursorInfoToolStripMenuItem.Checked = _MapViewer.Sett.CurrentDebugInfoShown.MouseCursorInfo;
			drawingQualityInfoToolStripMenuItem.Checked = _MapViewer.Sett.CurrentDebugInfoShown.DrawingQualityInfo;
			memoryAllocationInfoToolStripMenuItem.Checked = _MapViewer.Sett.CurrentDebugInfoShown.MemoryAllocationInfo;
		}

		private void ZoomLevelTrB_Scroll(object sender, EventArgs e)
		{
			_MapViewer.Sett.ZoomLevel = (uint)ZoomLevelTrB.Value;
			_MapViewer.Sett.CenterMap(_MapViewer.Sett.GetCurrentMapCoordCenter());
			_MapViewer.Invalidate();
		}

		private void FViewer_KeyDown(object sender, KeyEventArgs e)
		{
			return;

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
			_MapViewer.Sett.SaveToFile(Paths.SETTINGS_FILE);
			base.OnClosed(e);
		}

		#region Main menu events
		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show($"by BogdanBBA{Environment.NewLine}{Environment.NewLine}December 2021 - January 2022");
		}

		private void showInExplorerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("explorer.exe", Paths.ROOT_FOLDER);
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void location_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var toolstripItemTag = int.Parse((sender as ToolStripMenuItem).Tag as string);
			var location = DEFAULT_LOCATIONS[toolstripItemTag];
			_MapViewer.CenterAndZoom(location.Item1, location.Item2, location.Item3);
		}

		private void debugONOFFToolStripMenuItem_Click(object sender, EventArgs e)
		{
			debugONOFFToolStripMenuItem.Checked = !debugONOFFToolStripMenuItem.Checked;
			_MapViewer.DebugMode = debugONOFFToolStripMenuItem.Checked;
			_MapViewer.Invalidate();
		}

		private void setDrawingQualityToolstripItemChecked(ToolStripItem item)
		{
			foreach (var iItem in new[] { lowToolStripMenuItem, mediumToolStripMenuItem, highToolStripMenuItem })
			{
				iItem.Checked = iItem.Equals(item);
			}
		}

		private void lowToolStripMenuItem_Click(object sender, EventArgs e)
		{
			setDrawingQualityToolstripItemChecked(sender as ToolStripItem);
			_MapViewer.Sett.CurrentQualitySettings.SetFrom(MapSettings.QualitySettings.LOW);
			_MapViewer.Invalidate();
		}

		private void mediumToolStripMenuItem_Click(object sender, EventArgs e)
		{
			setDrawingQualityToolstripItemChecked(sender as ToolStripItem);
			_MapViewer.Sett.CurrentQualitySettings.SetFrom(MapSettings.QualitySettings.MEDIUM);
			_MapViewer.Invalidate();
		}

		private void highToolStripMenuItem_Click(object sender, EventArgs e)
		{
			setDrawingQualityToolstripItemChecked(sender as ToolStripItem);
			_MapViewer.Sett.CurrentQualitySettings.SetFrom(MapSettings.QualitySettings.HIGH);
			_MapViewer.Invalidate();
		}

		private void mouseCursorInfoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			mouseCursorInfoToolStripMenuItem.Checked = !mouseCursorInfoToolStripMenuItem.Checked;
			_MapViewer.Sett.CurrentDebugInfoShown.MouseCursorInfo = mouseCursorInfoToolStripMenuItem.Checked;
			_MapViewer.Invalidate();
		}

		private void drawingQualityInfoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			drawingQualityInfoToolStripMenuItem.Checked = !drawingQualityInfoToolStripMenuItem.Checked;
			_MapViewer.Sett.CurrentDebugInfoShown.DrawingQualityInfo = drawingQualityInfoToolStripMenuItem.Checked;
			_MapViewer.Invalidate();
		}

		private void memoryAllocationInfoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			memoryAllocationInfoToolStripMenuItem.Checked = !memoryAllocationInfoToolStripMenuItem.Checked;
			_MapViewer.Sett.CurrentDebugInfoShown.MemoryAllocationInfo = memoryAllocationInfoToolStripMenuItem.Checked;
			_MapViewer.Invalidate();
		}

		private void currentViewToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string filename = @$"{Paths.EXPORTS_FOLDER}\{DateTime.Now:yyyyMMdd_HHmmss}.png";
			_MapViewer.SaveCurrentViewToFile(filename);
			Process.Start("explorer.exe", $"/select, \"{filename}\"");
		}

		private void currentViewallYearsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string filename = @$"{Paths.EXPORTS_FOLDER}\{DateTime.Now:yyyyMMdd_HHmmss}.png";
			_MapViewer.SaveAllYearsToFile(filename);
			Process.Start("explorer.exe", $"/select, \"{filename}\"");
		}
		#endregion
	}
}
