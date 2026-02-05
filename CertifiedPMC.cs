using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Eft.Profile;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CertifiedPMC
{
    [Injectable]
    public class CertifiedPMC
    {
        private readonly SaveServer _saveServer;
        private readonly ISptLogger<CertifiedPMC> _logger;

        private int skillMinValue = 0;
        private int skillMaxValue = 5100;

        public CertifiedPMC(SaveServer saveServer, ISptLogger<CertifiedPMC> logger) 
        {
            _saveServer = saveServer;
            _logger = logger;
        }


        public void Modify(MongoId sessionId)
        {
            SptProfile? profile = _saveServer.GetProfile(sessionId);
            ModifySkills(profile);
        }

        private void ModifySkills(SptProfile profile)
        {
            IEnumerable<CommonSkill> commonSkills = profile.CharacterData.PmcData.Skills.Common;
            foreach (var skill in commonSkills)
            {
                Random rand = new Random();
                skill.Progress = rand.Next(skillMinValue, skillMaxValue);
                _logger.Info($"According to documentation your {skill.Id} is {skill.Progress}.");
            }
        }
    }
}
