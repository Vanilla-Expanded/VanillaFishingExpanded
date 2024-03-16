using RimWorld;
using UnityEngine;
using Verse;

namespace VCE_Fishing.Options
{
    public class VCE_Fishing_Settings : ModSettings

    {

        public const int minimumZoneSizeBase = 25;
        public const int smallFishDurationFactorBase = 4500;
        public const int chanceForSpecialsBase = 1;
        public const float minMapTempForLowFishBase = 0f;
        public const float maxMapTempForLowFishBase = 50.0f;





        public static int VCEF_minimumZoneSize = minimumZoneSizeBase;
        public static int VCEF_smallFishDurationFactor = smallFishDurationFactorBase;
        public static int VCEF_chanceForSpecials = chanceForSpecialsBase;
        public static float VCEF_minMapTempForLowFish = minMapTempForLowFishBase;
        public static float VCEF_maxMapTempForLowFish = maxMapTempForLowFishBase;
        public static bool VCEF_dropFish = false;







        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref VCEF_minimumZoneSize, "VCEF_minimumZoneSize", minimumZoneSizeBase, true);
            Scribe_Values.Look(ref VCEF_smallFishDurationFactor, "VCEF_smallFishDurationFactor", smallFishDurationFactorBase, true);
            Scribe_Values.Look(ref VCEF_chanceForSpecials, "VCEF_chanceForSpecials", chanceForSpecialsBase, true);
            Scribe_Values.Look(ref VCEF_minMapTempForLowFish, "VCEF_minMapTempForLowFish", minMapTempForLowFishBase, true);
            Scribe_Values.Look(ref VCEF_maxMapTempForLowFish, "VCEF_maxMapTempForLowFish", maxMapTempForLowFishBase, true);
            Scribe_Values.Look(ref VCEF_dropFish, "VCEF_dropFish", false, true);






        }

        public static void DoWindowContents(Rect inRect)
        {
            Listing_Standard ls = new Listing_Standard();


            ls.Begin(inRect);
            ls.Gap(12f);
            ls.Label("VCEF_minimumZoneSize".Translate()+": "+ VCEF_minimumZoneSize, -1, "VCEF_minimumZoneSizeTooltip".Translate());
            VCEF_minimumZoneSize = (int)ls.Slider(VCEF_minimumZoneSize, 1,100);
            ls.Gap(12f);

            ls.Label("VCEF_smallFishDurationFactor".Translate() + ": " + VCEF_smallFishDurationFactor, -1, "VCEF_smallFishDurationFactorTooltip".Translate());
            VCEF_smallFishDurationFactor = (int)ls.Slider(VCEF_smallFishDurationFactor, 1000, 10000);
            ls.Gap(12f);

            ls.Label("VCEF_chanceForSpecials".Translate() + ": " + VCEF_chanceForSpecials+"%", -1, "VCEF_chanceForSpecialsTooltip".Translate());
            VCEF_chanceForSpecials = (int)ls.Slider(VCEF_chanceForSpecials, 0, 100);
            ls.Gap(12f);

            ls.Label("VCEF_minMapTempForLowFish".Translate() + ": " + (int)VCEF_minMapTempForLowFish + "ºC", -1, "VCEF_minMapTempForLowFishTooltip".Translate());
            VCEF_minMapTempForLowFish = ls.Slider(VCEF_minMapTempForLowFish, -50, 50);
            ls.Gap(12f);

            ls.Label("VCEF_maxMapTempForLowFish".Translate() + ": " + (int)VCEF_maxMapTempForLowFish + "ºC", -1, "VCEF_maxMapTempForLowFishTooltip".Translate());
            VCEF_maxMapTempForLowFish = ls.Slider(VCEF_maxMapTempForLowFish, 0, 100);
            ls.Gap(12f);


            ls.CheckboxLabeled("VCEF_dropFish".Translate(), ref VCEF_dropFish, null);

            ls.End();
        }



    }










}
