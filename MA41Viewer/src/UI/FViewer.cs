using MA41Viewer.Data;
using MA41Viewer.UI.Controls;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MA41Viewer.UI
{
	public partial class FViewer : Form
	{
		//private readonly FStartup fStartup;

		public FViewer(/*FStartup fStartup*/)
		{
			//this.fStartup = fStartup;
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
			_MapViewer.InitializeGeoModel(GeoData.GeoModel, zoomLevel => ZoomLevelTrB.Value = (int)zoomLevel);
			ZoomLevelTrB.Maximum = MapViewer.MapSettings.ZOOMS.Length - 1;
			ZoomLevelTrB.Value = (int)_MapViewer.Sett.ZoomLevel;
		}

		private void FViewer_Resize(object sender, EventArgs e)
		{
			_MapViewer.Size = new Size(Width - _MapViewer.Left - 30, Height - _MapViewer.Top - 55);

			YearHeaderL.Location = new Point(10, menuStrip1.Bottom + 10);
			YearFLP.Location = new Point(10, YearHeaderL.Bottom + 10);
			ZoomHeaderL.Location = new Point(10, YearFLP.Bottom + 10);
			DebugModeChB.Location = new Point(YearHeaderL.Left, _MapViewer.Bottom - DebugModeChB.Height);

			ZoomLevelTrB.Bounds = new Rectangle(YearHeaderL.Left + YearFLP.Width / 2 - ZoomLevelTrB.Width / 2 - 10,
				ZoomHeaderL.Bottom + 3, ZoomLevelTrB.Width, Math.Min(250, DebugModeChB.Top - ZoomHeaderL.Bottom - 2 * 3));

			_MapViewer.Invalidate();
		}

		private void InitializeMyComponents()
		{
			DebugModeChB.Checked = _MapViewer.DebugMode;

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
					Text = $"{year}",
					Cursor = Cursors.Hand,
					Font = new Font("Segoe UI", 12, FontStyle.Bold),
					Tag = year,
					AutoSize = true,
					Checked = iYear == GeoData.GeoModel.Years.Length - 1
				};
				rb.Click += yearRB_ClickHandler;
				YearFLP.Controls.Add(rb);
			}

			yearRB_ClickHandler(YearFLP.Controls[0], null);
		}

		private void DebugModeChB_CheckedChanged(object sender, EventArgs e)
		{
			(sender as RadioButton)?.Invalidate();
			_MapViewer.DebugMode = DebugModeChB.Checked;
			_MapViewer.Invalidate();
		}

		private void ZoomLevelTrB_Scroll(object sender, EventArgs e)
		{
			_MapViewer.Sett.ZoomLevel = (uint)ZoomLevelTrB.Value;
			_MapViewer.Sett.CenterMap(_MapViewer.Sett.CurrentMapCoordCenter);
			_MapViewer.Invalidate();
		}

		private void FViewer_KeyDown(object sender, KeyEventArgs e)
		{
			return;

			YearHeaderL.Text = e.KeyData.ToString();
			if ((e.KeyData & Keys.N) == Keys.N)
			{
				var oldIndex = Enumerable.Range(0, YearFLP.Controls.Count).Where(index => (YearFLP.Controls[index] as RadioButton).Checked).First();
				var newIndex = oldIndex + (((e.KeyData & Keys.Shift) == Keys.Shift) ? -1 : 1);
				newIndex = newIndex < 0 ? YearFLP.Controls.Count - 1 : (newIndex >= YearFLP.Controls.Count ? 0 : newIndex);
				//(YearFLP.Controls[oldIndex] as RadioButton).Checked = false;
				DebugModeChB_CheckedChanged(YearFLP.Controls[newIndex], e);
			}
		}

		protected override void OnClosed(EventArgs e)
		{
			//fStartup.Show();
			base.OnClosed(e);
		}
	}
}
