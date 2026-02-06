using SPTarkov.Server.Core.Models.Enums;

namespace CertifiedPMC.config
{

    public class ConfigJson
    {
        public bool enabled { get; set; }
        public int minSkillLevel { get; set; }
        public int maxSkillLevel { get; set; }
        public int minMasteringLevel { get; set; }
        public int maxMasteringLevel { get; set; }
        public Dictionary<SkillTypes, bool> Skills { get; set; }
        public Mastering Mastering { get; set; }
    }

    public class Skills
    {
        public bool Endurance { get; set; }
        public bool Strength { get; set; }
        public bool Vitality { get; set; }
        public bool Health { get; set; }
        public bool StressResistance { get; set; }
        public bool Metabolism { get; set; }
        public bool Immunity { get; set; }
        public bool Perception { get; set; }
        public bool Intellect { get; set; }
        public bool Attention { get; set; }
        public bool Charisma { get; set; }
        public bool Pistol { get; set; }
        public bool Revolver { get; set; }
        public bool SMG { get; set; }
        public bool Assault { get; set; }
        public bool Shotgun { get; set; }
        public bool Sniper { get; set; }
        public bool LMG { get; set; }
        public bool HMG { get; set; }
        public bool Launcher { get; set; }
        public bool AttachedLauncher { get; set; }
        public bool Throwing { get; set; }
        public bool Melee { get; set; }
        public bool DMR { get; set; }
        public bool AimDrills { get; set; }
        public bool TroubleShooting { get; set; }
        public bool Surgery { get; set; }
        public bool CovertMovement { get; set; }
        public bool Search { get; set; }
        public bool MagDrills { get; set; }
        public bool LightVests { get; set; }
        public bool HeavyVests { get; set; }
        public bool WeaponTreatment { get; set; }
        public bool Crafting { get; set; }
        public bool HideoutManagement { get; set; }
    }

    public class Mastering
    {
        public bool SplitByFaction { get; set; }
        public string[] Bear { get; set; }
        public string[] USEC { get; set; }
    }

}
