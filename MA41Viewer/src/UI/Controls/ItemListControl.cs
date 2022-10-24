using Commons;
using MA41.Commons;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace MA41Viewer.src.UI.Controls
{
	public partial class ItemListControl : UserControl
	{
		private const bool DEBUG_MODE = false;

		public enum Orientations { Horizontal, Vertical }

		public static readonly Font DefaultTextFont = new("Roboto Slab", 20, FontStyle.Regular);
		public static readonly Font DefaultDescriptionFont = new("Roboto Slab", 9);

		public static readonly ColorSetBySelected BackgroundColorsRed = new(new ColorSet("#d9b168", "#cca050"), new ColorSet("#f5f4f2", "#f0eadf"));
		public static readonly ColorSetBySelected BackgroundColorsBlue = new(new ColorSet("#68a0d9", "#508ecc"), new ColorSet("#f2f4f5", "#dfe7f0"));
		public static readonly ColorSetBySelected TextColorsRed = new(new ColorSet("#d12c52", "#cc1b43", "#a11534"), new ColorSet("#524e4f", "#6e585d", "#99495c"));
		public static readonly ColorSetBySelected TextColorsBlue = new(new ColorSet("#2c7fd1", "#1b73cc", "#155ba1"), new ColorSet("#4e5052", "#58636e", "#497199"));

		public Orientations Orientation { get; set; } = Orientations.Horizontal;
		public List<StringWithTag<uint>> Items { get; set; } = new List<StringWithTag<uint>>() { new StringWithTag<uint>(1111, "ABCD"), new StringWithTag<uint>(2222, "WXYZ"), new StringWithTag<uint>(1234, "1234") };
		public int SelectedItemIndex { get; set; } = 1;
		public Font TextFont { get; set; } = DefaultTextFont;
		public Font DescriptionFont { get; set; } = DefaultDescriptionFont;
		public ColorSetBySelected BackgroundColors { get; private set; }
		public ColorSetBySelected TextColors { get; private set; }
		public Action<uint> OnSelectedItemChanged { get; set; } = null;

		private Point? mouseLocation = null;
		private bool mouseIsDown = false;

		public ItemListControl() : base()
		{
			InitializeComponent();
			DoubleBuffered = true;
		}

		public void SetColors(ColorSetBySelected background, ColorSetBySelected text)
		{
			BackgroundColors = background;
			TextColors = text;
		}

		private int GetItemIndexByLocation(Point? location)
		{
			if (location == null || location?.X < 0 || location?.Y < 0 || location?.X > Width || location?.Y > Height)
				return -1;
			return Orientation switch
			{
				Orientations.Horizontal => location.Value.X / (Width / Items.Count),
				Orientations.Vertical => location.Value.Y / (Height / Items.Count),
				_ => -1
			};
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			mouseLocation = e.Location;
			Invalidate();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			mouseLocation = null;
			Invalidate();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			mouseIsDown = true;
			Invalidate();
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			mouseIsDown = false;
			SelectedItemIndex = GetItemIndexByLocation(e.Location);
			Invalidate();

			if (SelectedItemIndex >= 0 && SelectedItemIndex < Items.Count)
				OnSelectedItemChanged?.Invoke(Items[SelectedItemIndex].Tag);
		}

		protected Font GetFontToUse(Graphics g, Font font, Size cellSize, int hPadding, int vPadding)
		{
			if (Items.Count == 0) return font;
			string longestItem = Items.OrderBy(item => item.String.Length).First().String;
			bool textFitsInCell;
			do
			{
				Size size = g.MeasureString(longestItem, font).ToSize();
				textFitsInCell = cellSize.Width >= size.Width + 2 * hPadding && cellSize.Height >= size.Height + 2 * vPadding;
				if (!textFitsInCell) font = new Font(font.FontFamily, font.Size - 1, font.Style);
			} while (!textFitsInCell);
			return font;
		}

		private PointF GetDrawLocation(Graphics g, Rectangle cell, string @string, Font font, float verticalWeight, VerticalAlignment alignment)
		{
			if (alignment == VerticalAlignment.Center) throw new ArgumentException("Not what I had in mind");
			if (verticalWeight <= 0.0 || verticalWeight > 1.0) throw new ArgumentException("Not what I had in mind");

			SizeF size = g.MeasureString(@string, font);
			float x = cell.Left + cell.Width / 2f - size.Width / 2f;
			float y;
			if (alignment == VerticalAlignment.Top) // to top
			{
				float weightedY = cell.Bottom - cell.Height * verticalWeight;
				float weightedH = cell.Bottom - weightedY;
				y = size.Height <= weightedH ? weightedY : cell.Bottom - size.Height;
			}
			else // to bottom
			{
				float weightedY = cell.Top + cell.Height * verticalWeight;
				float weightedH = weightedY - cell.Top;
				y = size.Height <= weightedH ? weightedY - size.Height : cell.Top;
			}
			if (DEBUG_MODE) g.DrawRectangle(Pens.DarkGray, new Rectangle((int)x, (int)y, (int)size.Width, (int)size.Height));
			return new(x, y);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			var g = e.Graphics;
			g.TextRenderingHint = TextRenderingHint.AntiAlias;
			if (Items.Count == 0) g.Clear(BackgroundColors.NotSelected.Normal.Color);

			const float textWeight = 0.65f;
			int hoveredItemIndex = GetItemIndexByLocation(mouseLocation);
			Size cellSize = Orientation == Orientations.Horizontal ? new Size(Width / Items.Count, Height) : new Size(Width, Height / Items.Count);
			Font textFont = GetFontToUse(g, TextFont, cellSize, 5, 2);
			Font descriptionFont = GetFontToUse(g, DescriptionFont, new Size(cellSize.Width, (int)(cellSize.Height * (1.0f - textWeight))), 0, -2);
			PointF location;

			for (int index = 0, x = 0, y = 0; index < Items.Count; index++)
			{
				bool thisCellHovered = index == hoveredItemIndex;
				bool thisCellSelected = index == SelectedItemIndex;
				Rectangle cell = new Rectangle(x, y, cellSize.Width, cellSize.Height);

				g.FillRectangle(BackgroundColors[thisCellSelected].Get(thisCellHovered, thisCellHovered && mouseIsDown).AsBrush(), cell);

				if (Items[index].Description != null)
				{
					location = GetDrawLocation(g, cell, Items[index].Description, descriptionFont, 1.0f - textWeight, VerticalAlignment.Top);
					g.DrawString(Items[index].Description, descriptionFont, TextColors[thisCellSelected].Get(thisCellHovered, thisCellHovered && mouseIsDown).AsBrush(), location);
				}

				location = GetDrawLocation(g, cell, Items[index].String, textFont, textWeight, VerticalAlignment.Bottom);
				g.DrawString(Items[index].String, textFont, TextColors[thisCellSelected].Get(thisCellHovered, thisCellHovered && mouseIsDown).AsBrush(), location);

				if (Orientation == Orientations.Horizontal) x += cellSize.Width;
				if (Orientation == Orientations.Vertical) y += cellSize.Height;
			}
		}
	}
}
