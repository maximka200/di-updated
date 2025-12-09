using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public class LowerCaseNormalizer : IWordNormalizer
{
    public string Normalize(string word) => word.ToLowerInvariant();
}