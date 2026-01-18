using Newtonsoft.Json;

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

    public static FactionsConfig config { get; private set; } = null;

    public static void Load()
    {
        Logger.Log("Loading factions config \"" + Directory.GetCurrentDirectory() + "/factions.json\"");
        config = JsonConvert.DeserializeObject<FactionsConfig>(File.ReadAllText($"{Directory.GetCurrentDirectory()}/factions.json"));
    }

    public static void Save()
    {
        File.WriteAllText($"{Directory.GetCurrentDirectory()}/factions.json", JsonConvert.SerializeObject(config));
    }

    public static void AddFaction(string name)
    {
        // Make sure only one faction with the same name can exist
    }

    public static void RemoveFaction(string name)
    {

    }

    public static void AddNation(string faction, string nation)
    {
        // Make sure a nation can only be in one faction
    }

    public static void RemoveNation(string faction)
    {

    }
}

