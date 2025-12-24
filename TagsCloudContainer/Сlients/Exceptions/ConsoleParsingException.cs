namespace TagsCloudContainer.Сlients.Exceptions;

public sealed class ConsoleParsingException : Exception
{
    public ConsoleParsingException(string message) : base(message) { }
    public ConsoleParsingException(string message, Exception inner) : base(message, inner) { }
}