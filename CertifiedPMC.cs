using CertifiedPMC.config;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Helpers;
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
using System.Reflection;
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

        private readonly ConfigJson _config;
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

            string? pathToMod = _modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
            _config = _modHelper.GetJsonDataFromFile<ConfigJson>(pathToMod, "config/config.json");

            skillMinValue = _config.minSkillLevel;
            skillMaxValue = _config.maxSkillLevel;
            masteMinValue = _config.minMasteringLevel;
            masteMaxValue = _config.maxMasteringLevel;
        }


        public void Modify(MongoId sessionId)
        {
            if (_config.enabled)
            {
                SptProfile? profile = _saveServer.GetProfile(sessionId);
                ModifySkills(profile);
                ModifyWeaponMasteries(profile);
            }
        }

        private void ModifySkills(SptProfile profile)
        {
            IEnumerable<CommonSkill> commonSkills = profile.CharacterData.PmcData.Skills.Common;
            foreach (var skill in commonSkills)
            {
                _config.Skills.TryGetValue(skill.Id, out bool enabled);
                if (enabled)
                {
                    skill.Progress = _randomUtil.GetInt(skillMinValue, skillMaxValue);
                    _logger.Info($"{LogPrefix}According to documentation your {skill.Id} is at {Math.Floor(skill.Progress / 100)} level.");
                }
            }
        }

        private void ModifyWeaponMasteries(SptProfile profile)
        {
            profile.CharacterData.PmcData.Skills.Mastering = new List<MasterySkill>();

            List<MasterySkill> listOfSkills = new List<MasterySkill>();
            string[] weaponList = [];
            if (_config.Mastering.SplitByFaction)
            {
                _logger.Info($"{LogPrefix}Modifying weapon masteries for {profile.CharacterData.PmcData.Info.Side} faction.");
                weaponList = profile.CharacterData.PmcData.Info.Side == "Bear" ? _config.Mastering.Bear : _config.Mastering.USEC;
            }
            else            {
                _logger.Info($"{LogPrefix}Modifying weapon masteries for both factions.");
                weaponList = _config.Mastering.Bear.Union(_config.Mastering.USEC).Distinct().ToArray();
            }
            foreach (string weapon in weaponList)
                {
                    listOfSkills.Add(CreateMasterySkill(weapon));
                }

            if (profile.CharacterData.PmcData.Skills != null)
            {
                profile.CharacterData.PmcData.Skills.Mastering = listOfSkills;
            }

            foreach (MasterySkill? skill in profile.CharacterData.PmcData.Skills?.Mastering)
            {                
                _logger.Info($"{LogPrefix}According to documentation your {skill.Id} is {skill.Progress}.");
            }
        }

        private MasterySkill CreateMasterySkill(string id)
        {
            return new MasterySkill
            {
                Id = id,
                Progress = _randomUtil.GetInt(masteMinValue, masteMaxValue)
            };
        }
    }
}
