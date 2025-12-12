using System;
using BepInEx;
using R2API;
using RoR2;

namespace ParrySfx
{

    [BepInDependency(SoundAPI.PluginGUID)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]

    public class ParrySfx : BaseUnityPlugin
    {
        public const String PluginGUID = PluginAuthor + "." + PluginName;
        public const String PluginAuthor = "Glyn";
        public const String PluginName = "ParrySFX";
        public const String PluginVersion = "1.0.0";

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
                Log.Info("[ParrySfx] Parry success, attempting to play sound on " + self.gameObject.ToString());
                Util.PlaySound("ThirdStrike", self.gameObject);
            }
            orig(self, damageInfo);
        }

        public void OnDisable() { 
            On.RoR2.HealthComponent.TakeDamageProcess -= ParrySuccess;
        }

    }
}
