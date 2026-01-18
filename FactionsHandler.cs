using System.Text.Json;

namespace HOI4Announcer;

public static class FactionsHandler
{
    public class FactionsConfig
    {
        public class Faction
        {
            public string name { get; set; }
            public List<Nation> nations { get; set; }
        }

        public class Nation
        {
            public string name { get; set; }
        }

        public List<Faction> factions { get; set; }
    }

    public static FactionsConfig config;

    private static readonly JsonSerializerOptions serializerOptions = new() { WriteIndented = true };

    public static void Load()
    {
        config = JsonSerializer.Deserialize<FactionsConfig>(File.ReadAllText($"{Directory.GetCurrentDirectory()}/factions.json"));
    }

    public static void Save()
    {
        File.WriteAllText($"{Directory.GetCurrentDirectory()}/factions.json", JsonSerializer.Serialize(config, serializerOptions));
    }
}

