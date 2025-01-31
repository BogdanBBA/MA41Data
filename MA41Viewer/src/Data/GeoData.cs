﻿using MA41.Commons;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MA41Viewer.Data
{
	public static class GeoData
	{
		public static GeoModel GeoModel { get; private set; }

		public static string Initialize()
		{
			try
			{
				ImageSizeDictionary.Initialize();
				GeoModel = new GeoModel();
				string[] files = Directory.GetFiles(Paths.IMAGES_FOLDER, "*.*", SearchOption.AllDirectories);

				string[] acceptedExtensions = [".jpg", ".jgw"];
				uint[] years = [.. Directory.GetDirectories(Paths.IMAGES_FOLDER, "*", SearchOption.TopDirectoryOnly).Select(Path.GetFileName).Select(uint.Parse).OrderBy(x => x)];

				// all files are either a JPG tile image or a JGW jpeg world data file
				IEnumerable<string> distinctExtensions = files.Select(Path.GetExtension).Distinct();
				if (distinctExtensions.Any(ext => !acceptedExtensions.Contains(ext)))
					throw new ApplicationException($"There are unknown exceptions: only accepting {string.Join(", ", acceptedExtensions)}; files have {string.Join(", ", distinctExtensions)}.");

				// read JGW
				foreach (uint year in years)
				{
					string yearFolder = Path.Combine(Paths.IMAGES_FOLDER, year.ToString());
					string[] jgwPaths = Directory.GetFiles(yearFolder, "*.jgw", SearchOption.TopDirectoryOnly);
					foreach (string jgwPath in jgwPaths)
					{
						Match match = Regex.Match(Path.GetFileNameWithoutExtension(jgwPath), @"(\d+).+(\d+)");
						(uint square, uint quadrant) = (uint.Parse(match.Groups[1].Value), uint.Parse(match.Groups[2].Value));
						WorldFileDataDictionary.WorldFileDataYear jgw = new(jgwPath);
						GeoModel.GetByMA41Coordinates(square, quadrant).Add(year, jgw);

						// also check now that for every JGW, there's a corresponding JPG and get its WxH size
						string jpgPath = Path.Combine(yearFolder, $"{Path.GetFileNameWithoutExtension(jgwPath)}.jpg");
						if (!File.Exists(jpgPath))
							throw new ApplicationException($"File '{jpgPath}' does not exist!");
						(uint year, uint square, uint quadrant) imageSizeKey = (year, square, quadrant);
						if (!ImageSizeDictionary.Sizes.ContainsKey(imageSizeKey))
						{
							using (Image img = Image.Load(jpgPath))
							{
								Size size = img.Size;
								ImageSizeDictionary.Sizes[imageSizeKey] = ((uint)size.Width, (uint)size.Height);
								ImageSizeDictionary.Save();
							}
							//using (var imageFactory = new ImageFactory())
							//{
							//	var size = imageFactory.Load(jpgPath).Image.Size;
							//	ImageSizeDictionary.Sizes[imageSizeKey] = ((uint)size.Width, (uint)size.Height);
							//	ImageSizeDictionary.Save();
							//}
						}

						GeoModel.GetByMA41Coordinates(square, quadrant).Get(year).SetImageSize(ImageSizeDictionary.Sizes[imageSizeKey]);
					}
				}

				GeoModel.DoPostProcessing(years);

				return string.Empty;
			}
			catch (Exception ex)
			{
				return ex.ToString();
			}
		}

		public static string GetJpgPath(uint year, uint square, uint quadrant)
			=> Path.Combine(Paths.IMAGES_FOLDER, $"{year}", $"{square}-{quadrant}.jpg");

		public static string GetJpgThumbnailPath(uint year, uint square, uint quadrant, uint size)
		{
			string folder = Path.Combine(Paths.THUMBNAILS_FOLDER, $"{size}px");
			if (!Directory.Exists(folder))
				Directory.CreateDirectory(folder);
			return Path.Combine(folder, $"{year}-{square}-{quadrant}.jpg");
		}
	}
}
