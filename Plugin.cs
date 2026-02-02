using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Bot;
using SPTarkov.Server.Core.Models.Eft.Profile;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Utils;
using System;
using System.Collections.Generic;
using System.Text;

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
        cpmc.ModifySkills();
        return new ValueTask<string>(output ?? string.Empty);
    }
}