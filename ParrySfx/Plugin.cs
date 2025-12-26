using BepInEx;
using BepInEx.Configuration;
using R2API;
using R2API.Utils;
using RoR2;
using RiskOfOptions;
using RiskOfOptions.Options;
using UnityEngine;
using System.Runtime.CompilerServices;

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
        public const string PluginVersion = "1.1.0";

        public static ConfigEntry<SfxEnum> ParrySoundType { get; set; }
        //public static ConfigEntry<float> Volume {  get; set; }

        public void Awake()
        {
            Log.Init(Logger);

            LoadAssets.Init(Info);

            ParrySoundType = Config.Bind("Parry SFX", "ParrySoundType", SfxEnum.ThirdStrike, "Controls which sound effect to play on a successful parry.");
            //Volume = Config.Bind("Parry SFX", "Volume", 0.5f, "[unimplemented] Controls the volume of the sound effect.");

            if(BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions"))
            {
                ModSettingsManager.SetModIcon(LoadAssets.Bundle.LoadAsset<Sprite>("assets/modassets/thumbnail.png"));
                ModSettingsManager.SetModDescription("Adds various parry sound effects.");

                ModSettingsManager.AddOption(new ChoiceOption(ParrySoundType));
                //ModSettingsManager.AddOption(new SliderOption(Volume));
            }
        }

        public void OnEnable() {
            On.RoR2.HealthComponent.TakeDamageProcess += ParrySuccess;
        }

        private void ParrySuccess(On.RoR2.HealthComponent.orig_TakeDamageProcess orig, HealthComponent self, DamageInfo damageInfo)
        {
            if (self.body.HasBuff(RoR2.DLC3Content.Buffs.Parrying))
            {
                Log.Info("[ParrySfx] Parry success, attempting to play sound " + ParrySoundType.Value.ToString() + " on " + self.gameObject.ToString());
                Util.PlaySound(ParrySoundType.Value.ToString(), self.gameObject);
            }
            orig(self, damageInfo);
        }

        public void OnDisable() { 
            On.RoR2.HealthComponent.TakeDamageProcess -= ParrySuccess;
        }

    }
}
