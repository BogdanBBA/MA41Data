using System.Drawing;

namespace Commons
{
	/// <summary>
	/// Holds information for a color which can be rendered into a pen or brush.
	/// </summary>
	public class ColorData
	{
		private readonly Color color;
		private Pen pen;
		private Brush brush;

		public ColorData(string htmlColor)
			: this(ColorTranslator.FromHtml(htmlColor)) { }

		public ColorData(Color pColor)
		{
			color = pColor;
			pen = null;
			brush = null;
		}

		public Color Color { get => color; }

		public Pen AsPen()
		{
			return pen ?? AsPen(1.0f);
		}

		public Pen AsPen(float width)
		{
			pen = new Pen(color, width);
			return pen;
		}

		public Brush AsBrush()
		{
			if (brush == null)
				brush = new SolidBrush(color);
			return brush;
		}
	}

	/// <summary>
	/// Holds color information that can be used on a UI control in the context of mouse events.
	/// </summary>
	public class ColorSet
	{
		public ColorData Normal { get; }
		public ColorData Hovered { get; }
		public ColorData Clicked { get; }

		public ColorSet(Color normal, Color hoveredAndClicked)
			: this(normal, hoveredAndClicked, hoveredAndClicked) { }

		public ColorSet(Color normal, Color hovered, Color clicked)
		{
			Normal = new ColorData(normal);
			Hovered = new ColorData(hovered);
			Clicked = new ColorData(clicked);
		}

		public ColorSet(string normal, string hoveredAndClicked)
			: this(normal, hoveredAndClicked, hoveredAndClicked) { }

		public ColorSet(string normal, string hovered, string clicked)
		{
			Normal = new ColorData(normal);
			Hovered = new ColorData(hovered);
			Clicked = new ColorData(clicked);
		}

		public ColorData Get(bool hovered, bool clicked)
		{
			if (clicked) return Clicked;
			if (hovered) return Hovered;
			return Normal;
		}
	}

	public class ColorSetBySelected
	{
		public ColorSet Selected { get; }
		public ColorSet NotSelected { get; }

		public ColorSetBySelected(ColorSet selected, ColorSet notSelected)
		{
			Selected = selected;
			NotSelected = notSelected;
		}

		public ColorSet Get(bool selected) => selected ? Selected : NotSelected;

		public ColorSet this[bool selected] => Get(selected);
	}
}
