using MA41.Commons;
using System.Globalization;

internal record MinMax<TYPE>(TYPE Min, TYPE Max);

internal class Square(int year, string jpgPath, string jgwPath)
{
	public record JgwData(double PixelSizeX, double PixelSizeY, double RotationRow, double RotationCol, double CoordinateX, double CoordinateY)
	{
		public JgwData(double[] values) : this(values[0], values[3], values[1], values[2], values[4], values[5]) { }
	};

	public int Year { get; private set; } = year;
	public string JpgPath { get; private set; } = jpgPath;
	public string JgwPath { get; private set; } = jgwPath;
	public JgwData Data { get; private set; } = new JgwData(File.ReadAllLines(jgwPath).Select(double.Parse).ToArray());

	public override string ToString() => $"{Year}: {Path.GetFileNameWithoutExtension(JgwPath)}";
}

internal class Program
{
	private static List<(int year, string JPG, string JGW)> GetJpgJgwPair()
	{
		string[] yearFolders = Directory.GetDirectories(Paths.IMAGES_FOLDER, "*", SearchOption.TopDirectoryOnly);
		return yearFolders.SelectMany(yearFolder =>
		{
			int year = int.Parse(Path.GetFileName(yearFolder));
			List<string> jgwFiles = [.. Directory.GetFiles(yearFolder, "*.jgw").Order()];
			List<string> jpgFiles = jgwFiles.Select(jgwFile => jgwFile.Replace(".jgw", ".jpg")).ToList();
			jpgFiles.ForEach(jpgFile =>
			{
				if (!File.Exists(jpgFile))
					throw new Exception($"jpg file {jpgFile} does not exist!");
			});
			return Enumerable.Range(0, jgwFiles.Count).Select(index => (year, jpgFiles[index], jgwFiles[index])).ToList();
		}).ToList();
	}

	private static void XXX(Range zoomRange, MinMax<double> latRange, MinMax<double> lonRange)
	{
		(double Lat, double Lon) TileXYToLatLong(int x, int y, int zoom)
		{
			double lng = x / Math.Pow(2, zoom) * 360 - 180;
			double n = Math.PI - (2 * Math.PI * y) / Math.Pow(2, zoom);
			double lat = Math.Atan(Math.Sinh(n)) * (180.0 / Math.PI);
			return (lat, lng);
		}

		for (int zoom = zoomRange.Start.Value; zoom <= zoomRange.End.Value; zoom++)
		{
			for (int x = 0; x < Math.Pow(2, zoom); x++)
			{
				for (int y = 0; y < Math.Pow(2, zoom); y++)
				{
					(double Lat, double Lon) = TileXYToLatLong(x, y, zoom);
				}
			}
		}
	}

	/// <summary>
	/// The purpose here is to redraw the dumb tiles from MA41 into tiles as strcutured in online APIs such as Google Maps'.<br/>
	/// The source structure is squares whose coordinates are defined in the JGW files.<br/>
	/// The destination structure is squares which are indexed based on the zoom level and their zoom-specific distance from the reference lines.<br/>
	/// <list type="number">
	/// <item>Read existing JPG and JGW data.</item>
	/// <item>Define a zoom range and a geographical lat/lon range to be output.</item>
	/// <item>Determine which tiles are included in the defined area (for each zoom level), and associate with their coordinate bounds.</item>
	/// <item>For each determined tile, find which MA41 squares are included and redraw them appropriately; save to file.</item>
	/// </list>
	/// </summary>
	private static void Main(string[] _)
	{
		// initialize

		CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
		Paths.Initialize();

		// read existing data

		List<Square> squares = GetJpgJgwPair().Select(pair => new Square(pair.year, pair.JPG, pair.JGW)).ToList();
		Console.WriteLine($"Read {squares.Count} squares (sample: {string.Join(", ", squares.Take(3))})");

		// defined desired zoom range and area

		Range zoomRange = new(11, 17);
		(MinMax<double> latRange, MinMax<double> lonRange) = (new(16.3, 16.45), new(48.15, 48.25));

		// determine tiles

		XXX(zoomRange, latRange, lonRange);

		// redraw
	}
}