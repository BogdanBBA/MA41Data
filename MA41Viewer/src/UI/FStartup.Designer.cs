
namespace MA41Viewer.UI
{
	partial class FStartup
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.ViewB = new System.Windows.Forms.Button();
			this.ExitB = new System.Windows.Forms.Button();
			this.GetTilesB = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(190, 15);
			this.label1.TabIndex = 0;
			this.label1.Text = "Geodata download and processing";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(13, 28);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(68, 15);
			this.label2.TabIndex = 1;
			this.label2.Text = "Map viewer";
			// 
			// ViewB
			// 
			this.ViewB.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.ViewB.Location = new System.Drawing.Point(13, 75);
			this.ViewB.Name = "ViewB";
			this.ViewB.Size = new System.Drawing.Size(189, 23);
			this.ViewB.TabIndex = 2;
			this.ViewB.Text = "View map";
			this.ViewB.UseVisualStyleBackColor = true;
			this.ViewB.Click += new System.EventHandler(this.ViewB_Click);
			// 
			// ExitB
			// 
			this.ExitB.Location = new System.Drawing.Point(13, 116);
			this.ExitB.Name = "ExitB";
			this.ExitB.Size = new System.Drawing.Size(189, 23);
			this.ExitB.TabIndex = 3;
			this.ExitB.Text = "Exit";
			this.ExitB.UseVisualStyleBackColor = true;
			this.ExitB.Click += new System.EventHandler(this.ExitB_Click);
			// 
			// GetTilesB
			// 
			this.GetTilesB.Enabled = false;
			this.GetTilesB.Location = new System.Drawing.Point(13, 46);
			this.GetTilesB.Name = "GetTilesB";
			this.GetTilesB.Size = new System.Drawing.Size(189, 23);
			this.GetTilesB.TabIndex = 4;
			this.GetTilesB.Text = "Get tiles";
			this.GetTilesB.UseVisualStyleBackColor = true;
			// 
			// FStartup
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(216, 151);
			this.Controls.Add(this.GetTilesB);
			this.Controls.Add(this.ExitB);
			this.Controls.Add(this.ViewB);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FStartup";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "FStartup";
			this.Load += new System.EventHandler(this.FStartup_Load);
			this.Shown += new System.EventHandler(this.FStartup_Shown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button ViewB;
		private System.Windows.Forms.Button ExitB;
		private System.Windows.Forms.Button GetTilesB;
	}
}

