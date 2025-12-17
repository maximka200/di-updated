using SixLabors.ImageSharp;
using TagCloud;
using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public sealed class CircularCloudLayouterWrapper(Point center) : ICircularCloudLayouterWrapper
{
    private readonly CircularCloudLayouter inner = new(center.ToDrawingPoint());

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        var rect = inner.PutNextRectangle(rectangleSize.ToDrawingSize());
        return rect.ToImageSharpRectangle();
    }
}