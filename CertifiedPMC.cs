using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Eft.Profile;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Utils;
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
        private readonly ModHelper _modHelper;
        private readonly RandomUtil _randomUtil;
        private readonly ISptLogger<CertifiedPMC> _logger;

        private const string LogPrefix = "[CertifiedPMC] ";

        private int skillMinValue = 0;
        private int skillMaxValue = 5100;
        private int masteMinValue = 0;
        private int masteMaxValue = 1000;

        public CertifiedPMC(SaveServer saveServer, ModHelper modHelper, RandomUtil randomUtil, ISptLogger<CertifiedPMC> logger) 
        {
            _saveServer = saveServer;
            _modHelper = modHelper;
            _randomUtil = randomUtil;
            _logger = logger;
        }


        public void Modify(MongoId sessionId)
        {
            SptProfile? profile = _saveServer.GetProfile(sessionId);
            ModifySkills(profile);
            ModifyWeaponMasteries(profile);
        }

        private void ModifySkills(SptProfile profile)
        {
            IEnumerable<CommonSkill> commonSkills = profile.CharacterData.PmcData.Skills.Common;
            foreach (var skill in commonSkills)
            {
                skill.Progress = _randomUtil.GetInt(skillMinValue, skillMaxValue);
                _logger.Info($"{LogPrefix}According to documentation your {skill.Id} is at {Math.Floor(skill.Progress / 100)} level.");
            }
        }

        private void ModifyWeaponMasteries(SptProfile profile)
        {
            var weaponMasteries = profile.CharacterData.PmcData.Skills?.Mastering;
            _logger.Info($"{LogPrefix}Modifying weapon masteries... {weaponMasteries?.Count()}");
            foreach (var skill in weaponMasteries)
            {                
                _logger.Info($"{LogPrefix}According to documentation your {skill.Id} is {skill.Progress}.");
            }
        }
    }
}
