
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
			this._MapViewer = new MA41Viewer.UI.Controls.MapViewer();
			this.YearFLP = new System.Windows.Forms.FlowLayoutPanel();
			this.YearHeaderL = new System.Windows.Forms.Label();
			this.ZoomLevelTrB = new System.Windows.Forms.TrackBar();
			this.ZoomHeaderL = new System.Windows.Forms.Label();
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
			((System.ComponentModel.ISupportInitialize)(this.ZoomLevelTrB)).BeginInit();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// _MapViewer
			// 
			this._MapViewer.Location = new System.Drawing.Point(155, 27);
			this._MapViewer.Name = "_MapViewer";
			this._MapViewer.Size = new System.Drawing.Size(241, 99);
			this._MapViewer.TabIndex = 0;
			// 
			// YearFLP
			// 
			this.YearFLP.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.YearFLP.Location = new System.Drawing.Point(12, 67);
			this.YearFLP.Name = "YearFLP";
			this.YearFLP.Size = new System.Drawing.Size(137, 310);
			this.YearFLP.TabIndex = 1;
			// 
			// YearHeaderL
			// 
			this.YearHeaderL.AutoSize = true;
			this.YearHeaderL.Font = new System.Drawing.Font("Segoe UI Semilight", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.YearHeaderL.Location = new System.Drawing.Point(12, 27);
			this.YearHeaderL.Name = "YearHeaderL";
			this.YearHeaderL.Size = new System.Drawing.Size(51, 30);
			this.YearHeaderL.TabIndex = 2;
			this.YearHeaderL.Text = "Year";
			// 
			// ZoomLevelTrB
			// 
			this.ZoomLevelTrB.Cursor = System.Windows.Forms.Cursors.SizeNS;
			this.ZoomLevelTrB.Location = new System.Drawing.Point(12, 420);
			this.ZoomLevelTrB.Maximum = 12;
			this.ZoomLevelTrB.Name = "ZoomLevelTrB";
			this.ZoomLevelTrB.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.ZoomLevelTrB.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.ZoomLevelTrB.RightToLeftLayout = true;
			this.ZoomLevelTrB.Size = new System.Drawing.Size(45, 137);
			this.ZoomLevelTrB.TabIndex = 4;
			this.ZoomLevelTrB.Scroll += new System.EventHandler(this.ZoomLevelTrB_Scroll);
			// 
			// ZoomHeaderL
			// 
			this.ZoomHeaderL.AutoSize = true;
			this.ZoomHeaderL.Font = new System.Drawing.Font("Segoe UI Semilight", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.ZoomHeaderL.Location = new System.Drawing.Point(12, 380);
			this.ZoomHeaderL.Name = "ZoomHeaderL";
			this.ZoomHeaderL.Size = new System.Drawing.Size(67, 30);
			this.ZoomHeaderL.TabIndex = 5;
			this.ZoomHeaderL.Text = "Zoom";
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
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// showInExplorerToolStripMenuItem
			// 
			this.showInExplorerToolStripMenuItem.Name = "showInExplorerToolStripMenuItem";
			this.showInExplorerToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
			this.showInExplorerToolStripMenuItem.Text = "Show in Explorer";
			this.showInExplorerToolStripMenuItem.Click += new System.EventHandler(this.showInExplorerToolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
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
			this.debugONOFFToolStripMenuItem.Click += new System.EventHandler(this.debugONOFFToolStripMenuItem_Click);
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
			this.lowToolStripMenuItem.Click += new System.EventHandler(this.lowToolStripMenuItem_Click);
			// 
			// mediumToolStripMenuItem
			// 
			this.mediumToolStripMenuItem.Name = "mediumToolStripMenuItem";
			this.mediumToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
			this.mediumToolStripMenuItem.Text = "Medium";
			this.mediumToolStripMenuItem.Click += new System.EventHandler(this.mediumToolStripMenuItem_Click);
			// 
			// highToolStripMenuItem
			// 
			this.highToolStripMenuItem.Name = "highToolStripMenuItem";
			this.highToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
			this.highToolStripMenuItem.Text = "High";
			this.highToolStripMenuItem.Click += new System.EventHandler(this.highToolStripMenuItem_Click);
			// 
			// informationShownToolStripMenuItem
			// 
			this.informationShownToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mouseCursorInfoToolStripMenuItem,
            this.drawingQualityInfoToolStripMenuItem,
            this.memoryAllocationInfoToolStripMenuItem});
			this.informationShownToolStripMenuItem.Name = "informationShownToolStripMenuItem";
			this.informationShownToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
			this.informationShownToolStripMenuItem.Text = "Information shown";
			// 
			// mouseCursorInfoToolStripMenuItem
			// 
			this.mouseCursorInfoToolStripMenuItem.Name = "mouseCursorInfoToolStripMenuItem";
			this.mouseCursorInfoToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			this.mouseCursorInfoToolStripMenuItem.Text = "Mouse cursor info";
			this.mouseCursorInfoToolStripMenuItem.Click += new System.EventHandler(this.mouseCursorInfoToolStripMenuItem_Click);
			// 
			// drawingQualityInfoToolStripMenuItem
			// 
			this.drawingQualityInfoToolStripMenuItem.Name = "drawingQualityInfoToolStripMenuItem";
			this.drawingQualityInfoToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			this.drawingQualityInfoToolStripMenuItem.Text = "Drawing quality info";
			this.drawingQualityInfoToolStripMenuItem.Click += new System.EventHandler(this.drawingQualityInfoToolStripMenuItem_Click);
			// 
			// memoryAllocationInfoToolStripMenuItem
			// 
			this.memoryAllocationInfoToolStripMenuItem.Name = "memoryAllocationInfoToolStripMenuItem";
			this.memoryAllocationInfoToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			this.memoryAllocationInfoToolStripMenuItem.Text = "Memory allocation info";
			this.memoryAllocationInfoToolStripMenuItem.Click += new System.EventHandler(this.memoryAllocationInfoToolStripMenuItem_Click);
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
			this.viennaOverviewToolStripMenuItem.Click += new System.EventHandler(this.location_ToolStripMenuItem_Click);
			// 
			// innereStadtToolStripMenuItem
			// 
			this.innereStadtToolStripMenuItem.Name = "innereStadtToolStripMenuItem";
			this.innereStadtToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			this.innereStadtToolStripMenuItem.Tag = "1";
			this.innereStadtToolStripMenuItem.Text = "Innere Stadt";
			this.innereStadtToolStripMenuItem.Click += new System.EventHandler(this.location_ToolStripMenuItem_Click);
			// 
			// hauptbahnhofToolStripMenuItem
			// 
			this.hauptbahnhofToolStripMenuItem.Name = "hauptbahnhofToolStripMenuItem";
			this.hauptbahnhofToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			this.hauptbahnhofToolStripMenuItem.Tag = "2";
			this.hauptbahnhofToolStripMenuItem.Text = "Hauptbahnhof";
			this.hauptbahnhofToolStripMenuItem.Click += new System.EventHandler(this.location_ToolStripMenuItem_Click);
			// 
			// lambrechtgasseToolStripMenuItem
			// 
			this.lambrechtgasseToolStripMenuItem.Name = "lambrechtgasseToolStripMenuItem";
			this.lambrechtgasseToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			this.lambrechtgasseToolStripMenuItem.Tag = "3";
			this.lambrechtgasseToolStripMenuItem.Text = "Lambrechtgasse";
			this.lambrechtgasseToolStripMenuItem.Click += new System.EventHandler(this.location_ToolStripMenuItem_Click);
			// 
			// rittingergasseToolStripMenuItem
			// 
			this.rittingergasseToolStripMenuItem.Name = "rittingergasseToolStripMenuItem";
			this.rittingergasseToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			this.rittingergasseToolStripMenuItem.Tag = "4";
			this.rittingergasseToolStripMenuItem.Text = "Rittingergasse";
			this.rittingergasseToolStripMenuItem.Click += new System.EventHandler(this.location_ToolStripMenuItem_Click);
			// 
			// hietzingPenzingToolStripMenuItem
			// 
			this.hietzingPenzingToolStripMenuItem.Name = "hietzingPenzingToolStripMenuItem";
			this.hietzingPenzingToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			this.hietzingPenzingToolStripMenuItem.Tag = "5";
			this.hietzingPenzingToolStripMenuItem.Text = "Hietzing/Penzing";
			this.hietzingPenzingToolStripMenuItem.Click += new System.EventHandler(this.location_ToolStripMenuItem_Click);
			// 
			// iKEANordToolStripMenuItem
			// 
			this.iKEANordToolStripMenuItem.Name = "iKEANordToolStripMenuItem";
			this.iKEANordToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			this.iKEANordToolStripMenuItem.Tag = "6";
			this.iKEANordToolStripMenuItem.Text = "IKEA Nord";
			this.iKEANordToolStripMenuItem.Click += new System.EventHandler(this.location_ToolStripMenuItem_Click);
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
			this.currentViewToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
			this.currentViewToolStripMenuItem.Text = "Current view";
			this.currentViewToolStripMenuItem.Click += new System.EventHandler(this.currentViewToolStripMenuItem_Click);
			// 
			// currentViewallYearsToolStripMenuItem
			// 
			this.currentViewallYearsToolStripMenuItem.Name = "currentViewallYearsToolStripMenuItem";
			this.currentViewallYearsToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
			this.currentViewallYearsToolStripMenuItem.Text = "Current view (all years)";
			this.currentViewallYearsToolStripMenuItem.Click += new System.EventHandler(this.currentViewallYearsToolStripMenuItem_Click);
			// 
			// FViewer
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1231, 585);
			this.Controls.Add(this.ZoomHeaderL);
			this.Controls.Add(this.ZoomLevelTrB);
			this.Controls.Add(this.YearHeaderL);
			this.Controls.Add(this.YearFLP);
			this.Controls.Add(this._MapViewer);
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
			((System.ComponentModel.ISupportInitialize)(this.ZoomLevelTrB)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private UI.Controls.MapViewer _MapViewer;
		private System.Windows.Forms.FlowLayoutPanel YearFLP;
		private System.Windows.Forms.Label YearHeaderL;
		private System.Windows.Forms.TrackBar ZoomLevelTrB;
		private System.Windows.Forms.Label ZoomHeaderL;
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
	}
}