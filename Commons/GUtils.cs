using System;
using System.Drawing;

namespace Commons
{
	public static class GUtils
	{
		/// <summary>
		/// <para>Gets a Font with its size adjusted to fit iun the given container size.</para>
		/// <para>From https://msdn.microsoft.com/en-us/library/bb986765.aspx via https://stackoverflow.com/questions/15571715/auto-resize-font-to-fit-rectangle. Modified slightly.</para>
		/// </summary>
		public static Font GetAdjustedFont(this Graphics g, string text, Font baseFont, float containerWidth, int maxFontSize, int minFontSize, bool smallestOnFail = true)
		{
			Font font = null;

			// We use MeasureString which we get via a control instance           
			for (int adjustedSize = maxFontSize; adjustedSize >= minFontSize; adjustedSize--)
			{
				font = new Font(baseFont.Name, adjustedSize, baseFont.Style);

				// Test the string with the new size
				SizeF adjustedSizeNew = g.MeasureString(text, font);

				if (containerWidth > Convert.ToInt32(adjustedSizeNew.Width))
					return font;
			}

			// If you get here, there was no fontsize that worked; return minimumSize or original?
			return smallestOnFail ? font : baseFont;
		}

		public static float GetXForCenteredText(this RectangleF rectangle, float textWidth)
			=> rectangle.Left + rectangle.Width / 2f - textWidth / 2f;

		public static float GetYForCenteredText(this RectangleF rectangle, float textHeight)
			=> rectangle.Top + rectangle.Height / 2f - textHeight / 2f;
	}
}
