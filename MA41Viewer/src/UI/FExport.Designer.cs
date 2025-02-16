using System;
using System.ComponentModel;

namespace MA41Viewer.UI
{
	partial class FExport
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
			textTV = new System.Windows.Forms.Label();
			progressBar = new System.Windows.Forms.ProgressBar();
			oneViewOneYearWorker = new BackgroundWorker();
			oneViewAllYearsWorker = new BackgroundWorker();
			twoViewsOneYearWorker = new BackgroundWorker();
			SuspendLayout();
			// 
			// textTV
			// 
			textTV.AutoSize = true;
			textTV.Location = new System.Drawing.Point(12, 9);
			textTV.Name = "textTV";
			textTV.Size = new System.Drawing.Size(38, 15);
			textTV.TabIndex = 0;
			textTV.Text = "label1";
			// 
			// progressBar
			// 
			progressBar.Location = new System.Drawing.Point(12, 28);
			progressBar.Name = "progressBar";
			progressBar.Size = new System.Drawing.Size(539, 23);
			progressBar.TabIndex = 1;
			// 
			// oneViewOneYearWorker
			// 
			oneViewOneYearWorker.WorkerReportsProgress = true;
			oneViewOneYearWorker.DoWork += OneViewOneYearWorker_DoWork;
			oneViewOneYearWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
			oneViewOneYearWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
			// 
			// oneViewAllYearsWorker
			// 
			oneViewAllYearsWorker.WorkerReportsProgress = true;
			oneViewAllYearsWorker.DoWork += OneViewAllYearsWorker_DoWork;
			oneViewAllYearsWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
			oneViewAllYearsWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
			// 
			// twoViewsOneYearWorker
			// 
			twoViewsOneYearWorker.WorkerReportsProgress = true;
			twoViewsOneYearWorker.DoWork += TwoViewsOneYearWorker_DoWork;
			twoViewsOneYearWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
			twoViewsOneYearWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
			// 
			// FExport
			// 
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			ClientSize = new System.Drawing.Size(559, 59);
			ControlBox = false;
			Controls.Add(progressBar);
			Controls.Add(textTV);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			Name = "FExport";
			ShowIcon = false;
			ShowInTaskbar = false;
			StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Please wait";
			TopMost = true;
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Label textTV;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.ComponentModel.BackgroundWorker oneViewOneYearWorker;
		private System.ComponentModel.BackgroundWorker oneViewAllYearsWorker;
		private System.ComponentModel.BackgroundWorker twoViewsOneYearWorker;
	}
}