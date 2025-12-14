using BepInEx;
using R2API;
using R2API.Utils;
using RoR2;

namespace ParrySfx
{

    [BepInDependency(SoundAPI.PluginGUID)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync)]

    public class ParrySfx : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Glyn";
        public const string PluginName = "ParrySFX";
        public const string PluginVersion = "1.0.0";

        public void Awake()
        {
            Log.Init(Logger);
        }

        public void OnEnable() {
            On.RoR2.HealthComponent.TakeDamageProcess += ParrySuccess;
        }

        private void ParrySuccess(On.RoR2.HealthComponent.orig_TakeDamageProcess orig, HealthComponent self, DamageInfo damageInfo)
        { 
            if (self.body.HasBuff(RoR2.DLC3Content.Buffs.Parrying))
            {
                Util.PlaySound("ThirdStrike", self.gameObject);
            }
            orig(self, damageInfo);
        }

        public void OnDisable() { 
            On.RoR2.HealthComponent.TakeDamageProcess -= ParrySuccess;
        }

    }
}
