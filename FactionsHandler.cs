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
            public List<Nation> nations { get; set; } = new List<Nation>();
        }

        public class Nation
        {
            [JsonConverter(typeof(StringEnumConverter))]
            [JsonProperty("id")]
            public NationID id { get; set; }
        }

        [JsonProperty("factions")]
        public List<Faction> factions { get; set; } = new List<Faction>();
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
        File.WriteAllText($"{Directory.GetCurrentDirectory()}/factions.json", JsonConvert.SerializeObject(config, Formatting.Indented));
    }

    public static FactionsConfig.Faction GetFaction(FactionID faction)
    {
        return config.factions.FirstOrDefault(f => f.id == faction) ?? new FactionsConfig.Faction() { id = faction };
    }

    public static void AddFaction(FactionID id)
    {
        if (config.factions.Any(f => f.id == id)) return;
        config.factions.Add(new FactionsConfig.Faction { id = id });
        Save();
    }

    public static void ClearFaction(FactionID id)
    {
        config.factions.RemoveAll(f => f.id == id);
        Save();
    }

    public static void AddNation(FactionID factionID, NationID nation)
    {
        // TODO: error when nation is already in a faction maybe?
        RemoveNation(nation);
        FactionsConfig.Faction faction = GetFaction(factionID);
        if (faction == null)
        {
            AddFaction(factionID);
            faction = GetFaction(factionID);
        }
        faction.nations.Add(new FactionsConfig.Nation { id = nation });
        Save();
    }

    public static void RemoveNation(NationID nation)
    {
        foreach (FactionsConfig.Faction faction in config.factions)
        {
            faction.nations.RemoveAll(n => n.id == nation);
        }
        Save();
    }
}

