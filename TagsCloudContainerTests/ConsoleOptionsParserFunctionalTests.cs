using FluentAssertions;
using TagsCloudContainer.Сlients.Console;
using Color = SixLabors.ImageSharp.Color;

namespace TagsCloudContainerTests;

[TestFixture]
public class ConsoleOptionsParserFunctionalTests
{
    [Test]
    public void TryParse_WhenArgsAreEmpty_ShouldReturnHelp()
    {
        var ok = ConsoleOptionsParser.TryParse([], out var options, out var error);

        ok.Should().BeFalse();
        options.Should().BeNull();
        error.Should().Be("help");
    }

    [TestCase("-h")]
    [TestCase("--help")]
    public void TryParse_WhenHelpFlagProvided_ShouldReturnHelp(string helpFlag)
    {
        var ok = ConsoleOptionsParser.TryParse([helpFlag], out var options, out var error);

        ok.Should().BeFalse();
        options.Should().BeNull();
        error.Should().Be("help");
    }

    [Test]
    public void TryParse_WhenRequiredInputIsMissing_ShouldFail()
    {
        var tempDir = Directory.CreateTempSubdirectory("tags-cloud-tests-");
        try
        {
            var args = new[]
            {
                "--output", Path.Combine(tempDir.FullName, "cloud.png"),
            };

            var ok = ConsoleOptionsParser.TryParse(args, out var options, out var error);

            ok.Should().BeFalse();
            options.Should().BeNull();
            error.Should().Be("Обязательный параметр не задан: --input");
        }
        finally
        {
            TryDeleteDirectory(tempDir.FullName);
        }
    }

    [Test]
    public void TryParse_WhenInputFileDoesNotExist_ShouldFail()
    {
        var tempDir = Directory.CreateTempSubdirectory("tags-cloud-tests-");
        try
        {
            var missingInput = Path.Combine(tempDir.FullName, "missing.txt");

            var args = new[]
            {
                "--input", missingInput,
                "--output", Path.Combine(tempDir.FullName, "cloud.png")
            };

            var ok = ConsoleOptionsParser.TryParse(args, out var options, out var error);

            ok.Should().BeFalse();
            options.Should().BeNull();
            error.Should().Contain("Входной файл не найден:");
        }
        finally
        {
            TryDeleteDirectory(tempDir.FullName);
        }
    }

    [Test]
    public void TryParse_WhenUnknownFlagProvided_ShouldFail()
    {
        var tempDir = Directory.CreateTempSubdirectory("tags-cloud-tests-");
        try
        {
            var inputPath = CreateFile(tempDir.FullName, "words.txt", ["hello"]);

            var args = new[]
            {
                "--input", inputPath,
                "--unknown-flag", "1"
            };

            var ok = ConsoleOptionsParser.TryParse(args, out var options, out var error);

            ok.Should().BeFalse();
            options.Should().BeNull();
            error.Should().Be("Неизвестный флаг: --unknown-flag");
        }
        finally
        {
            TryDeleteDirectory(tempDir.FullName);
        }
    }

    [Test]
    public void TryParse_WhenFlagIsDuplicated_ShouldFail()
    {
        var tempDir = Directory.CreateTempSubdirectory("tags-cloud-tests-");
        try
        {
            var inputPath = CreateFile(tempDir.FullName, "words.txt", ["hello"]);

            var args = new[]
            {
                "--input", inputPath,
                "--width", "800",
                "--width", "900"
            };

            var ok = ConsoleOptionsParser.TryParse(args, out var options, out var error);

            ok.Should().BeFalse();
            options.Should().BeNull();
            error.Should().Be("Флаг указан дважды: --width");
        }
        finally
        {
            TryDeleteDirectory(tempDir.FullName);
        }
    }

    [Test]
    public void TryParse_WhenFlagValueIsMissing_ShouldFail()
    {
        var tempDir = Directory.CreateTempSubdirectory("tags-cloud-tests-");
        try
        {
            var inputPath = CreateFile(tempDir.FullName, "words.txt", ["hello"]);

            var args = new[]
            {
                "--input", inputPath,
                "--width"
            };

            var ok = ConsoleOptionsParser.TryParse(args, out var options, out var error);

            ok.Should().BeFalse();
            options.Should().BeNull();
            error.Should().Be("Ожидалось значение после --width");
        }
        finally
        {
            TryDeleteDirectory(tempDir.FullName);
        }
    }

    [Test]
    public void TryParse_WhenFlagValueIsAnotherFlag_ShouldFail()
    {
        var tempDir = Directory.CreateTempSubdirectory("tags-cloud-tests-");
        try
        {
            var inputPath = CreateFile(tempDir.FullName, "words.txt", ["hello"]);

            var args = new[]
            {
                "--input", inputPath,
                "--width", "--height",
                "--height", "600"
            };

            var ok = ConsoleOptionsParser.TryParse(args, out var options, out var error);

            ok.Should().BeFalse();
            options.Should().BeNull();
            error.Should().Be("Ожидалось значение после --width");
        }
        finally
        {
            TryDeleteDirectory(tempDir.FullName);
        }
    }

    [Test]
    public void TryParse_WhenMinFontIsGreaterThanMaxFont_ShouldFail()
    {
        var tempDir = Directory.CreateTempSubdirectory("tags-cloud-tests-");
        try
        {
            var inputPath = CreateFile(tempDir.FullName, "words.txt", ["hello"]);

            var args = new[]
            {
                "--input", inputPath,
                "--min-font", "50",
                "--max-font", "10"
            };

            var ok = ConsoleOptionsParser.TryParse(args, out var options, out var error);

            ok.Should().BeFalse();
            options.Should().BeNull();
            error.Should().Be("Некорректные значения шрифтов: min > max");
        }
        finally
        {
            TryDeleteDirectory(tempDir.FullName);
        }
    }

    [Test]
    public void TryParse_WhenBackgroundColorIsInvalid_ShouldFail()
    {
        var tempDir = Directory.CreateTempSubdirectory("tags-cloud-tests-");
        try
        {
            var inputPath = CreateFile(tempDir.FullName, "words.txt", ["hello"]);

            var args = new[]
            {
                "--input", inputPath,
                "--bg", "not-a-color"
            };

            var ok = ConsoleOptionsParser.TryParse(args, out var options, out var error);

            ok.Should().BeFalse();
            options.Should().BeNull();
            error.Should().Be("Некорректный --bg: not-a-color. Пример: --bg #ffffff");
        }
        finally
        {
            TryDeleteDirectory(tempDir.FullName);
        }
    }

    [TestCase("arial", false)]
    [TestCase("helvetica", false)]
    [TestCase("menlo", false)]
    [TestCase("arial", true)]
    public void TryParse_WithFontChoicesAndInvertFlag_ShouldParseOptions(string font, bool invert)
    {
        var tempDir = Directory.CreateTempSubdirectory("tags-cloud-tests-");
        try
        {
            var inputPath = CreateFile(tempDir.FullName, "words.txt", ["hello", "world"]);
            var outputPath = Path.Combine(tempDir.FullName, "cloud.png");

            var args = BuildArgs(
                inputPath: inputPath,
                outputPath: outputPath,
                font: font,
                invert: invert);

            var ok = ConsoleOptionsParser.TryParse(args, out var options, out var error);

            ok.Should().BeTrue();
            error.Should().BeEmpty();
            options.Should().NotBeNull();

            options.InputPath.Should().Be(Path.GetFullPath(inputPath));
            options.OutputPath.Should().Be(outputPath);

            options.Width.Should().Be(800);
            options.Height.Should().Be(800);
            options.CenterX.Should().Be(400);
            options.CenterY.Should().Be(400);

            options.MinFontSize.Should().Be(10f);
            options.MaxFontSize.Should().Be(60f);

            options.SourceType.Should().Be("txt");
            options.OutputFormat.Should().Be("png");

            options.BackgroundColor.Should().Be(Color.White);
            options.TextColor.Should().Be(Color.Black);

            options.Font.Should().Be(font);
            options.Desc.Should().Be(invert);
        }
        finally
        {
            TryDeleteDirectory(tempDir.FullName);
        }
    }

    [Test]
    public void TryParse_WhenOutputFormatIsUnsupported_ShouldFail()
    {
        var tempDir = Directory.CreateTempSubdirectory("tags-cloud-tests-");
        try
        {
            var inputPath = CreateFile(tempDir.FullName, "words.txt", ["hello", "world"]);

            var args = new[]
            {
                "--input", inputPath,
                "--format", "definitely-not-a-format"
            };

            var ok = ConsoleOptionsParser.TryParse(args, out var options, out var error);

            ok.Should().BeFalse();
            options.Should().BeNull();
            error.Should().Be("Неподдерживаемый формат: definitely-not-a-format");
        }
        finally
        {
            TryDeleteDirectory(tempDir.FullName);
        }
    }

    private static string[] BuildArgs(string inputPath, string outputPath, string font, bool invert)
    {
        var baseArgs = new List<string>
        {
            "--input", inputPath,
            "--output", outputPath,
            "--font", font
        };
        
        if (invert) baseArgs.Add("--desc");

        return baseArgs.ToArray();
    }

    private static string CreateFile(string dir, string fileName, IEnumerable<string> lines)
    {
        var path = Path.Combine(dir, fileName);
        File.WriteAllLines(path, lines);
        return path;
    }

    private static void TryDeleteDirectory(string path)
    {
        try { Directory.Delete(path, recursive: true); }
        catch
        {
            // ignored
        }
    }
}
