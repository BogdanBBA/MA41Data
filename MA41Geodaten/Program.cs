using MA41.Commons;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MA41Geodaten
{
	public class Program
	{
		private const string ARGUMENT_DOWNLOAD = "download";
		private const string ARGUMENT_VALIDATE = "validate";
		private const string ARGUMENT_UNPACK = "unpack";
		private const string ARGUMENT_COPY = "copy";

		private static readonly string[] VALID_ARGUMENTS = [ARGUMENT_DOWNLOAD, ARGUMENT_VALIDATE, ARGUMENT_UNPACK, ARGUMENT_COPY];

		private static readonly string[] DEFAULT_ARGUMENTS = [ARGUMENT_COPY];

		public static string GetCurrentFileStats()
		{
			IOrderedEnumerable<IGrouping<string, string>> groupings = Directory
				.GetFiles(Paths.DOWNLOAD_FOLDER, "*.zip", SearchOption.TopDirectoryOnly)
				.Select(Path.GetFileNameWithoutExtension)
				.Select(name => Regex.Match(name, @"(\d{4,})").Groups[1].Value)
				.GroupBy(year => year)
				.OrderByDescending(grouping => grouping.Key);
			if (!groupings.Any())
				return $"There are no years in '{Paths.DOWNLOAD_FOLDER}'";
			return $"Currently there are {groupings.Count()} years: {string.Join(", ", groupings.Select(grouping => $"{grouping.Key} ({grouping.Count()} files)"))}";
		}

		public static async Task Main(string[] args)
		{
			CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
			Paths.Initialize();

			if (args.Length == 0) args = [.. DEFAULT_ARGUMENTS];
			Console.WriteLine($"\n *** Program arguments: {string.Join(' ', args)}\n");

			if (!args.All(arg => VALID_ARGUMENTS.Contains(arg)))
			{
				Console.WriteLine($" *** Invalid arguments!\n *** Valid arguments (minimum one) are: {string.Join(", ", VALID_ARGUMENTS.Select(arg => $"\"{arg}\""))}.\n");
				return;
			}

			foreach (string arg in args)
			{
				switch (arg)
				{
					case ARGUMENT_DOWNLOAD:
						Console.WriteLine($" *** {GetCurrentFileStats()}.\n");
						await Downloader.Download();
						break;
					case ARGUMENT_VALIDATE:
						Unpacker.Validate();
						break;
					case ARGUMENT_UNPACK:
						Console.WriteLine($" *** {GetCurrentFileStats()}.\n");
						Unpacker.UnpackAndRename();
						break;
					case ARGUMENT_COPY:
						Unpacker.Copy();
						break;
				}
			}
		}
	}
}
