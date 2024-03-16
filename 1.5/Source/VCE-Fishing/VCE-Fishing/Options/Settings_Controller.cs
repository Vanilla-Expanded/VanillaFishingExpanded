using RimWorld;
using UnityEngine;
using Verse;

namespace VCE_Fishing.Options
{
    class VCE_Fishing_Mod : Mod
    {

        public VCE_Fishing_Mod(ModContentPack content) : base(content)
        {
            GetSettings<VCE_Fishing_Settings>();
        }
        public override string SettingsCategory() => "Vanilla Fishing Expanded";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            VCE_Fishing_Settings.DoWindowContents(inRect);
        }
    }
}



