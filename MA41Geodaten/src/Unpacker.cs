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

		public static void Validate()
		{
			Console.WriteLine($" *** Validating downloaded files...");
			string[] zipPaths = Directory.GetFiles(Paths.DOWNLOAD_FOLDER, @"*.zip", SearchOption.TopDirectoryOnly);
			int failures = 0;
			for (int iZF = 0; iZF < zipPaths.Length; iZF++)
			{
				string zipPath = zipPaths[iZF];
				try
				{
					if (!Regex.IsMatch(Path.GetFileNameWithoutExtension(zipPath), FILENAME_PATTERN))
						throw new ApplicationException($"Zip file path '{zipPath}' does not match expected filename pattern!");
					if (new FileInfo(zipPath).Length == 0)
						throw new ApplicationException($"Zip file path '{zipPath}' is empty!");
					using (ZipArchive zipFile = ZipFile.OpenRead(zipPath))
					{
						Console.WriteLine($"\t[{iZF,3}/{zipPaths.Length,3}] file '{zipPath}' contains {zipFile.Entries.Count} entries, OK");
					}
				}
				catch (Exception ex)
				{
					failures++;
					if (ex is not ApplicationException)
						Console.WriteLine($"\t\tWarning, file '{zipPath}' threw unexpected exception {ex}!");
					Console.WriteLine($"\t\tWill delete file '{zipPath}'. Press Enter to continue...");
					Console.ReadLine();
					File.Delete(zipPath);
				}
			}
			Console.WriteLine($" *** Validation done, with {failures} failures.");
		}

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
			=> ext.Replace(".", "").ToLowerInvariant() switch
			{
				"jpg" => "jpg",
				"jgw" or "wld" => "jgw",
				_ => throw new ApplicationException($"Unexpected file extension '{ext}'!"),
			};

		public static void UnpackAndRename()
		{
			Console.WriteLine($" *** Unpacking and renaming...");

			int existingFileCount = Directory.Exists(UNPACKED_FOLDER) ? Directory.GetFiles(UNPACKED_FOLDER).Length : 0;
			Console.WriteLine($" *** Warning: all files under '{UNPACKED_FOLDER}' ({existingFileCount} files) will be erased!\n\tPress Enter to continue...");
			Console.ReadLine();
			DeleteDirectoryRecursive(UNPACKED_FOLDER);

			int archives = 0, totalFiles = 0;
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
				archives++;
				foreach (string file in files)
				{
					string newFile = Path.Combine(folder, $"{Square}-{Quadrant}.{GetNewUnpackedFileExtension(Path.GetExtension(file))}");
					Console.WriteLine($"   - Moving file '{file}' -> '{newFile}'...");
					if (!File.Exists(newFile))
					{
						File.Move(file, newFile);
						totalFiles++;
					}
				}
			}

			Console.WriteLine($" *** Done unpacking and renaming {archives} archives and {totalFiles} files.\n");
		}

		public static void Copy()
		{
			Console.WriteLine($" *** Copying files ('{Paths.IMAGES_FOLDER}' -> '{Paths.THUMBNAILS_FOLDER}'; preparing for a later, manual resize)...");
			Console.Write($"  *  Name the thumbnail-size destination folder (existing folders: {string.Join(", ", Directory.GetDirectories(Paths.THUMBNAILS_FOLDER, "*", SearchOption.TopDirectoryOnly).Select(dir => $"'{Path.GetFileNameWithoutExtension(dir)}'"))}): ");
			string destFolder = Console.ReadLine();
			destFolder = Path.Combine(Paths.THUMBNAILS_FOLDER, destFolder);
			Console.WriteLine();
			if (Directory.Exists(destFolder))
			{
				Console.WriteLine($" *** ERROR: Folder '{destFolder}' already exists. Will not do anything");
				return;
			}
			Directory.CreateDirectory(destFolder);
			string[] yearFolders = Directory.GetDirectories(Paths.IMAGES_FOLDER, "*", SearchOption.TopDirectoryOnly);
			foreach (string yearFolder in yearFolders)
			{
				uint year = uint.Parse(Path.GetFileNameWithoutExtension(yearFolder));
				string[] files = Directory.GetFiles(yearFolder, "*.jpg", SearchOption.TopDirectoryOnly);
				foreach (string srcFile in files)
				{
					string destFile = Path.Combine(destFolder, $"{year}-{Path.GetFileName(srcFile)}");
					Console.WriteLine($"  -  Copy '{srcFile}' -> '{destFile}'...");
					File.Copy(srcFile, destFile);
				}
			}
		}
	}
}