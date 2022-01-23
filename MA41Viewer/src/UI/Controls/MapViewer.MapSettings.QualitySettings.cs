using System;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace MA41Viewer.UI.Controls
{
	public partial class MapSettings
	{
		// thumbnail size/quality stuff

		public static readonly uint[] THUMBNAILS_SIZES = { 128, 256, 512, 1024, 2048, 4096 };

		private static readonly Func<float, uint> GetThumbnailSize_LowQuality = (tileLength) => tileLength switch
		{
			< 256 => 128,
			< 512 => 256,
			< 1024 => 512,
			< 2048 => 1024,
			< 4096 => 2048,
			< 8192 => 4096,
			_ => 0u,
		};

		private static readonly Func<float, uint> GetThumbnailSize_MediumQuality = (tileLength) => tileLength switch
		{
			<= 128 => 128,
			<= 256 => 256,
			<= 512 => 512,
			<= 1024 => 1024,
			<= 2048 => 2048,
			<= 4096 => 4096,
			_ => 0u,
		};

		private static readonly Func<float, uint> GetThumbnailSize_HighQuality = (tileLength) => tileLength switch
		{
			< 64 => 128,
			< 256 - 64 => 256,
			< 512 - 64 => 512,
			< 1024 - 64 => 1024,
			< 2048 - 64 => 2048,
			< 4096 - 64 => 4096,
			_ => 0u,
		};

		public static bool TryGetRecommendedThumbnailSize(float tileLengthPx, uint qualityLevel, out uint recommendedThumbnailSize)
		{
			var qualityFunction = qualityLevel == 1 ? GetThumbnailSize_LowQuality : (qualityLevel == 2 ? GetThumbnailSize_MediumQuality : GetThumbnailSize_HighQuality);
			recommendedThumbnailSize = qualityFunction(tileLengthPx);
			return recommendedThumbnailSize != 0u;
		}

		// inner classes

		public class QualitySettings
		{
			public static readonly QualitySettings LOW = new(CompositingMode.SourceOver, CompositingQuality.HighSpeed, InterpolationMode.NearestNeighbor, PixelOffsetMode.Half, SmoothingMode.HighSpeed, TextRenderingHint.SingleBitPerPixel, 1);
			public static readonly QualitySettings MEDIUM = new(CompositingMode.SourceOver, CompositingQuality.HighSpeed, InterpolationMode.Low, PixelOffsetMode.HighSpeed, SmoothingMode.Default, TextRenderingHint.AntiAlias, 2);
			public static readonly QualitySettings HIGH = new(CompositingMode.SourceOver, CompositingQuality.HighQuality, InterpolationMode.HighQualityBicubic, PixelOffsetMode.HighQuality, SmoothingMode.AntiAlias, TextRenderingHint.ClearTypeGridFit, 3);

			public CompositingMode CompositingModeValue { get; private set; }
			public CompositingQuality CompositingQualityValue { get; private set; }
			public InterpolationMode InterpolationModeValue { get; private set; }
			public PixelOffsetMode PixelOffsetModeValue { get; private set; }
			public SmoothingMode SmoothingModeValue { get; private set; }
			public TextRenderingHint TextRenderingHintValue { get; private set; }
			public uint ThumbnailQualityLevel { get; private set; }

			public QualitySettings()
			{
				SetFrom(HIGH);
			}

			public QualitySettings(CompositingMode compositingModeValue, CompositingQuality compositingQualityValue, InterpolationMode interpolationModeValue, PixelOffsetMode pixelOffsetModeValue, SmoothingMode smoothingModeValue, TextRenderingHint textRenderingHintValue, uint thumbnailQualityLevel)
			{
				SetFrom(compositingModeValue, compositingQualityValue, interpolationModeValue, pixelOffsetModeValue, smoothingModeValue, textRenderingHintValue, thumbnailQualityLevel);
			}

			public QualitySettings SetFrom(QualitySettings settings)
			{
				return SetFrom(settings.CompositingModeValue, settings.CompositingQualityValue, settings.InterpolationModeValue, settings.PixelOffsetModeValue, settings.SmoothingModeValue, settings.TextRenderingHintValue, settings.ThumbnailQualityLevel);
			}

			public QualitySettings SetFrom(CompositingMode compositingModeValue, CompositingQuality compositingQualityValue, InterpolationMode interpolationModeValue, PixelOffsetMode pixelOffsetModeValue, SmoothingMode smoothingModeValue, TextRenderingHint textRenderingHintValue, uint thumbnailQualityLevel)
			{
				CompositingModeValue = compositingModeValue;
				CompositingQualityValue = compositingQualityValue;
				InterpolationModeValue = interpolationModeValue;
				PixelOffsetModeValue = pixelOffsetModeValue;
				SmoothingModeValue = smoothingModeValue;
				TextRenderingHintValue = textRenderingHintValue;
				ThumbnailQualityLevel = thumbnailQualityLevel;
				return this;
			}

			//public override string ToString() => $"{(int)CompositingModeValue},{(int)CompositingQualityValue},{(int)InterpolationModeValue},{(int)PixelOffsetModeValue},{(int)SmoothingModeValue},{(int)TextRenderingHintValue},{(ThumbnailQualityHigher ? 1 : 0)}";
		}
	}
}
