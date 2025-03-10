﻿
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
			_MapViewerLeft = new Controls.MapViewer();
			menuStrip1 = new System.Windows.Forms.MenuStrip();
			appToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			showInExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			debuggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			ToggleSecondViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			CrosshairToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			debugONOFFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			drawingQualityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			lowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			mediumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			highToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			informationShownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			mouseCursorInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			drawingQualityInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			memoryAllocationInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			detailedTileInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			locationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			viennaOverviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			innereStadtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			hauptbahnhofToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			lambrechtgasseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			rittingergasseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			hietzingPenzingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			iKEANordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			DatasetInfoleftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			DatasetInforightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			currentViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			currentViewallYearsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			currentViewsbothToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			_MapViewerRight = new Controls.MapViewer();
			_YearControlLeft = new Controls.ItemListControl();
			_YearControlRight = new Controls.ItemListControl();
			_zoomControl = new Controls.ItemListControl();
			menuStrip1.SuspendLayout();
			SuspendLayout();
			// 
			// _MapViewerLeft
			// 
			_MapViewerLeft.DebugMode = false;
			_MapViewerLeft.Location = new System.Drawing.Point(155, 27);
			_MapViewerLeft.Name = "_MapViewerLeft";
			_MapViewerLeft.OnMapBoundsChanged = null;
			_MapViewerLeft.OnMouseLocationPxChanged = null;
			_MapViewerLeft.OnScrollWithModifierKey = null;
			_MapViewerLeft.Size = new System.Drawing.Size(716, 251);
			_MapViewerLeft.TabIndex = 0;
			// 
			// menuStrip1
			// 
			menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { appToolStripMenuItem, debuggingToolStripMenuItem, locationsToolStripMenuItem, exportToolStripMenuItem });
			menuStrip1.Location = new System.Drawing.Point(0, 0);
			menuStrip1.Name = "menuStrip1";
			menuStrip1.Size = new System.Drawing.Size(1136, 24);
			menuStrip1.TabIndex = 6;
			menuStrip1.Text = "menuStrip1";
			// 
			// appToolStripMenuItem
			// 
			appToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { aboutToolStripMenuItem, showInExplorerToolStripMenuItem, toolStripSeparator2, exitToolStripMenuItem });
			appToolStripMenuItem.Name = "appToolStripMenuItem";
			appToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
			appToolStripMenuItem.Text = "App";
			// 
			// aboutToolStripMenuItem
			// 
			aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			aboutToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			aboutToolStripMenuItem.Text = "About";
			aboutToolStripMenuItem.Click += AboutToolStripMenuItem_Click;
			// 
			// showInExplorerToolStripMenuItem
			// 
			showInExplorerToolStripMenuItem.Name = "showInExplorerToolStripMenuItem";
			showInExplorerToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			showInExplorerToolStripMenuItem.Text = "Show in Explorer";
			showInExplorerToolStripMenuItem.Click += ShowInExplorerToolStripMenuItem_Click;
			// 
			// toolStripSeparator2
			// 
			toolStripSeparator2.Name = "toolStripSeparator2";
			toolStripSeparator2.Size = new System.Drawing.Size(158, 6);
			// 
			// exitToolStripMenuItem
			// 
			exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			exitToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			exitToolStripMenuItem.Text = "Exit";
			exitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
			// 
			// debuggingToolStripMenuItem
			// 
			debuggingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ToggleSecondViewToolStripMenuItem, toolStripSeparator4, CrosshairToolStripMenuItem, toolStripSeparator3, debugONOFFToolStripMenuItem, drawingQualityToolStripMenuItem, informationShownToolStripMenuItem });
			debuggingToolStripMenuItem.Name = "debuggingToolStripMenuItem";
			debuggingToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
			debuggingToolStripMenuItem.Text = "Settings";
			// 
			// ToggleSecondViewToolStripMenuItem
			// 
			ToggleSecondViewToolStripMenuItem.Name = "ToggleSecondViewToolStripMenuItem";
			ToggleSecondViewToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
			ToggleSecondViewToolStripMenuItem.Text = "Toggle second view";
			ToggleSecondViewToolStripMenuItem.Click += ToggleSecondViewToolStripMenuItem_Click;
			// 
			// toolStripSeparator4
			// 
			toolStripSeparator4.Name = "toolStripSeparator4";
			toolStripSeparator4.Size = new System.Drawing.Size(232, 6);
			// 
			// CrosshairToolStripMenuItem
			// 
			CrosshairToolStripMenuItem.Name = "CrosshairToolStripMenuItem";
			CrosshairToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
			CrosshairToolStripMenuItem.Text = "Toggle crosshair on other view";
			CrosshairToolStripMenuItem.Click += CrosshairToolStripMenuItem_Click;
			// 
			// toolStripSeparator3
			// 
			toolStripSeparator3.Name = "toolStripSeparator3";
			toolStripSeparator3.Size = new System.Drawing.Size(232, 6);
			// 
			// debugONOFFToolStripMenuItem
			// 
			debugONOFFToolStripMenuItem.Name = "debugONOFFToolStripMenuItem";
			debugONOFFToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
			debugONOFFToolStripMenuItem.Text = "Debug: ON/OF";
			debugONOFFToolStripMenuItem.Click += DebugONOFFToolStripMenuItem_Click;
			// 
			// drawingQualityToolStripMenuItem
			// 
			drawingQualityToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { lowToolStripMenuItem, mediumToolStripMenuItem, highToolStripMenuItem });
			drawingQualityToolStripMenuItem.Name = "drawingQualityToolStripMenuItem";
			drawingQualityToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
			drawingQualityToolStripMenuItem.Text = "Drawing quality";
			// 
			// lowToolStripMenuItem
			// 
			lowToolStripMenuItem.Name = "lowToolStripMenuItem";
			lowToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
			lowToolStripMenuItem.Text = "Low";
			lowToolStripMenuItem.Click += LowToolStripMenuItem_Click;
			// 
			// mediumToolStripMenuItem
			// 
			mediumToolStripMenuItem.Name = "mediumToolStripMenuItem";
			mediumToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
			mediumToolStripMenuItem.Text = "Medium";
			mediumToolStripMenuItem.Click += MediumToolStripMenuItem_Click;
			// 
			// highToolStripMenuItem
			// 
			highToolStripMenuItem.Name = "highToolStripMenuItem";
			highToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
			highToolStripMenuItem.Text = "High";
			highToolStripMenuItem.Click += HighToolStripMenuItem_Click;
			// 
			// informationShownToolStripMenuItem
			// 
			informationShownToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { mouseCursorInfoToolStripMenuItem, drawingQualityInfoToolStripMenuItem, memoryAllocationInfoToolStripMenuItem, detailedTileInfoToolStripMenuItem });
			informationShownToolStripMenuItem.Name = "informationShownToolStripMenuItem";
			informationShownToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
			informationShownToolStripMenuItem.Text = "Information shown";
			// 
			// mouseCursorInfoToolStripMenuItem
			// 
			mouseCursorInfoToolStripMenuItem.Name = "mouseCursorInfoToolStripMenuItem";
			mouseCursorInfoToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			mouseCursorInfoToolStripMenuItem.Text = "Mouse cursor info";
			mouseCursorInfoToolStripMenuItem.Click += MouseCursorInfoToolStripMenuItem_Click;
			// 
			// drawingQualityInfoToolStripMenuItem
			// 
			drawingQualityInfoToolStripMenuItem.Name = "drawingQualityInfoToolStripMenuItem";
			drawingQualityInfoToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			drawingQualityInfoToolStripMenuItem.Text = "Drawing quality info";
			drawingQualityInfoToolStripMenuItem.Click += DrawingQualityInfoToolStripMenuItem_Click;
			// 
			// memoryAllocationInfoToolStripMenuItem
			// 
			memoryAllocationInfoToolStripMenuItem.Name = "memoryAllocationInfoToolStripMenuItem";
			memoryAllocationInfoToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			memoryAllocationInfoToolStripMenuItem.Text = "Memory allocation info";
			memoryAllocationInfoToolStripMenuItem.Click += MemoryAllocationInfoToolStripMenuItem_Click;
			// 
			// detailedTileInfoToolStripMenuItem
			// 
			detailedTileInfoToolStripMenuItem.Name = "detailedTileInfoToolStripMenuItem";
			detailedTileInfoToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			detailedTileInfoToolStripMenuItem.Text = "Detailed tile info";
			detailedTileInfoToolStripMenuItem.Click += DetailedTileInfoToolStripMenuItem_Click;
			// 
			// locationsToolStripMenuItem
			// 
			locationsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { viennaOverviewToolStripMenuItem, innereStadtToolStripMenuItem, hauptbahnhofToolStripMenuItem, lambrechtgasseToolStripMenuItem, rittingergasseToolStripMenuItem, hietzingPenzingToolStripMenuItem, iKEANordToolStripMenuItem, toolStripSeparator1, DatasetInfoleftToolStripMenuItem, DatasetInforightToolStripMenuItem });
			locationsToolStripMenuItem.Name = "locationsToolStripMenuItem";
			locationsToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
			locationsToolStripMenuItem.Text = "Locations";
			// 
			// viennaOverviewToolStripMenuItem
			// 
			viennaOverviewToolStripMenuItem.Name = "viennaOverviewToolStripMenuItem";
			viennaOverviewToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
			viennaOverviewToolStripMenuItem.Tag = "0";
			viennaOverviewToolStripMenuItem.Text = "Vienna overview";
			viennaOverviewToolStripMenuItem.Click += Location_ToolStripMenuItem_Click;
			// 
			// innereStadtToolStripMenuItem
			// 
			innereStadtToolStripMenuItem.Name = "innereStadtToolStripMenuItem";
			innereStadtToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
			innereStadtToolStripMenuItem.Tag = "1";
			innereStadtToolStripMenuItem.Text = "Innere Stadt";
			innereStadtToolStripMenuItem.Click += Location_ToolStripMenuItem_Click;
			// 
			// hauptbahnhofToolStripMenuItem
			// 
			hauptbahnhofToolStripMenuItem.Name = "hauptbahnhofToolStripMenuItem";
			hauptbahnhofToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
			hauptbahnhofToolStripMenuItem.Tag = "2";
			hauptbahnhofToolStripMenuItem.Text = "Hauptbahnhof";
			hauptbahnhofToolStripMenuItem.Click += Location_ToolStripMenuItem_Click;
			// 
			// lambrechtgasseToolStripMenuItem
			// 
			lambrechtgasseToolStripMenuItem.Name = "lambrechtgasseToolStripMenuItem";
			lambrechtgasseToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
			lambrechtgasseToolStripMenuItem.Tag = "3";
			lambrechtgasseToolStripMenuItem.Text = "Lambrechtgasse";
			lambrechtgasseToolStripMenuItem.Click += Location_ToolStripMenuItem_Click;
			// 
			// rittingergasseToolStripMenuItem
			// 
			rittingergasseToolStripMenuItem.Name = "rittingergasseToolStripMenuItem";
			rittingergasseToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
			rittingergasseToolStripMenuItem.Tag = "4";
			rittingergasseToolStripMenuItem.Text = "Rittingergasse";
			rittingergasseToolStripMenuItem.Click += Location_ToolStripMenuItem_Click;
			// 
			// hietzingPenzingToolStripMenuItem
			// 
			hietzingPenzingToolStripMenuItem.Name = "hietzingPenzingToolStripMenuItem";
			hietzingPenzingToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
			hietzingPenzingToolStripMenuItem.Tag = "5";
			hietzingPenzingToolStripMenuItem.Text = "Hietzing/Penzing";
			hietzingPenzingToolStripMenuItem.Click += Location_ToolStripMenuItem_Click;
			// 
			// iKEANordToolStripMenuItem
			// 
			iKEANordToolStripMenuItem.Name = "iKEANordToolStripMenuItem";
			iKEANordToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
			iKEANordToolStripMenuItem.Tag = "6";
			iKEANordToolStripMenuItem.Text = "IKEA Nord";
			iKEANordToolStripMenuItem.Click += Location_ToolStripMenuItem_Click;
			// 
			// toolStripSeparator1
			// 
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new System.Drawing.Size(170, 6);
			// 
			// DatasetInfoleftToolStripMenuItem
			// 
			DatasetInfoleftToolStripMenuItem.Name = "DatasetInfoleftToolStripMenuItem";
			DatasetInfoleftToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
			DatasetInfoleftToolStripMenuItem.Text = "Dataset info (left)";
			DatasetInfoleftToolStripMenuItem.Click += DatasetInfoleftToolStripMenuItem_Click;
			// 
			// DatasetInforightToolStripMenuItem
			// 
			DatasetInforightToolStripMenuItem.Name = "DatasetInforightToolStripMenuItem";
			DatasetInforightToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
			DatasetInforightToolStripMenuItem.Text = "Dataset info (right)";
			DatasetInforightToolStripMenuItem.Click += DatasetInforightToolStripMenuItem_Click;
			// 
			// exportToolStripMenuItem
			// 
			exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { currentViewToolStripMenuItem, currentViewallYearsToolStripMenuItem, currentViewsbothToolStripMenuItem });
			exportToolStripMenuItem.Name = "exportToolStripMenuItem";
			exportToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
			exportToolStripMenuItem.Text = "Export";
			// 
			// currentViewToolStripMenuItem
			// 
			currentViewToolStripMenuItem.Name = "currentViewToolStripMenuItem";
			currentViewToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
			currentViewToolStripMenuItem.Text = "Current (left) view";
			currentViewToolStripMenuItem.Click += CurrentViewToolStripMenuItem_Click;
			// 
			// currentViewallYearsToolStripMenuItem
			// 
			currentViewallYearsToolStripMenuItem.Name = "currentViewallYearsToolStripMenuItem";
			currentViewallYearsToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
			currentViewallYearsToolStripMenuItem.Text = "Current (left) view (all years)";
			currentViewallYearsToolStripMenuItem.Click += CurrentViewallYearsToolStripMenuItem_Click;
			// 
			// currentViewsbothToolStripMenuItem
			// 
			currentViewsbothToolStripMenuItem.Name = "currentViewsbothToolStripMenuItem";
			currentViewsbothToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
			currentViewsbothToolStripMenuItem.Text = "Current views (both)";
			currentViewsbothToolStripMenuItem.Click += CurrentViewsbothToolStripMenuItem_Click;
			// 
			// _MapViewerRight
			// 
			_MapViewerRight.DebugMode = false;
			_MapViewerRight.Location = new System.Drawing.Point(398, 259);
			_MapViewerRight.Name = "_MapViewerRight";
			_MapViewerRight.OnMapBoundsChanged = null;
			_MapViewerRight.OnMouseLocationPxChanged = null;
			_MapViewerRight.OnScrollWithModifierKey = null;
			_MapViewerRight.Size = new System.Drawing.Size(716, 251);
			_MapViewerRight.TabIndex = 8;
			// 
			// _YearControlLeft
			// 
			_YearControlLeft.DescriptionFont = new System.Drawing.Font("Bahnschrift Light Condensed", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			_YearControlLeft.Location = new System.Drawing.Point(407, 85);
			_YearControlLeft.Name = "_YearControlLeft";
			_YearControlLeft.OnSelectedItemChanged = null;
			_YearControlLeft.Orientation = UI.Controls.ItemListControl.Orientations.Horizontal;
			_YearControlLeft.SelectedItemIndex = 1;
			_YearControlLeft.Size = new System.Drawing.Size(568, 129);
			_YearControlLeft.TabIndex = 9;
			_YearControlLeft.TextFont = new System.Drawing.Font("Roboto Slab", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			// 
			// _YearControlRight
			// 
			_YearControlRight.DescriptionFont = new System.Drawing.Font("Bahnschrift Light Condensed", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			_YearControlRight.Location = new System.Drawing.Point(631, 355);
			_YearControlRight.Name = "_YearControlRight";
			_YearControlRight.OnSelectedItemChanged = null;
			_YearControlRight.Orientation = UI.Controls.ItemListControl.Orientations.Horizontal;
			_YearControlRight.SelectedItemIndex = 1;
			_YearControlRight.Size = new System.Drawing.Size(568, 104);
			_YearControlRight.TabIndex = 10;
			_YearControlRight.TextFont = new System.Drawing.Font("Roboto Slab", 21.75F);
			// 
			// _zoomControl
			// 
			_zoomControl.DescriptionFont = new System.Drawing.Font("Roboto Slab Light", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			_zoomControl.Location = new System.Drawing.Point(12, 307);
			_zoomControl.Name = "_zoomControl";
			_zoomControl.OnSelectedItemChanged = null;
			_zoomControl.Orientation = UI.Controls.ItemListControl.Orientations.Vertical;
			_zoomControl.SelectedItemIndex = 2;
			_zoomControl.Size = new System.Drawing.Size(430, 152);
			_zoomControl.TabIndex = 11;
			_zoomControl.TextFont = new System.Drawing.Font("Bahnschrift Light Condensed", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			// 
			// FViewer
			// 
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			ClientSize = new System.Drawing.Size(1136, 585);
			Controls.Add(_zoomControl);
			Controls.Add(_YearControlRight);
			Controls.Add(_YearControlLeft);
			Controls.Add(_MapViewerRight);
			Controls.Add(_MapViewerLeft);
			Controls.Add(menuStrip1);
			Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			KeyPreview = true;
			MainMenuStrip = menuStrip1;
			MinimumSize = new System.Drawing.Size(800, 600);
			Name = "FViewer";
			StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Wien MA41 Historical Aerial Imagery Viewer";
			WindowState = System.Windows.Forms.FormWindowState.Maximized;
			Load += FViewer_Load;
			KeyDown += FViewer_KeyDown;
			Resize += FViewer_Resize;
			menuStrip1.ResumeLayout(false);
			menuStrip1.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
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
		private UI.Controls.ItemListControl _YearControlLeft;
		private UI.Controls.ItemListControl _YearControlRight;
		private UI.Controls.ItemListControl _zoomControl;
		private System.Windows.Forms.ToolStripMenuItem currentViewsbothToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem CrosshairToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem DatasetInfoleftToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem DatasetInforightToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem ToggleSecondViewToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
	}
}