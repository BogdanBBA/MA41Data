using System.Collections.Generic;

namespace MA41Viewer.Data
{
	public static class Datasets
	{
		public record DatasetInfo(uint Year, string YearDescription, string URL, string DatasetDescription);

		private static readonly Dictionary<uint, DatasetInfo> _MAPPING = new()
		{
			{ 0, new DatasetInfo(0, "? !",
				string.Empty,
				"INVALID YEAR!") },
			{ 1938, new DatasetInfo(1938, "Oct-Nov '38",
				"https://www.data.gv.at/katalog/de/dataset/stadt-wien_luftbildplanwien1938",
				"The Vienna aerial image catalogue from the year 1938 covers the entire city area with a pixel size of 20 centimetres. The aerial images were taken in October and November 1938.") },
			{ 1956, new DatasetInfo(1956, "Apr '56",
				"https://www.data.gv.at/katalog/de/dataset/stadt-wien_luftbildplanwien1956",
				"The Vienna aerial image catalogue from the year 1956 covers the entire city area with a pixel size of 50 centimetres. The aerial images were taken in April 1956.") },
			{ 1961, new DatasetInfo(1961, "Sep '61",
				"https://www.data.gv.at/katalog/dataset/luftbildplan-1961-wien",
				"The Vienna aerial image catalogue from the year 1961 covers the entire city area. The pixel size oft the black and white aerial images is 25 centimetres. They were taken on September 1st, 1961.") },
			{ 1971, new DatasetInfo(1971, "May '71",
				"https://www.data.gv.at/katalog/dataset/luftbildplan-1971-wien",
				"The Vienna aerial image catalogue from the year 1971 covers the entire city area. The pixel size oft the black and white aerial images is 25 centimetres. They were taken on Mai 14th an Mai 16th, 1971.") },
			{ 1972, new DatasetInfo(1972, "May-Sep '71",
				"https://www.data.gv.at/katalog/dataset/luftbildplan-1971-umland-wien",
				"The Vienna surrounding countryside aerial image catalogue from the year 1961 covers the entire city area and the surrounding countryside of lower austria. The pixel size oft the black and white aerial images is 50 centimetres. They were taken on Mai 20th and September 23rd, 1971.") },
			{ 1976, new DatasetInfo(1976, "May '76",
				"https://www.data.gv.at/katalog/de/dataset/luftbildplan-1976-wien",
				"The aerial image catalogue from the year 1976 covers the entire city area of Vienna with a pixel size of 25 centimetres. The aerial images were taken on the 8th and 9th of May 1976.") },
			{ 1981, new DatasetInfo(1981, "May '81",
				"https://www.data.gv.at/katalog/dataset/luftbildplan-1981-wien",
				"The Vienna aerial image catalogue from the year 1981 covers the entire city area. The pixel size oft the black and white aerial images is 50 centimetres. They were taken on Mai 19th, 1981.") },
			{ 1986, new DatasetInfo(1986, "Oct '86",
				"https://www.data.gv.at/katalog/dataset/luftbildplan-1986-wien",
				"The Vienna aerial image catalogue from the year 1986 covers the entire city area. The pixel size oft the black and white aerial images is 50 centimetres. They were taken on October 2nd, 1986.") },
			{ 1992, new DatasetInfo(1992, "May '92",
				"https://www.data.gv.at/katalog/de/dataset/luftbildplan-1992-wien",
				"The aerial image catalogue from the year 1992 covers the entire city area of Vienna with a pixel size of 50 centimetres. The aerial images were taken on the 25th of May 1992.") },
			{ 2014, new DatasetInfo(2014, "Jun '14",
				"https://www.data.gv.at/katalog/de/dataset/stadt-wien_orthofotowien2014",
				"The Vienna orthophoto from the year 2014 covers the entire city area with a pixel size of 15 centimeters. The aerial images were taken on 6th and 7th of June 2014.") },
			{ 2019, new DatasetInfo(2019, "Jun '19",
				"https://www.data.gv.at/katalog/dataset/orthofoto-2019-wien",
				"The Vienna orthophoto from the year 2019 covers the entire city area with a pixel size of 15 centimeters. The aerial images were taken on 4th, 5th and 10th of June 2019.") },
			{ 2024, new DatasetInfo(2024, "Mar-Apr '24",
				"https://www.data.gv.at/katalog/dataset/orthofoto-2024-wien",
				"The Vienna orthophoto of the year 2024 is a true-orthofoto and covers the entire city area with a pixel size of 15 centimeters. The aerial images were taken on the 19th, 20th and 29th of March and the 12th of April 2024.") },
		};

		public static DatasetInfo GetInfoByYear(uint year)
			=> _MAPPING[_MAPPING.ContainsKey(year) ? year : 0];
	}
}
