using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Windows.Forms;

namespace MA41Viewer
{
	static class Program
	{
		private static void SetupLog()
		{
			LoggingConfiguration logConfig = new();
			ConsoleTarget consoleLogTarget = new();
			logConfig.AddTarget("console", consoleLogTarget);
			logConfig.AddRule(LogLevel.Trace, LogLevel.Fatal, consoleLogTarget);
			LogManager.Configuration = logConfig;
		}

		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			SetupLog();
			Application.Run(new UI.FStartup());
		}
	}
}
