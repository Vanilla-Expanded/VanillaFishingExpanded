using RimWorld;
using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace VCE_Fishing.Options
{
    public class VCE_Fishing_Settings : ModSettings

    {

        private static Vector2 scrollPosition = Vector2.zero;

        public bool VCEF_DisableGillRot = VCEF_DisableGillRotBase;
        public static bool VCEF_DisableGillRotBase = false;

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

        public const float chanceForNegativeOutcomeBase = 0.02f;
        public static float VCEF_chanceForNegativeOutcome = chanceForNegativeOutcomeBase;

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref VCEF_fishingYieldMultiplier, "VCEF_fishingYieldMultiplier", fishingYieldBase, true);
            Scribe_Values.Look(ref VCEF_fishingSpeedMultiplier, "VCEF_fishingSpeedMultiplier", fishingSpeedBase, true);
            Scribe_Values.Look(ref VCEF_maxFishPopulationMultiplier, "VCEF_maxFishPopulationMultiplier", maxFishPopulationMultiplierBase, true);
            Scribe_Values.Look(ref VCEF_fishAmountMultiplier, "VCEF_fishAmountMultiplier", fishAmountMultiplierBase, true);
            Scribe_Values.Look(ref VCEF_chanceForSpecials, "VCEF_chanceForSpecials", chanceForSpecialsBase, true);
            Scribe_Values.Look(ref VCEF_minDaysBetweenRareCatches, "VCEF_minDaysBetweenRareCatches", minDaysBetweenRareCatchesBase, true);
            Scribe_Values.Look(ref VCEF_chanceForNegativeOutcome, "VCEF_chanceForNegativeOutcome", chanceForNegativeOutcomeBase, true);
            Scribe_Values.Look(ref VCEF_DisableGillRot, "VCEF_DisableGillRot", VCEF_DisableGillRotBase, true);

        }

        public void DoWindowContents(Rect inRect)
        {
            Listing_Standard ls = new Listing_Standard();

            var scrollContainer = inRect.ContractedBy(10);
            scrollContainer.height -= ls.CurHeight;
            scrollContainer.y += ls.CurHeight;
            var frameRect = inRect.ContractedBy(5);
            frameRect.y += 15;
            frameRect.height -= 15;
            var contentRect = frameRect;
            contentRect.x = 0;
            contentRect.y = 0;
            contentRect.width -= 20;

            Widgets.BeginScrollView(frameRect, ref scrollPosition, contentRect, true);
            ls.Begin(contentRect.AtZero());

            ls.CheckboxLabeled("VCEF_DisableGillRot".Translate(), ref VCEF_DisableGillRot, "VCEF_DisableGillRotDesc".Translate());
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

            ls.Label(new TaggedString("VCEF_chanceForSpecials".Translate() + ": " + VCEF_chanceForSpecials + "%"), -1, "VCEF_chanceForSpecialsTooltip_Odyssey".Translate());
            VCEF_chanceForSpecials = (int)ls.Slider(VCEF_chanceForSpecials, 0, 100);
            ls.Gap(12f);

            ls.Label(new TaggedString("VCEF_minDaysBetweenRareCatches".Translate() + ": " + VCEF_minDaysBetweenRareCatches + " days"), -1, "VCEF_minDaysBetweenRareCatchesTooltip".Translate());
            VCEF_minDaysBetweenRareCatches = (float)Math.Round(ls.Slider(VCEF_minDaysBetweenRareCatches, 0.01f, 10), 2);
            ls.Gap(12f);

            var LastLabel = ls.Label(new TaggedString("VCEF_chanceForNegativeOutcome".Translate() + ": " + VCEF_chanceForNegativeOutcome.ToStringPercent()), -1, "VCEF_chanceForNegativeOutcomeTooltip".Translate());
            VCEF_chanceForNegativeOutcome = (float)Math.Round(ls.Slider(VCEF_chanceForNegativeOutcome, 0, 1), 2);
            ls.Gap(12f);

            if (ls.Settings_Button("VCEF_Reset_Plain".Translate(), new Rect(0, LastLabel.y+ 50, 250, 24)))
            {
                VCEF_fishingYieldMultiplier = fishingYieldBase;

                VCEF_fishingSpeedMultiplier = fishingSpeedBase;

                VCEF_maxFishPopulationMultiplier = maxFishPopulationMultiplierBase;

                VCEF_fishAmountMultiplier = fishAmountMultiplierBase;

                VCEF_chanceForSpecials = chanceForSpecialsBase;

                VCEF_minDaysBetweenRareCatches = minDaysBetweenRareCatchesBase;

                VCEF_chanceForNegativeOutcome = chanceForNegativeOutcomeBase;

                VCEF_DisableGillRot = VCEF_DisableGillRotBase;
            }


            ls.End();
            Widgets.EndScrollView();
            Write();
        }



    }










}
