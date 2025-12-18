using System.Globalization;
using SixLabors.ImageSharp.PixelFormats;
using TagsCloudContainer.Core;
using Color = SixLabors.ImageSharp.Color;

namespace TagsCloudContainer.Сlients.Console;

internal static class ConsoleOptionsParser
{
    private const int BaseWidth = 800;
    private const int BaseHeight = 800;

    public static bool TryParse(string[] args, out ConsoleOptions options, out string error)
    {
        options = default;
        error = string.Empty;

        if (args.Length == 0 || args.Contains("-h") || args.Contains("--help"))
        {
            error = "help";
            return false;
        }

        try
        {
            var flags = ParseFlags(args);

            var inputPath = Require(flags, CliFlags.Input);
            inputPath = Path.GetFullPath(inputPath);
            EnsureFileExists(inputPath, "Входной файл");

            var outputPath = Get(flags, CliFlags.Output, "cloud.png");

            var width = GetInt(flags, CliFlags.Width, BaseWidth, v => v > 0, CliFlags.Width);
            var height = GetInt(flags, CliFlags.Height, BaseHeight, v => v > 0, CliFlags.Height);

            var centerX = GetInt(flags, CliFlags.CenterX, width / 2, v => v >= 0, CliFlags.CenterX);
            var centerY = GetInt(flags, CliFlags.CenterY, height / 2, v => v >= 0, CliFlags.CenterY);

            var stopWordsPath = Get(flags, CliFlags.StopWords, Path.Combine(AppContext.BaseDirectory, "stop-words.txt"));
            stopWordsPath = Path.GetFullPath(stopWordsPath);

            var minFont = GetFloat(flags, CliFlags.MinFont, 10f, v => v > 0, CliFlags.MinFont);
            var maxFont = GetFloat(flags, CliFlags.MaxFont, 60f, v => v > 0, CliFlags.MaxFont);
            if (minFont > maxFont) throw new Exception("Некорректные значения шрифтов: min > max");

            var sourceType = Get(flags, CliFlags.SourceType, "txt");

            var bg = GetColor(flags, CliFlags.Bg, Color.White);
            var fg = GetColor(flags, CliFlags.Fg, Color.Black);

            var outputFormat = Get(flags, CliFlags.Format, GetFormatFromPath(outputPath));
            EnsureFormatSupported(outputFormat);

            var fontFamily = Get(flags, CliFlags.FontFamily, "monospace");

            options = new ConsoleOptions(
                InputPath: inputPath,
                OutputPath: outputPath,
                Width: width,
                Height: height,
                CenterX: centerX,
                CenterY: centerY,
                StopWordsPath: stopWordsPath,
                MinFontSize: minFont,
                MaxFontSize: maxFont,
                SourceType: sourceType,
                OutputFormat: outputFormat,
                BackgroundColor: bg,
                TextColor: fg,
                FontFamily: fontFamily
            );

            return true;
        }
        catch (Exception e)
        {
            error = e.Message;
            return false;
        }
    }
    
    private static Dictionary<string, string?> ParseFlags(string[] args)
    {
        var flags = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

        for (var i = 0; i < args.Length; i++)
        {
            var a = args[i];

            if (!a.StartsWith("--", StringComparison.Ordinal))
                throw new Exception($"Ожидался флаг, но получено: '{a}'");
            
            if (a.Contains('=', StringComparison.Ordinal))
            {
                var idx = a.IndexOf('=', StringComparison.Ordinal);
                Put(flags, a[..idx], a[(idx + 1)..]);
                continue;
            }
            
            if (i + 1 >= args.Length || args[i + 1].StartsWith("--", StringComparison.Ordinal))
                throw new Exception($"Ожидалось значение после {a}");

            Put(flags, a, args[++i]);
        }

        return flags;
    }

    private static void Put(IDictionary<string, string?> flags, string key, string? value)
    {
        if (!CliFlags.All.Contains(key, StringComparer.OrdinalIgnoreCase))
            throw new Exception($"Неизвестный флаг: {key}");

        if (!flags.TryAdd(key, value))
            throw new Exception($"Флаг указан дважды: {key}");

        if (string.IsNullOrWhiteSpace(value))
            throw new Exception($"Пустое значение для {key}");
    }
    
    private static string Require(IReadOnlyDictionary<string, string?> flags, string key)
    {
        if (!flags.TryGetValue(key, out var v) || string.IsNullOrWhiteSpace(v))
            throw new Exception($"Обязательный параметр не задан: {key}");
        return v.Trim();
    }

    private static string Get(IReadOnlyDictionary<string, string?> flags, string key, string def)
        => flags.TryGetValue(key, out var v) && !string.IsNullOrWhiteSpace(v) ? v.Trim() : def;

    private static int GetInt(
        IReadOnlyDictionary<string, string?> flags,
        string key,
        int def,
        Func<int, bool> isValid,
        string label)
    {
        if (!flags.TryGetValue(key, out var v) || string.IsNullOrWhiteSpace(v)) return def;
        if (!int.TryParse(v.Trim(), out var n)) throw new Exception($"Некорректный {label}: {v}");
        if (!isValid(n)) throw new Exception($"Некорректный {label}: {n}");
        return n;
    }

    private static float GetFloat(
        IReadOnlyDictionary<string, string?> flags,
        string key,
        float def,
        Func<float, bool> isValid,
        string label)
    {
        if (!flags.TryGetValue(key, out var v) || string.IsNullOrWhiteSpace(v)) return def;
        if (!float.TryParse(v.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var f))
            throw new Exception($"Некорректный {label}: {v}");
        if (!isValid(f)) throw new Exception($"Некорректный {label}: {f}");
        return f;
    }

    private static Color GetColor(IReadOnlyDictionary<string, string?> flags, string key, Color def)
    {
        if (!flags.TryGetValue(key, out var v) || string.IsNullOrWhiteSpace(v)) return def;
        if (!TryParseColor(v, out var c))
            throw new Exception($"Некорректный {key}: {v}. Пример: {key} #ffffff");
        return c;
    }
    

    private static void EnsureFileExists(string path, string name)
    {
        if (!File.Exists(path))
            throw new Exception($"{name} не найден: {path}");
    }

    private static void EnsureFormatSupported(string fmt)
    {
        var f = fmt.Trim().ToLowerInvariant();
        if (!WordsSourceFactory.SourceFormats.Contains(f))
            throw new Exception($"Неподдерживаемый формат: {fmt} (доступно: png, jpg, jpeg, bmp)");
    }

    private static string GetFormatFromPath(string outputPath)
    {
        var ext = Path.GetExtension(outputPath);
        return string.IsNullOrWhiteSpace(ext) ? "png" : ext.TrimStart('.').ToLowerInvariant();
    }

    private static bool TryParseColor(string? s, out Color color)
    {
        color = default;
        if (string.IsNullOrWhiteSpace(s)) return false;

        var v = s.Trim();

        if (v.Equals("white", StringComparison.OrdinalIgnoreCase)) { color = Color.White; return true; }
        if (v.Equals("black", StringComparison.OrdinalIgnoreCase)) { color = Color.Black; return true; }

        if (v.StartsWith("#", StringComparison.Ordinal)) v = v[1..];
        if (v.Length != 6 && v.Length != 8) return false;

        byte a = 255, r, g, b2;
        if (v.Length == 6)
        {
            if (!TryByte(v[..2], out r)) return false;
            if (!TryByte(v[2..4], out g)) return false;
            if (!TryByte(v[4..6], out b2)) return false;
        }
        else
        {
            if (!TryByte(v[..2], out a)) return false;
            if (!TryByte(v[2..4], out r)) return false;
            if (!TryByte(v[4..6], out g)) return false;
            if (!TryByte(v[6..8], out b2)) return false;
        }

        color = new Rgba32(r, g, b2, a);
        return true;

        static bool TryByte(string hex, out byte b) =>
            byte.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out b);
    }
}
