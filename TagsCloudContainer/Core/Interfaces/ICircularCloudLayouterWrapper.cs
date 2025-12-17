using SixLabors.ImageSharp;

namespace TagsCloudContainer.Core.Interfaces;

public interface ICircularCloudLayouterWrapper
{
    Rectangle PutNextRectangle(Size rectangleSize);
}