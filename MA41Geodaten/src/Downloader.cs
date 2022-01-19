using MA41.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace MA41Geodaten
{
	public static class Downloader
	{
		public static readonly List<(int page, int part)> COORDINATES = Enumerable.Range(1, 5).SelectMany(x => Enumerable.Range(2, 8).SelectMany(y => Enumerable.Range(1, 4).Select(q => (x * 10 + y, q)))).ToList();

		public static readonly string URL_ROOT = "https://www.wien.gv.at/ma41datenviewer/downloads/geodaten";

		public static readonly Dictionary<string, bool> YEAR_URL_TEMPLATES = new Dictionary<string, bool>()
		{
			{ "{0}/op_img/{1}_{2}_op_2021.zip", true },
			{ "{0}/op_img/{1}_{2}_op_2020.zip", true },
			{ "{0}/op_img/{1}_{2}_op_2019.zip", true },
			{ "{0}/op_img/{1}_{2}_op_2018.zip", false },
			{ "{0}/op_img/{1}_{2}_op_2017.zip", false },
			{ "{0}/op_img/{1}_{2}_op_2016.zip", false },
			{ "{0}/op_img/{1}_{2}_op_2015.zip", false },
			{ "{0}/op_img/{1}_{2}_op2014.zip", true },
			{ "{0}/lb_img/{1}_{2}_lb1992.zip", true },
			{ "{0}/lb_img/{1}_{2}_lb1976.zip", true },
			{ "{0}/lb_img/{1}_{2}_lb1956.zip", true },
			{ "{0}/lb_img/{1}_{2}_lb1938.zip", true }
		};

		public static readonly string URLS_NOT_FOUND_404_FILE = Path.Combine(Paths.DOWNLOAD_FOLDER, @"404s.txt");

		static void DownloadFileCallback(object sender, AsyncCompletedEventArgs e)
		{
			Console.WriteLine($"\tDownloadFileCallback: user state {e.UserState}");
			if (e.Cancelled)
				Console.WriteLine("\tFile download cancelled.");
			if (e.Error != null)
				Console.WriteLine($"\t{e.Error}");
		}

		static void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
		{
			Console.WriteLine("\tDownloadProgressCallback user state '{0}', downloaded {1} of {2} bytes. {3} % complete", (string)e.UserState, e.BytesReceived, e.TotalBytesToReceive, e.ProgressPercentage);
		}

		public static void Download()
		{
			Console.WriteLine($" *** Downloading...");
			Console.WriteLine($" *** Page coordinates: {string.Join(", ", COORDINATES.Select(coord => $"{coord.page}/{coord.part}"))}.\n");
			Console.WriteLine($" *** URL templates: {string.Join(", ", YEAR_URL_TEMPLATES.Select(template => $"{template.Key} ({template.Value})"))}.\n");

			List<string> urlsNotFound404 = new List<string>();
			if (File.Exists(URLS_NOT_FOUND_404_FILE))
			{
				urlsNotFound404.AddRange(File.ReadAllLines(URLS_NOT_FOUND_404_FILE));
			}

			using (WebClient web = new WebClient())
			{
				web.DownloadFileCompleted += DownloadFileCallback;
				web.DownloadProgressChanged += DownloadProgressCallback;
				string[] urlTemplates = YEAR_URL_TEMPLATES.Keys.Where(key => YEAR_URL_TEMPLATES[key]).ToArray();

				if (urlTemplates.Length > 0)
				{
					for (int iCoord = 0; iCoord < COORDINATES.Count; iCoord++)
					{
						for (int iUrl = 0; iUrl < urlTemplates.Length; iUrl++)
						{
							string url = string.Format(urlTemplates[iUrl], URL_ROOT, COORDINATES[iCoord].page, COORDINATES[iCoord].part);
							string file = Path.Combine(Paths.DOWNLOAD_FOLDER, Path.GetFileName(url));
							bool fileDownloadedNow = false;

							Console.WriteLine($"Item {iCoord * urlTemplates.Length + (iUrl + 1)}/{COORDINATES.Count * urlTemplates.Length} (page {iCoord + 1}/{COORDINATES.Count}: '{COORDINATES[iCoord].page}/{COORDINATES[iCoord].part}'), url {iUrl + 1}/{urlTemplates.Length} ('{url}' -> '{file}')...");

							if (File.Exists(file) && new FileInfo(file).Length == 0)
							{
								Console.WriteLine($"\tFile exists but has length 0, deleting...");
								File.Delete(file);
							}

							if (File.Exists(file))
							{
								Console.WriteLine($"\tFile  exists ({new FileInfo(file).Length / 1024:N0} KB).");
							}
							else if (urlsNotFound404.Contains(url))
							{
								Console.WriteLine($"\tFile returned a 404 not found in the past, will be skipped");
							}
							else
							{
								try
								{
									fileDownloadedNow = true;
									web.DownloadFile(url, file);
									Console.WriteLine($"\tFile downloaded now ({new FileInfo(file).Length / 1024:N0} KB)");
								}
								catch (WebException wex)
								{
									if (((HttpWebResponse)wex.Response)?.StatusCode == HttpStatusCode.NotFound)
									{
										Console.WriteLine($"\tFile not found on server (404), will be skipped");
										urlsNotFound404.Add(url);
										File.WriteAllLines(URLS_NOT_FOUND_404_FILE, urlsNotFound404);
									}
									else
									{
										Console.WriteLine($"\tWeb exception {((HttpWebResponse)wex.Response).StatusCode}: {wex} !");
									}
								}
							}

							if (fileDownloadedNow)
							{
								Console.WriteLine($"\tSleeping a bit...");
								Thread.Sleep(3000);
							}
						}
					}
					Console.WriteLine($" *** Done downloading.\n");
				}
				else
				{
					Console.WriteLine($" *** Warning: no year URL template set to true (will not download anything)!\n");
				}
			}
		}
	}
}