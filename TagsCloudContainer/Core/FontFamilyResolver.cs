using SixLabors.Fonts;

namespace TagsCloudContainer.Core;

public static class FontFamilyResolver
{
    private static readonly FontCollection PrivateFonts = new();

    private static FontFamily? cachedDefault;
    private static FontFamily? cachedMono;

    public static FontFamily Resolve(string? nameOrPath)
    {
        if (string.IsNullOrWhiteSpace(nameOrPath)) return cachedDefault ??= PickDefault();
        var v = nameOrPath.Trim();

        if (v.Equals("mono", StringComparison.OrdinalIgnoreCase) ||
            v.Equals("monospace", StringComparison.OrdinalIgnoreCase))
            return cachedMono ??= PickMonospace();

        if (File.Exists(v))
            return PrivateFonts.Add(v);
            
        if (SystemFonts.Collection.TryGet(v, out var sysFamily))
            return sysFamily;

        return cachedDefault ??= PickDefault();
    }

    private static FontFamily PickMonospace()
    {
        if (SystemFonts.Collection.TryGet("DejaVu Sans Mono", out var dejavu)) return dejavu;
        if (SystemFonts.Collection.TryGet("Menlo", out var menlo)) return menlo;
        
        var anyMono = SystemFonts.Collection.Families
            .FirstOrDefault(f => f.Name.Contains("mono", StringComparison.OrdinalIgnoreCase));

        return anyMono;
    }

    private static FontFamily PickDefault()
    {
        return SystemFonts.Collection.Families.FirstOrDefault();
    }
}