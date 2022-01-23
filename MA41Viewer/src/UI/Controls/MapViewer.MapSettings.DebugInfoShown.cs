namespace MA41Viewer.UI.Controls
{
	public partial class MapSettings
	{
		public class DebugInfoShown
		{
			public bool MouseCursorInfo { get; set; } = true;
			public bool DrawingQualityInfo { get; set; } = true;
			public bool MemoryAllocationInfo { get; set; } = true;
			public bool DetailedTileInfo { get; set; } = true;

			public DebugInfoShown()
				: this(true, true, true, true) { }

			public DebugInfoShown(bool mouseCursorInfo, bool drawingQualityInfo, bool memoryAllocationInfo, bool detailedTileInfo)
			{
				SetFrom(mouseCursorInfo, drawingQualityInfo, memoryAllocationInfo, detailedTileInfo);
			}

			public void SetFrom(bool mouseCursorInfo, bool drawingQualityInfo, bool memoryAllocationInfo, bool detailedTileInfo)
			{
				MouseCursorInfo = mouseCursorInfo;
				DrawingQualityInfo = drawingQualityInfo;
				MemoryAllocationInfo = memoryAllocationInfo;
				DetailedTileInfo = detailedTileInfo;
			}

			public override string ToString()
				=> $"{MouseCursorInfo},{DrawingQualityInfo},{MemoryAllocationInfo},{DetailedTileInfo}";
		}
	}
}
