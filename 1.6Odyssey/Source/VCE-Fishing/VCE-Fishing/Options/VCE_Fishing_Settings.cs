using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace VCE_Fishing.Options
{
    public class VCE_Fishing_Settings : ModSettings

    {
        public const int fishingYieldBase = 1;
        public static float VCEF_fishingYieldMultiplier = fishingYieldBase;

        public const int fishingSpeedBase = 1;
        public static float VCEF_fishingSpeedMultiplier = fishingSpeedBase;

        public const int maxFishPopulationMultiplierBase = 1;
        public static float VCEF_maxFishPopulationMultiplier = maxFishPopulationMultiplierBase;

        public const int fishAmountMultiplierBase = 1;
        public static float VCEF_fishAmountMultiplier = fishAmountMultiplierBase;

        public const int chanceForSpecialsBase = 1;
        public static int VCEF_chanceForSpecials = chanceForSpecialsBase;

        public const int minDaysBetweenRareCatchesBase = 5;
        public static float VCEF_minDaysBetweenRareCatches = minDaysBetweenRareCatchesBase;

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref VCEF_fishingYieldMultiplier, "VCEF_fishingYieldMultiplier", fishingYieldBase, true);
            Scribe_Values.Look(ref VCEF_fishingSpeedMultiplier, "VCEF_fishingSpeedMultiplier", fishingSpeedBase, true);
            Scribe_Values.Look(ref VCEF_maxFishPopulationMultiplier, "VCEF_maxFishPopulationMultiplier", maxFishPopulationMultiplierBase, true);
            Scribe_Values.Look(ref VCEF_fishAmountMultiplier, "VCEF_fishAmountMultiplier", fishAmountMultiplierBase, true);
            Scribe_Values.Look(ref VCEF_chanceForSpecials, "VCEF_chanceForSpecials", chanceForSpecialsBase, true);
            Scribe_Values.Look(ref VCEF_minDaysBetweenRareCatches, "VCEF_minDaysBetweenRareCatches", minDaysBetweenRareCatchesBase, true);
        }

        public static void DoWindowContents(Rect inRect)
        {
            Listing_Standard ls = new Listing_Standard();


            ls.Begin(inRect);
            ls.Gap(12f);

            ls.Label(new TaggedString("VCEF_fishingYieldMultiplier".Translate() + ": x" + VCEF_fishingYieldMultiplier), -1, "VCEF_fishingYieldMultiplierTooltip".Translate());
            VCEF_fishingYieldMultiplier = (float)Math.Round(ls.Slider(VCEF_fishingYieldMultiplier, 0.1f, 5), 1);
            ls.Gap(12f);

            ls.Label(new TaggedString("VCEF_fishingSpeedMultiplier".Translate() + ": x" + VCEF_fishingSpeedMultiplier), -1, "VCEF_fishingSpeedMultiplierTooltip".Translate());
            VCEF_fishingSpeedMultiplier = (float)Math.Round(ls.Slider(VCEF_fishingSpeedMultiplier, 0.1f, 5), 1);
            ls.Gap(12f);

            ls.Label(new TaggedString("VCEF_maxFishPopulationMultiplier".Translate() + ": x" + VCEF_maxFishPopulationMultiplier), -1, "VCEF_maxFishPopulationMultiplierTooltip".Translate());
            VCEF_maxFishPopulationMultiplier = (float)Math.Round(ls.Slider(VCEF_maxFishPopulationMultiplier, 0.1f, 10), 1);
            ls.Gap(12f);

            ls.Label(new TaggedString("VCEF_fishAmountMultiplier".Translate() + ": x" + VCEF_fishAmountMultiplier), -1, "VCEF_fishAmountMultiplierTooltip".Translate());
            VCEF_fishAmountMultiplier = (float)Math.Round(ls.Slider(VCEF_fishAmountMultiplier, 0.1f, 5), 1);
            ls.Gap(12f);

            ls.Label(new TaggedString("VCEF_chanceForSpecials".Translate() + ": " + VCEF_chanceForSpecials+"%"), -1, "VCEF_chanceForSpecialsTooltip_Odyssey".Translate());
            VCEF_chanceForSpecials = (int)ls.Slider(VCEF_chanceForSpecials, 0, 100);
            ls.Gap(12f);

            ls.Label(new TaggedString("VCEF_minDaysBetweenRareCatches".Translate() + ": " + VCEF_minDaysBetweenRareCatches + " days"), -1, "VCEF_minDaysBetweenRareCatchesTootip".Translate());
            VCEF_minDaysBetweenRareCatches = (float)Math.Round(ls.Slider(VCEF_minDaysBetweenRareCatches, 0.01f, 10),2);
            ls.Gap(12f);



            ls.End();
        }



    }










}
