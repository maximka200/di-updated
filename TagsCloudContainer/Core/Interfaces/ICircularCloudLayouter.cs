using System.Drawing;

namespace TagsCloudContainer.Core.Interfaces;

public interface ICircularCloudLayouter
{
    Rectangle PutNextRectangle(Size rectangleSize);
}