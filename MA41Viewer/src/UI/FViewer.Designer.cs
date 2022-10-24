
namespace MA41Viewer.UI
{
	partial class FViewer
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FViewer));
			this._MapViewerLeft = new MA41Viewer.UI.Controls.MapViewer();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.appToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showInExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.debuggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.debugONOFFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.drawingQualityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.lowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mediumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.highToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.informationShownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mouseCursorInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.drawingQualityInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.memoryAllocationInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.detailedTileInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.locationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viennaOverviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.innereStadtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.hauptbahnhofToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.lambrechtgasseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rittingergasseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.hietzingPenzingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.iKEANordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.currentViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.currentViewallYearsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._MapViewerRight = new MA41Viewer.UI.Controls.MapViewer();
			this._YearControlLeft = new MA41Viewer.src.UI.Controls.ItemListControl();
			this._YearControlRight = new MA41Viewer.src.UI.Controls.ItemListControl();
			this._zoomControl = new MA41Viewer.src.UI.Controls.ItemListControl();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// _MapViewerLeft
			// 
			this._MapViewerLeft.DebugMode = false;
			this._MapViewerLeft.Location = new System.Drawing.Point(155, 27);
			this._MapViewerLeft.OnMapBoundsChanged = null;
			this._MapViewerLeft.Name = "_MapViewerLeft";
			this._MapViewerLeft.Size = new System.Drawing.Size(716, 251);
			this._MapViewerLeft.TabIndex = 0;
			// 
			// menuStrip1
			// 
			this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.appToolStripMenuItem,
            this.debuggingToolStripMenuItem,
            this.locationsToolStripMenuItem,
            this.exportToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(1231, 24);
			this.menuStrip1.TabIndex = 6;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// appToolStripMenuItem
			// 
			this.appToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.showInExplorerToolStripMenuItem,
            this.exitToolStripMenuItem});
			this.appToolStripMenuItem.Name = "appToolStripMenuItem";
			this.appToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
			this.appToolStripMenuItem.Text = "App";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
			this.aboutToolStripMenuItem.Text = "About";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
			// 
			// showInExplorerToolStripMenuItem
			// 
			this.showInExplorerToolStripMenuItem.Name = "showInExplorerToolStripMenuItem";
			this.showInExplorerToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
			this.showInExplorerToolStripMenuItem.Text = "Show in Explorer";
			this.showInExplorerToolStripMenuItem.Click += new System.EventHandler(this.ShowInExplorerToolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
			// 
			// debuggingToolStripMenuItem
			// 
			this.debuggingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.debugONOFFToolStripMenuItem,
            this.drawingQualityToolStripMenuItem,
            this.informationShownToolStripMenuItem});
			this.debuggingToolStripMenuItem.Name = "debuggingToolStripMenuItem";
			this.debuggingToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
			this.debuggingToolStripMenuItem.Text = "Debugging";
			// 
			// debugONOFFToolStripMenuItem
			// 
			this.debugONOFFToolStripMenuItem.Name = "debugONOFFToolStripMenuItem";
			this.debugONOFFToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
			this.debugONOFFToolStripMenuItem.Text = "Debug: ON/OF";
			this.debugONOFFToolStripMenuItem.Click += new System.EventHandler(this.DebugONOFFToolStripMenuItem_Click);
			// 
			// drawingQualityToolStripMenuItem
			// 
			this.drawingQualityToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lowToolStripMenuItem,
            this.mediumToolStripMenuItem,
            this.highToolStripMenuItem});
			this.drawingQualityToolStripMenuItem.Name = "drawingQualityToolStripMenuItem";
			this.drawingQualityToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
			this.drawingQualityToolStripMenuItem.Text = "Drawing quality";
			// 
			// lowToolStripMenuItem
			// 
			this.lowToolStripMenuItem.Name = "lowToolStripMenuItem";
			this.lowToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
			this.lowToolStripMenuItem.Text = "Low";
			this.lowToolStripMenuItem.Click += new System.EventHandler(this.LowToolStripMenuItem_Click);
			// 
			// mediumToolStripMenuItem
			// 
			this.mediumToolStripMenuItem.Name = "mediumToolStripMenuItem";
			this.mediumToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
			this.mediumToolStripMenuItem.Text = "Medium";
			this.mediumToolStripMenuItem.Click += new System.EventHandler(this.MediumToolStripMenuItem_Click);
			// 
			// highToolStripMenuItem
			// 
			this.highToolStripMenuItem.Name = "highToolStripMenuItem";
			this.highToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
			this.highToolStripMenuItem.Text = "High";
			this.highToolStripMenuItem.Click += new System.EventHandler(this.HighToolStripMenuItem_Click);
			// 
			// informationShownToolStripMenuItem
			// 
			this.informationShownToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mouseCursorInfoToolStripMenuItem,
            this.drawingQualityInfoToolStripMenuItem,
            this.memoryAllocationInfoToolStripMenuItem,
            this.detailedTileInfoToolStripMenuItem});
			this.informationShownToolStripMenuItem.Name = "informationShownToolStripMenuItem";
			this.informationShownToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
			this.informationShownToolStripMenuItem.Text = "Information shown";
			// 
			// mouseCursorInfoToolStripMenuItem
			// 
			this.mouseCursorInfoToolStripMenuItem.Name = "mouseCursorInfoToolStripMenuItem";
			this.mouseCursorInfoToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			this.mouseCursorInfoToolStripMenuItem.Text = "Mouse cursor info";
			this.mouseCursorInfoToolStripMenuItem.Click += new System.EventHandler(this.MouseCursorInfoToolStripMenuItem_Click);
			// 
			// drawingQualityInfoToolStripMenuItem
			// 
			this.drawingQualityInfoToolStripMenuItem.Name = "drawingQualityInfoToolStripMenuItem";
			this.drawingQualityInfoToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			this.drawingQualityInfoToolStripMenuItem.Text = "Drawing quality info";
			this.drawingQualityInfoToolStripMenuItem.Click += new System.EventHandler(this.DrawingQualityInfoToolStripMenuItem_Click);
			// 
			// memoryAllocationInfoToolStripMenuItem
			// 
			this.memoryAllocationInfoToolStripMenuItem.Name = "memoryAllocationInfoToolStripMenuItem";
			this.memoryAllocationInfoToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			this.memoryAllocationInfoToolStripMenuItem.Text = "Memory allocation info";
			this.memoryAllocationInfoToolStripMenuItem.Click += new System.EventHandler(this.MemoryAllocationInfoToolStripMenuItem_Click);
			// 
			// detailedTileInfoToolStripMenuItem
			// 
			this.detailedTileInfoToolStripMenuItem.Name = "detailedTileInfoToolStripMenuItem";
			this.detailedTileInfoToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			this.detailedTileInfoToolStripMenuItem.Text = "Detailed tile info";
			this.detailedTileInfoToolStripMenuItem.Click += new System.EventHandler(this.DetailedTileInfoToolStripMenuItem_Click);
			// 
			// locationsToolStripMenuItem
			// 
			this.locationsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viennaOverviewToolStripMenuItem,
            this.innereStadtToolStripMenuItem,
            this.hauptbahnhofToolStripMenuItem,
            this.lambrechtgasseToolStripMenuItem,
            this.rittingergasseToolStripMenuItem,
            this.hietzingPenzingToolStripMenuItem,
            this.iKEANordToolStripMenuItem});
			this.locationsToolStripMenuItem.Name = "locationsToolStripMenuItem";
			this.locationsToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
			this.locationsToolStripMenuItem.Text = "Locations";
			// 
			// viennaOverviewToolStripMenuItem
			// 
			this.viennaOverviewToolStripMenuItem.Name = "viennaOverviewToolStripMenuItem";
			this.viennaOverviewToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			this.viennaOverviewToolStripMenuItem.Tag = "0";
			this.viennaOverviewToolStripMenuItem.Text = "Vienna overview";
			this.viennaOverviewToolStripMenuItem.Click += new System.EventHandler(this.Location_ToolStripMenuItem_Click);
			// 
			// innereStadtToolStripMenuItem
			// 
			this.innereStadtToolStripMenuItem.Name = "innereStadtToolStripMenuItem";
			this.innereStadtToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			this.innereStadtToolStripMenuItem.Tag = "1";
			this.innereStadtToolStripMenuItem.Text = "Innere Stadt";
			this.innereStadtToolStripMenuItem.Click += new System.EventHandler(this.Location_ToolStripMenuItem_Click);
			// 
			// hauptbahnhofToolStripMenuItem
			// 
			this.hauptbahnhofToolStripMenuItem.Name = "hauptbahnhofToolStripMenuItem";
			this.hauptbahnhofToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			this.hauptbahnhofToolStripMenuItem.Tag = "2";
			this.hauptbahnhofToolStripMenuItem.Text = "Hauptbahnhof";
			this.hauptbahnhofToolStripMenuItem.Click += new System.EventHandler(this.Location_ToolStripMenuItem_Click);
			// 
			// lambrechtgasseToolStripMenuItem
			// 
			this.lambrechtgasseToolStripMenuItem.Name = "lambrechtgasseToolStripMenuItem";
			this.lambrechtgasseToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			this.lambrechtgasseToolStripMenuItem.Tag = "3";
			this.lambrechtgasseToolStripMenuItem.Text = "Lambrechtgasse";
			this.lambrechtgasseToolStripMenuItem.Click += new System.EventHandler(this.Location_ToolStripMenuItem_Click);
			// 
			// rittingergasseToolStripMenuItem
			// 
			this.rittingergasseToolStripMenuItem.Name = "rittingergasseToolStripMenuItem";
			this.rittingergasseToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			this.rittingergasseToolStripMenuItem.Tag = "4";
			this.rittingergasseToolStripMenuItem.Text = "Rittingergasse";
			this.rittingergasseToolStripMenuItem.Click += new System.EventHandler(this.Location_ToolStripMenuItem_Click);
			// 
			// hietzingPenzingToolStripMenuItem
			// 
			this.hietzingPenzingToolStripMenuItem.Name = "hietzingPenzingToolStripMenuItem";
			this.hietzingPenzingToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			this.hietzingPenzingToolStripMenuItem.Tag = "5";
			this.hietzingPenzingToolStripMenuItem.Text = "Hietzing/Penzing";
			this.hietzingPenzingToolStripMenuItem.Click += new System.EventHandler(this.Location_ToolStripMenuItem_Click);
			// 
			// iKEANordToolStripMenuItem
			// 
			this.iKEANordToolStripMenuItem.Name = "iKEANordToolStripMenuItem";
			this.iKEANordToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			this.iKEANordToolStripMenuItem.Tag = "6";
			this.iKEANordToolStripMenuItem.Text = "IKEA Nord";
			this.iKEANordToolStripMenuItem.Click += new System.EventHandler(this.Location_ToolStripMenuItem_Click);
			// 
			// exportToolStripMenuItem
			// 
			this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.currentViewToolStripMenuItem,
            this.currentViewallYearsToolStripMenuItem});
			this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
			this.exportToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
			this.exportToolStripMenuItem.Text = "Export";
			// 
			// currentViewToolStripMenuItem
			// 
			this.currentViewToolStripMenuItem.Name = "currentViewToolStripMenuItem";
			this.currentViewToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
			this.currentViewToolStripMenuItem.Text = "Current (left) view";
			this.currentViewToolStripMenuItem.Click += new System.EventHandler(this.CurrentViewToolStripMenuItem_Click);
			// 
			// currentViewallYearsToolStripMenuItem
			// 
			this.currentViewallYearsToolStripMenuItem.Name = "currentViewallYearsToolStripMenuItem";
			this.currentViewallYearsToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
			this.currentViewallYearsToolStripMenuItem.Text = "Current (left) view (all years)";
			this.currentViewallYearsToolStripMenuItem.Click += new System.EventHandler(this.CurrentViewallYearsToolStripMenuItem_Click);
			// 
			// _MapViewerRight
			// 
			this._MapViewerRight.DebugMode = false;
			this._MapViewerRight.Location = new System.Drawing.Point(324, 219);
			this._MapViewerRight.OnMapBoundsChanged = null;
			this._MapViewerRight.Name = "_MapViewerRight";
			this._MapViewerRight.Size = new System.Drawing.Size(716, 251);
			this._MapViewerRight.TabIndex = 8;
			// 
			// _YearControlLeft
			// 
			this._YearControlLeft.Location = new System.Drawing.Point(275, 128);
			this._YearControlLeft.Name = "_YearControlLeft";
			this._YearControlLeft.Orientation = MA41Viewer.src.UI.Controls.ItemListControl.Orientations.Horizontal;
			this._YearControlLeft.SelectedItemIndex = 1;
			this._YearControlLeft.Size = new System.Drawing.Size(568, 55);
			this._YearControlLeft.TabIndex = 9;
			this._YearControlLeft.TextFont = new System.Drawing.Font("Roboto Slab", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this._YearControlLeft.OnSelectedItemChanged = null;
			// 
			// _YearControlRight
			// 
			this._YearControlRight.Location = new System.Drawing.Point(429, 334);
			this._YearControlRight.Name = "_YearControlRight";
			this._YearControlRight.Orientation = MA41Viewer.src.UI.Controls.ItemListControl.Orientations.Horizontal;
			this._YearControlRight.SelectedItemIndex = 1;
			this._YearControlRight.Size = new System.Drawing.Size(568, 55);
			this._YearControlRight.TabIndex = 10;
			this._YearControlRight.TextFont = new System.Drawing.Font("Roboto Slab", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this._YearControlRight.OnSelectedItemChanged = null;
			// 
			// _zoomControl
			// 
			this._zoomControl.Location = new System.Drawing.Point(22, 219);
			this._zoomControl.Name = "_zoomControl";
			this._zoomControl.Orientation = MA41Viewer.src.UI.Controls.ItemListControl.Orientations.Vertical;
			this._zoomControl.SelectedItemIndex = 2;
			this._zoomControl.Size = new System.Drawing.Size(88, 253);
			this._zoomControl.TabIndex = 11;
			this._zoomControl.TextFont = new System.Drawing.Font("Roboto Slab", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this._zoomControl.OnSelectedItemChanged = null;
			// 
			// FViewer
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1231, 585);
			this.Controls.Add(this._zoomControl);
			this.Controls.Add(this._YearControlRight);
			this.Controls.Add(this._YearControlLeft);
			this.Controls.Add(this._MapViewerRight);
			this.Controls.Add(this._MapViewerLeft);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MainMenuStrip = this.menuStrip1;
			this.MinimumSize = new System.Drawing.Size(800, 600);
			this.Name = "FViewer";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Wien MA41 Historical Aerial Imagery Viewer";
			this.Load += new System.EventHandler(this.FViewer_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FViewer_KeyDown);
			this.Resize += new System.EventHandler(this.FViewer_Resize);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private UI.Controls.MapViewer _MapViewerLeft;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem appToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem locationsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viennaOverviewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem innereStadtToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem hauptbahnhofToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem lambrechtgasseToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem rittingergasseToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem hietzingPenzingToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem iKEANordToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem debuggingToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem debugONOFFToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem drawingQualityToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem lowToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mediumToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem highToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem informationShownToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mouseCursorInfoToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem drawingQualityInfoToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem memoryAllocationInfoToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem currentViewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem currentViewallYearsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showInExplorerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem detailedTileInfoToolStripMenuItem;
		private Controls.MapViewer _MapViewerRight;
		private src.UI.Controls.ItemListControl _YearControlLeft;
		private src.UI.Controls.ItemListControl _YearControlRight;
		private src.UI.Controls.ItemListControl _zoomControl;
	}
}