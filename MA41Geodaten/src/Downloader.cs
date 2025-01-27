using MA41.Commons;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MA41Geodaten
{
	public static class Downloader
	{
		public static readonly List<(int page, int part)> COORDINATES = Enumerable.Range(1, 5).SelectMany(x => Enumerable.Range(2, 8).SelectMany(y => Enumerable.Range(1, 4).Select(q => (x * 10 + y, q)))).ToList();

		public static readonly string URL_ROOT = "https://www.wien.gv.at/ma41datenviewer/downloads/geodaten";

		public static readonly Dictionary<string, bool> YEAR_URL_TEMPLATES = new()
		{
			{ "{0}/op_img/{1}_{2}_op_2024.zip", true },
			{ "{0}/op_img/{1}_{2}_op_2021.zip", false },
			{ "{0}/op_img/{1}_{2}_op_2020.zip", false },
			{ "{0}/op_img/{1}_{2}_op_2019.zip", false },
			{ "{0}/op_img/{1}_{2}_op_2018.zip", false },
			{ "{0}/op_img/{1}_{2}_op_2017.zip", false },
			{ "{0}/op_img/{1}_{2}_op_2016.zip", false },
			{ "{0}/op_img/{1}_{2}_op_2015.zip", false },
			{ "{0}/op_img/{1}_{2}_op2014.zip", false },
			{ "{0}/lb_img/{1}_{2}_lb1992.zip", false },
			{ "{0}/lb_img/{1}_{2}_lb1986.zip", true },
			{ "{0}/lb_img/{1}_{2}_lb1981.zip", true },
			{ "{0}/lb_img/{1}_{2}_lb1976.zip", false },
			{ "{0}/lb_img/{1}_{2}_lb1971ul.zip", true },
			{ "{0}/lb_img/{1}_{2}_lb1971.zip", true },
			{ "{0}/lb_img/{1}_{2}_lb1961.zip", true },
			{ "{0}/lb_img/{1}_{2}_lb1956.zip", false },
			{ "{0}/lb_img/{1}_{2}_lb1938.zip", false }
		};

		public static readonly string URLS_NOT_FOUND_404_FILE = Path.Combine(Paths.DOWNLOAD_FOLDER, @"404s.txt");

		private class TimedProgressReport
		{
			private DateTime _lastReport;
			private readonly IProgress<double> _progress;
			private readonly object _lock = new();

			public TimedProgressReport(int seconds)
			{
				_lastReport = DateTime.Now.Subtract(TimeSpan.FromSeconds(seconds));
				_progress = new Progress<double>(p =>
				{
					if (p == 0.0)
						Console.Write("\tDownloading... ");
					else if (p == 100.0)
						Console.WriteLine("done");
					else
					{
						DateTime now = DateTime.Now;
						lock (_lock)
						{
							if ((now - _lastReport).TotalSeconds >= 2.0)
							{
								Console.Write($"{p:F1}%... ");
								_lastReport = now;
							}
						}
					}
				});
			}

			public void Report(long items, long total)
				=> _progress.Report(items == 0 || total == 0 ? 0.0 : (items == total ? 100.0 : (double)items / total * 100.0));
		}

		static async Task DownloadFile(HttpClient web, string url, string destinationFilePath, TimedProgressReport progress)
		{
			using (HttpResponseMessage response = await web.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
			{
				response.EnsureSuccessStatusCode();

				using (FileStream fileStream = new(destinationFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
				using (Stream httpStream = await response.Content.ReadAsStreamAsync())
				{
					long totalBytes = response.Content.Headers.ContentLength ?? -1L;
					byte[] buffer = new byte[81920];
					long totalRead = 0;
					int bytesRead;

					progress.Report(0, 0);
					while ((bytesRead = await httpStream.ReadAsync(buffer)) > 0)
					{
						await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead));
						totalRead += bytesRead;
						progress.Report(totalRead, totalBytes);
					}
				}
			}
		}

		public static async Task Download()
		{
			Console.WriteLine($" *** Downloading...");
			Console.WriteLine($" *** Page coordinates: {string.Join(", ", COORDINATES.Select(coord => $"{coord.page}/{coord.part}"))}.\n");
			Console.WriteLine($" *** All URL templates: {string.Join(", ", YEAR_URL_TEMPLATES.Select(template => $"{template.Key} ({template.Value})"))}.\n");

			List<string> urlsNotFound404 = File.Exists(URLS_NOT_FOUND_404_FILE) ? [.. File.ReadAllLines(URLS_NOT_FOUND_404_FILE)] : [];
			TimedProgressReport timedReport = new(2);

			using (HttpClient web = new())
			{
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
									await DownloadFile(web, url, file, timedReport);
									Console.WriteLine($"\tFile downloaded now ({new FileInfo(file).Length / 1024:N0} KB)");
								}
								catch (HttpRequestException rEx)
								{
									if (rEx.StatusCode == HttpStatusCode.NotFound)
									{
										Console.WriteLine($"\tFile not found on server (404), will be skipped");
										urlsNotFound404.Add(url);
										File.WriteAllLines(URLS_NOT_FOUND_404_FILE, urlsNotFound404);
									}
									else
									{
										Console.WriteLine($"\tHTTP web exception {rEx.StatusCode}: {rEx} !");
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