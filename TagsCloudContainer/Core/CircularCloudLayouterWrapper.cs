using System.Drawing;
using TagCloud;
using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public class CircularCloudLayouterWrapper(Point center) : CircularCloudLayouter(center),
    ICircularCloudLayouterWrapper;