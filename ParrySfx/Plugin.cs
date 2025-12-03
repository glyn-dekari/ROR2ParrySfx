using BepInEx;
using BepInEx.Configuration;
using R2API;
using R2API.Utils;
using RoR2;
using RiskOfOptions;
using RiskOfOptions.Options;

namespace ParrySfx
{

    [BepInDependency(SoundAPI.PluginGUID)]
    [BepInDependency("com.rune500.riskofoptions", BepInDependency.DependencyFlags.SoftDependency)]

    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync)]

    public class ParrySfx : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Glyn";
        public const string PluginName = "ParrySFX";
        public const string PluginVersion = "1.0.0";

        public static ConfigEntry<string> ParrySoundType { get; set; }
        public static ConfigEntry<float> Volume {  get; set; }

        public void Awake()
        {
            Log.Init(Logger);

            ParrySoundType = Config.Bind("Parry SFX", "ParrySoundIndex", "3S", "Controls which sound effect to play on a successful parry.");
            Volume = Config.Bind("Parry SFX", "Volume", 0.5f, "Controls the volume of the sound effect.");

            if(BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions"))
            {
                //TODO: change this to use the enum
                ModSettingsManager.AddOption(new StringInputFieldOption(ParrySoundType));
                ModSettingsManager.AddOption(new SliderOption(Volume));
            }
        }

        public void OnEnable() {
            On.RoR2.HealthComponent.TakeDamageProcess += ParrySuccess;
        }

        private void ParrySuccess(On.RoR2.HealthComponent.orig_TakeDamageProcess orig, HealthComponent self, DamageInfo damageInfo)
        {
            string sfxType = ChooseParrySfx(ParrySoundType.Value);
            if (self.body.HasBuff(RoR2.DLC3Content.Buffs.Parrying))
            {
                Log.Info("[ParrySfx] Parry success, attempting to play sound on " + self.gameObject.ToString());
                Util.PlaySound(sfxType, self.gameObject);
            }
            orig(self, damageInfo);
        }

        private string ChooseParrySfx(string sfxType)
        {
            //TODO: change this to use the enum
            switch (sfxType) {
                case "ThirdStrike":
                    return "ParrySuccess";
                case "MeltyBlood":
                    return "ParryShield";
                default:
                    return "ParrySuccess";
            }
        }

        public void OnDisable() { 
            On.RoR2.HealthComponent.TakeDamageProcess -= ParrySuccess;
        }

    }
}
