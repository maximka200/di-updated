using TagsCloudContainer.Сlients.Console.Parsing.Interfaces;
using Color = SixLabors.ImageSharp.Color;

namespace TagsCloudContainer.Сlients.Console.Parsing;

public class FlagReader(IReadOnlyDictionary<string, string?> flags)
{
    public string RequirePath(string key, string name)
    {
        var raw = RequireString(key);
        var full = Path.GetFullPath(raw);
        Ensure.FileExists(full, $"{name} не найден: {full}");
        return full;
    }

    public string RequireString(string key)
    {
        try
        {
            var v = flags[key];
            return Ensure.TrimmedNonEmpty(v, $"Обязательный параметр не задан: {key}");
        }
        catch (KeyNotFoundException)
        {
            throw new Exception($"Обязательный параметр не задан: {key}");
        }
    }

    public string GetString(string key, string def)
    {
        try
        {
            var v = flags[key];
            return Ensure.TrimmedOrDefault(v, def);
        }
        catch (KeyNotFoundException)
        {
            return def;
        }
    }

    public int GetInt(string key, int def, IIntRule rule)
    {
        try
        {
            var v = flags[key];
            return Ensure.ParseIntOrDefault(v, def, rule);
        }
        catch (KeyNotFoundException)
        {
            return def;
        }
    }

    public float GetFloat(string key, float def, IFloatRule rule)
    {
        try
        {
            var v = flags[key];
            return Ensure.ParseFloatOrDefault(v, def, rule);
        }
        catch (KeyNotFoundException)
        {
            return def;
        }
    }

    public Color GetColor(string key, Color def)
    {
        try
        {
            var v = flags[key];
            return Ensure.ParseColorOrDefault(key, v, def);
        }
        catch (KeyNotFoundException)
        {
            return def;
        }
    }
    
    public bool GetBool(string key, bool def)
    {
        try
        {
            var v = flags[key];
            return Ensure.ParseBoolOrDefault(v, def, key);
        }
        catch (KeyNotFoundException)
        {
            return def;
        }
    }
}