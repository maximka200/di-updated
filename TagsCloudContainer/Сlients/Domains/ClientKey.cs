using TagsCloudContainer.Сlients.Exceptions;

namespace TagsCloudContainer.Сlients.Domains;

public static class ClientKey
{
    public static string Parse(string raw)
    {
        var trimmed = string.Concat(raw).Trim();

        try
        {
            ArgumentException.ThrowIfNullOrEmpty(trimmed);
            return trimmed;
        }
        catch (ArgumentException)
        {
            throw new CommandLineException("Пустое значение флага");
        }
    }
}