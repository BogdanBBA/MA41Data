using MA41.Commons;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace MA41Viewer.UI
{
	public partial class FStartup : Form
	{
		public FStartup()
		{
			InitializeComponent();
		}

		private void FStartup_Load(object sender, EventArgs e)
		{
			CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
			Paths.Initialize();
		}

		private void FStartup_Shown(object sender, EventArgs e)
		{
			ViewB_Click(sender, e);
		}

		private void ViewB_Click(object sender, EventArgs e)
		{
			Hide();
			new FViewer().ShowDialog(this);
			Show();
			Close(); // <-- temporary
		}

		private void ExitB_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
