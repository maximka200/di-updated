using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public class LayoutService(ICloudPositionedTags layouter) : ILayoutService
{
    private readonly ICloudPositionedTags layouter = layouter ?? throw new ArgumentNullException(nameof(layouter));

    public IReadOnlyCollection<PositionedTag> Layout(TagCloudGenerationRequest request, IReadOnlyCollection<Tag> tags)
    {
        var ff = FontFamilyResolver.Resolve(request.Font);
        var settings = request.LayoutSettings;
        return layouter
            .GetPositionedTags(tags, settings.MinFontSize, settings.MaxFontSize, request.Desc, ff)
            .ToList();
    }
}