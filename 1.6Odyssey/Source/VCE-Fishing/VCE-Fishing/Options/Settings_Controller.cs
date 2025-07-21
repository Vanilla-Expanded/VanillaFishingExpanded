using RimWorld;
using UnityEngine;
using Verse;
using VCE_Fishing.Options;

namespace VCE_Fishing
{
    class VCE_Fishing_Mod : Mod
    {
        public static VCE_Fishing_Settings settings;

        public VCE_Fishing_Mod(ModContentPack content) : base(content)
        {
            settings = GetSettings<VCE_Fishing_Settings>();
        }
        public override string SettingsCategory() => "VE Fishing - Odyssey";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            settings.DoWindowContents(inRect);
        }
    }
}



