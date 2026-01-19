using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HOI4Announcer;

    public class FactionsConfig
    {
        public class Faction
        {
            [JsonConverter(typeof(StringEnumConverter))]
            [JsonProperty("id")]
            public FactionID id { get; set; }

            [JsonProperty("nations")]
            public List<Nation> nations { get; set; }
        }

        public class Nation
        {
            [JsonConverter(typeof(StringEnumConverter))]
            [JsonProperty("id")]
            public NationID id { get; set; }
        }

        [JsonProperty("factions")]
        public List<Faction> factions { get; set; }
    }

public static class FactionsHandler
{


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

    public static FactionsConfig.Faction GetFaction(FactionID faction)
    {
        return config.factions.FirstOrDefault(f => f.id == faction);
    }

    public static void AddFaction(FactionID id)
    {
        // Make sure only one faction with the same name can exist
        Save();
    }

    public static void RemoveFaction(FactionID id)
    {
        Save();
    }

    public static void AddNation(FactionID faction, NationID nation)
    {
        // Make sure a nation can only be in one faction
        Save();
    }

    public static void RemoveNation(NationID nation)
    {
        Save();
    }
}

