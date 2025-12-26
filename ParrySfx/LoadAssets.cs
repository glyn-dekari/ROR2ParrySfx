using BepInEx;
using System.IO;
using UnityEngine;

namespace ParrySfx
{
    internal static class LoadAssets
    {
        public static AssetBundle Bundle;
        public static string BundleName = "parryAssets";
        public static PluginInfo Info;

        internal static void Init(PluginInfo info)
        {
            Info = info;

            if (Directory.Exists(CurrentDir))
            {
                Bundle = AssetBundle.LoadFromFile(BundlePath);
            }
        }

        internal static string BundlePath
        {
            get
            {
                return System.IO.Path.Combine(CurrentDir, BundleName);
            }
        }

        internal static string CurrentDir
        {
            get
            {
                return System.IO.Path.GetDirectoryName(Info.Location);
            }
        }
    }
}
