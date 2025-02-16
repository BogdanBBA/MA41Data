using MA41Viewer.UI.Controls;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace MA41Viewer.UI
{
	public partial class FExport : Form
	{
		public record TaskArguments(MapViewer LeftMV, MapViewer RightMV, string Filename);

		public class TaskProgress(string text, int done, int total)
		{
			public readonly string Text = text;
			public readonly (int Done, int Total) Progress = (done, total);
		}

		public FExport()
		{
			InitializeComponent();
		}

		//

		public void Export_OneView_OneYear(FViewer parentForm, MapViewer mapViewer, string filename)
		{
			Show(parentForm);
			oneViewOneYearWorker.RunWorkerAsync(new TaskArguments(mapViewer, null, filename));
		}

		private void OneViewOneYearWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			TaskArguments args = e.Argument as TaskArguments;
			args.LeftMV.SaveCurrentViewToFile(args.Filename, taskProgress => oneViewOneYearWorker.ReportProgress(-1, taskProgress));
			e.Result = args.Filename;
		}

		//

		public void Export_TwoViews_OneYear(FViewer parentForm, MapViewer left, MapViewer right, string filename)
		{
			Show(parentForm);
			twoViewsOneYearWorker.RunWorkerAsync(new TaskArguments(left, right, filename));
		}

		private void TwoViewsOneYearWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			TaskArguments args = e.Argument as TaskArguments;
			args.LeftMV.SaveCurrentViewsToFile(args.RightMV, args.Filename, taskProgress => twoViewsOneYearWorker.ReportProgress(-1, taskProgress));
			e.Result = args.Filename;
		}

		//

		public void Export_OneView_AllYears(FViewer parentForm, MapViewer mapViewer, string filename)
		{
			Show(parentForm);
			oneViewAllYearsWorker.RunWorkerAsync(new TaskArguments(mapViewer, null, filename));
		}

		private void OneViewAllYearsWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			TaskArguments args = e.Argument as TaskArguments;
			args.LeftMV.SaveAllYearsToFile(args.Filename, taskProgress => oneViewAllYearsWorker.ReportProgress(-1, taskProgress));
			e.Result = args.Filename;
		}

		//

		private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			TaskProgress progress = e.UserState as TaskProgress;
			textTV.Text = progress.Text;
			progressBar.Maximum = progress.Progress.Total;
			progressBar.Value = progress.Progress.Done;
		}

		private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			Process.Start("explorer.exe", $"/select, \"{e.Result as string}\"");
			Close();
		}
	}
}
