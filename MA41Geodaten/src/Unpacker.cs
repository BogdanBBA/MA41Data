using MA41.Commons;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;

namespace MA41Geodaten
{
	public static class Unpacker
	{
		public static readonly string UNPACKED_FOLDER = Path.Combine(Environment.CurrentDirectory, @"unpacked");
		public static readonly string TEMP_FOLDER = Path.Combine(Environment.CurrentDirectory, @"temp");

		private const string FILENAME_PATTERN = @"(\d{1,2}).+(\d{1,2}).+(\d{4})";

		private static void DeleteDirectoryRecursive(string path, bool log = true)
		{
			if (!Directory.Exists(path))
				return;
			string[] folders = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
			foreach (string folder in folders)
				DeleteDirectoryRecursive(folder, log);
			string[] files = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
			foreach (string file in files)
				File.Delete(file);
			if (log)
				Console.WriteLine($" * Deleting '{path}'...");
			Directory.Delete(path);
		}

		private static string GetNewUnpackedFileExtension(string ext)
		{
			switch (ext.Replace(".", "").ToLowerInvariant())
			{
				case "jpg":
					return "jpg";
				case "jgw":
				case "wld":
					return "jgw";
				default:
					throw new ApplicationException($"Unexpected file extension '{ext}'!");
			}
		}

		public static void UnpackAndRename()
		{
			Console.WriteLine($" *** Unpacking and renaming...");

			Console.WriteLine($" *** Warning: all files under '{UNPACKED_FOLDER}' will be erased! Press Enter to continue...");
			Console.ReadLine();
			DeleteDirectoryRecursive(UNPACKED_FOLDER);

			string[] zipPaths = Directory.GetFiles(Paths.DOWNLOAD_FOLDER, @"*.zip", SearchOption.TopDirectoryOnly);
			for (int zipPathIndex = 0; zipPathIndex < zipPaths.Length; zipPathIndex++)
			{
				string zipPath = zipPaths[zipPathIndex];
				Match match = Regex.Match(Path.GetFileNameWithoutExtension(zipPath), FILENAME_PATTERN);
				(int Square, int Quadrant, int Year) = (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value));
				string folder = Path.Combine(UNPACKED_FOLDER, $"{Year}");
				Directory.CreateDirectory(folder);
				Console.WriteLine($" * Unpacking {zipPathIndex + 1}/{zipPaths.Length} '{zipPath}' -> temp folder -> '{folder}'...");
				DeleteDirectoryRecursive(TEMP_FOLDER, false);
				ZipFile.ExtractToDirectory(zipPath, TEMP_FOLDER, false);
				string[] files = Directory.GetFiles(TEMP_FOLDER, "*.*", SearchOption.TopDirectoryOnly);
				if (files.Length != 2)
					throw new ApplicationException($"Was expecting exactly 2 files (got instead {files.Length}: {string.Join(", ", files.Select(file => $"'{file}'"))})!");
				if (files.Any(file => !Regex.IsMatch(file, FILENAME_PATTERN)))
					throw new ApplicationException($"One or more of unpacked files {string.Join(", ", files.Select(file => $"'{file}'"))} aren't in a good format!");
				foreach (string file in files)
				{
					string newFile = Path.Combine(folder, $"{Square}-{Quadrant}.{GetNewUnpackedFileExtension(Path.GetExtension(file))}");
					Console.WriteLine($"   - Moving file '{file}' -> '{newFile}'...");
					if (!File.Exists(newFile))
						File.Move(file, newFile);
				}
			}

			Console.WriteLine($" *** Done unpacking and renaming.\n");
		}

		public static void Copy()
		{
			Console.WriteLine($" *** Copying (preparing for a later, manual resize)...");
			Console.Write($"  *  Name the thumbnail-size destination folder (existing folders: {string.Join(", ", Directory.GetDirectories(Paths.THUMBNAILS_FOLDER, "*", SearchOption.TopDirectoryOnly).Select(dir => $"'{Path.GetFileNameWithoutExtension(dir)}'"))}): ");
			var destFolder = Console.ReadLine();
			destFolder = Path.Combine(Paths.THUMBNAILS_FOLDER, destFolder);
			Console.WriteLine();
			if (Directory.Exists(destFolder))
			{
				Console.WriteLine($" *** ERROR: Folder '{destFolder}' already exists. Will not do anything");
				return;
			}
			Directory.CreateDirectory(destFolder);
			var yearFolders = Directory.GetDirectories(Paths.IMAGES_FOLDER, "*", SearchOption.TopDirectoryOnly);
			foreach (var yearFolder in yearFolders)
			{
				uint year = uint.Parse(Path.GetFileNameWithoutExtension(yearFolder));
				var files = Directory.GetFiles(yearFolder, "*.jpg", SearchOption.TopDirectoryOnly);
				foreach (var srcFile in files)
				{
					string destFile = Path.Combine(destFolder, $"{year}-{Path.GetFileName(srcFile)}");
					Console.WriteLine($"  -  Copy '{srcFile}' -> '{destFile}'...");
					File.Copy(srcFile, destFile);
				}
			}
		}
	}
}