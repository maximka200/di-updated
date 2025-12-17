using SixLabors.ImageSharp;

namespace TagsCloudContainer.Core;

internal static class GeometryConversions
{
    public static System.Drawing.Point ToDrawingPoint(this Point p) =>
        new(p.X, p.Y);

    public static System.Drawing.Size ToDrawingSize(this Size s) =>
        new(s.Width, s.Height);

    public static Rectangle ToImageSharpRectangle(this System.Drawing.Rectangle r) =>
        new(r.X, r.Y, r.Width, r.Height);
}