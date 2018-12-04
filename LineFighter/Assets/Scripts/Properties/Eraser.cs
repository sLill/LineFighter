public static class Eraser
{
	public enum EraserSize
    {
        Small,
        Medium,
        Large
    }
	
	public static EraserSize Size { get; set; }
	
    public static float Radius { get; set; }
	
	public static float RefillRate { get; set; }
}
