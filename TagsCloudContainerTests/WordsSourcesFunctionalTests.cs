using System.Text;
using FluentAssertions;
using TagsCloudContainer.Core;
using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Core.WordSources;

namespace TagsCloudContainerTests;


[SetUpFixture]
public sealed class EncodingSetup
{
    [OneTimeSetUp]
    public void RegisterEncodings()
        => Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
}


[TestFixture]
public class WordsSourcesFunctionalTests
{
    [Test]
    public void DocxWordsSource_GetWords_ShouldTokenizeTextFromDocxFixture()
    {
        var path = TestDataPath("words.docx");
        File.Exists(path).Should().BeTrue();

        IWordsSource source = new DocxWordsSource();

        var words = source.GetWords(path).ToArray();

        words.Should().Contain(["Hello", "world", "hello", "cloud", "2025"]);
    }

    [Test]
    public void DocWordsSource_GetWords_ShouldTokenizeTextFromDocFixture()
    {
        var path = TestDataPath("words.doc");
        File.Exists(path).Should().BeTrue("файл words.doc должен быть в TestData");

        IWordsSource source = new DocWordsSource();

        var words = source.GetWords(path).ToArray();

        words.Should().Contain(["Hello", "world", "hello", "cloud", "2025"]);
    }

    // [Test]
    // public void WordsSourceFactory_SourceFormats_ShouldContainTxtDocDocx()
    // {
    //     WordsSourceFactory.WordSourceFormats.Should().Contain(new[] { "txt", "doc", "docx" });
    // }
    //
    // [TestCase("txt", typeof(TxtWordsSource))]
    // [TestCase("doc", typeof(DocWordsSource))]
    // [TestCase("docx", typeof(DocxWordsSource))]
    // [TestCase("DOCX", typeof(DocxWordsSource))]
    // public void WordsSourceFactory_Create_ShouldReturnExpectedImplementation(string format, Type expectedType)
    // {
    //     var source = WordsSourceFactory.Create(new SourceSettings("whatever", format));
    //     source.Should().BeOfType(expectedType);
    // }
    //
    // [Test]
    // public void WordsSourceFactory_Create_WhenFormatIsUnsupported_ShouldThrow()
    // {
    //     var act = () => WordsSourceFactory.Create(new SourceSettings("whatever", "pdf"));
    //
    //     act.Should()
    //         .Throw<NotSupportedException>()
    //         .WithMessage("*'pdf'*");
    // }

    private static string TestDataPath(string fileName) =>
        Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", fileName);
}
