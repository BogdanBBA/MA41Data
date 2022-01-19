using System;
using System.IO;

namespace MA41.Commons
{
	public static class Paths
	{
		public static string ROOT_FOLDER { get; private set; }
		public static string DOWNLOAD_FOLDER { get; private set; }
		public static string IMAGES_FOLDER { get; private set; }
		public static string THUMBNAILS_FOLDER { get; private set; }
		public static string IMAGE_SIZE_DICTIONARY_FILE { get; private set; }
		public static string SETTINGS_FILE { get; private set; }

		private static string GetParentFolder(string name)
		{
			var folder = new DirectoryInfo(Environment.CurrentDirectory);
			while (folder.Name != name)
			{
				folder = folder.Parent;
			}
			return folder.FullName;
		}

		public static void Initialize()
		{
			const string rootFolderName = "MA41Data";
			const string dataFolder = "data";

			ROOT_FOLDER = GetParentFolder(rootFolderName);
			DOWNLOAD_FOLDER = Path.Combine(ROOT_FOLDER, dataFolder, "downloads");
			IMAGES_FOLDER = Path.Combine(ROOT_FOLDER, dataFolder, "unpacked");
			THUMBNAILS_FOLDER = Path.Combine(ROOT_FOLDER, dataFolder, "thumbnails");

			foreach (string folder in new[] { DOWNLOAD_FOLDER, IMAGES_FOLDER, THUMBNAILS_FOLDER })
			{
				if (!Directory.Exists(folder))
				{
					Console.WriteLine($" *** Warning: folder '{folder}' did not exist, but was created");
					Directory.CreateDirectory(folder);
				}
			}

			IMAGE_SIZE_DICTIONARY_FILE = Path.Combine(ROOT_FOLDER, dataFolder, "image-sizes.txt");
			SETTINGS_FILE = Path.Combine(ROOT_FOLDER, dataFolder, "settings.txt");
		}
	}
}
