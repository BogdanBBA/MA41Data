using MA41.Commons;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MA41Geodaten
{
    public class Program
    {
        private const string ARGUMENT_DOWNLOAD = "download";
        private const string ARGUMENT_UNPACK = "unpack";
        private static readonly string[] VALID_ARGUMENTS = { ARGUMENT_DOWNLOAD, ARGUMENT_UNPACK };

        public static string GetCurrentFileStats()
        {
            var files = Directory.GetFiles(Paths.DOWNLOAD_FOLDER, "*.zip", SearchOption.TopDirectoryOnly).Select(path => Path.GetFileNameWithoutExtension(path));
            var groupings = files.Select(name => Regex.Match(name, @"(\d{4,})").Groups[1].Value).GroupBy(year => year).OrderByDescending(grouping => grouping.Key);
            if (groupings.Count() == 0)
                return $"There are no years in '{Paths.DOWNLOAD_FOLDER}'";
            return $"Currently there are {groupings.Count()} years: {string.Join(", ", groupings.Select(grouping => $"{grouping.Key} ({grouping.Count()} files)"))}";
        }

        public static void Main(string[] args)
        {
            Paths.Initialize();

            Console.WriteLine($"\n *** Program arguments: {string.Join(' ', args)}\n");

            if (args.Length == 0 || !args.All(arg => VALID_ARGUMENTS.Contains(arg)))
            {
                Console.WriteLine($" *** Invalid arguments!\n *** Valid arguments (minimum one) are: {string.Join(", ", VALID_ARGUMENTS.Select(arg => $"\"{arg}\""))}.\n");
                return;
            }

            Console.WriteLine($" *** {GetCurrentFileStats()}.\n");

            foreach (string arg in args)
            {
                switch (arg)
                {
                    case ARGUMENT_DOWNLOAD:
                        Downloader.Download();
                        break;
                    case ARGUMENT_UNPACK:
                        Unpacker.UnpackAndRename();
                        break;
                }
            }
        }
    }
}
