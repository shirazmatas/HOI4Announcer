
using System.Reflection;
using System.Text.Json;
using DSharpPlus;


namespace HOI4Announcer;

public static class Utilities
{
    public static readonly JsonSerializerOptions jsonSerializerOptions = new() { WriteIndented = true };

    public static string DiscordRelativeTime(DateTimeOffset time)
    {
        return Formatter.Timestamp(time, TimestampFormat.ShortDateTime); // TOOD: Configurable timestamp format?
    }

    public static string ReadManifestData(string embeddedFileName)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        string resourceName = assembly.GetManifestResourceNames().First(s => s.EndsWith(embeddedFileName,StringComparison.CurrentCultureIgnoreCase));

        using Stream stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            throw new InvalidOperationException("Could not load manifest resource stream.");
        }

        using StreamReader reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}