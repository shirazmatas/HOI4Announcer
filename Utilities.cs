
using System.Reflection;


namespace hoi4announcer;

public static class Utilities
{

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