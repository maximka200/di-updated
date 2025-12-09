namespace TagsCloudContainer.Core;

public class SourceSettings(string path, string format)
{
    public string Path { get; } = path;
    public string Format { get; } = format; 
}