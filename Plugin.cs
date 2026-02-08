using CertifiedPMC.config;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Bot;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Eft.Profile;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CertifiedPMC;
public record ModMetadata : AbstractModMetadata
{
    public override string ModGuid { get; init; } = "com.fryciarz7.spt.certpmc";
    public override string Name { get; init; } = "Certified PMC";
    public override string Author { get; init; } = "fryciarz7";
    public override List<string>? Contributors { get; init; }
    public override SemanticVersioning.Version Version { get; init; } = new("1.0.0");
    public override SemanticVersioning.Range SptVersion { get; init; } = new("~4.0.11");

    public override List<string>? Incompatibilities { get; init; }
    public override Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
    public override string? Url { get; init; }
    public override bool? IsBundleMod { get; init; }
    public override string? License { get; init; } = "Creative Commons BY-NC-SA 3.0";
}

#if DEBUG
[Injectable(TypePriority = OnLoadOrder.PostSptModLoader)] // Can also give an int value for fine-grained control
public class OnLoadExample(SaveServer saveServer,
    ModHelper modHelper,
    ISptLogger<OnLoadExample> logger) : IOnLoad // Must implement the IOnLoad interface
{
    private const string LogPrefix = "[CertifiedPMC] ";
    public Task OnLoad()
    {
        // Can do work here

        string? pathToMod = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
        ConfigJson config = modHelper.GetJsonDataFromFile<ConfigJson>(pathToMod, "config/config.json");
        // logger.Info($"{LogPrefix}Config loaded: {JsonSerializer.Serialize(config)}");

        foreach (var (key, value) in config.Skills)
        {
            logger.Info($"{LogPrefix}Skill: {key} is {(value ? "enabled" : "disabled")}.");
        }

        return Task.CompletedTask;

        var profiles = saveServer.GetProfiles();
        logger.Info($"{LogPrefix}Found {profiles.Count} profiles in the database.");
        foreach (var kvp in profiles)
        {
            SptProfile? profile = kvp.Value;
            IEnumerable<MasterySkill>? weaponMasteries = profile.CharacterData.PmcData.Skills?.Mastering;
            logger.Info($"{LogPrefix}Reading weapon masteries... {weaponMasteries?.Count()}");
            string json = JsonSerializer.Serialize(weaponMasteries,
                        new JsonSerializerOptions
                        {
                            WriteIndented = true
                        }); // string.Empty;
            logger.Info(json);

            foreach (var skill in weaponMasteries)
            {
                //json += JsonSerializer.Serialize(skill);
                
                logger.Info($"{LogPrefix}According to documentation {profile?.ProfileInfo?.Username}'s {skill.Id} is {skill.Progress}.");
                foreach (var extData in skill.ExtensionData)
                {
                    logger.Info($"{LogPrefix}Extension data: {extData.Key} : {extData.Value}");
                }
            }
            //File.WriteAllText("ProfileCreateRequestData.json", json);
        }
        logger.Success($"Mod loaded after database!");

        return Task.CompletedTask;
    }
}
#endif
[Injectable]
public class CertifiedPMCRoute(JsonUtil jsonUtil, CertifiedPMCPlugin callbacks) : StaticRouter(
    jsonUtil, [
        new RouteAction<ProfileCreateRequestData>(
            "/client/game/profile/create",
            async (
                url,
                info,
                sessionId,
                output
                ) => await callbacks.ModifySkillsAsync(url, info, sessionId, output)
            )
        ]
    )
{ }

[Injectable]
public class CertifiedPMCPlugin(ISptLogger<CertifiedPMCPlugin> logger, ModHelper modHelper, HttpResponseUtil httpResponseUtil, CertifiedPMC cpmc)
{
    public ValueTask<string> ModifySkillsAsync(string url, ProfileCreateRequestData info, MongoId sessionId, string? output)
    {
        cpmc.Modify(sessionId);
        return new ValueTask<string>(output ?? string.Empty);
    }
}