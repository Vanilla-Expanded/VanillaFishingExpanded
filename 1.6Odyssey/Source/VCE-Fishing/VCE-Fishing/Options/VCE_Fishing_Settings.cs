using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace VCE_Fishing.Options
{
    public class VCE_Fishing_Settings : ModSettings

    {

      
        public const int chanceForSpecialsBase = 1;
        public static int VCEF_chanceForSpecials = chanceForSpecialsBase;

        public const int minDaysBetweenRareCatchesBase = 5;
        public static float VCEF_minDaysBetweenRareCatches = minDaysBetweenRareCatchesBase;

        public override void ExposeData()
        {
            base.ExposeData();
           
            Scribe_Values.Look(ref VCEF_chanceForSpecials, "VCEF_chanceForSpecials", chanceForSpecialsBase, true);
            Scribe_Values.Look(ref VCEF_minDaysBetweenRareCatches, "VCEF_minDaysBetweenRareCatches", minDaysBetweenRareCatchesBase, true);


        }

        public static void DoWindowContents(Rect inRect)
        {
            Listing_Standard ls = new Listing_Standard();


            ls.Begin(inRect);
            ls.Gap(12f);
           

            ls.Label(new TaggedString("VCEF_chanceForSpecials".Translate() + ": " + VCEF_chanceForSpecials+"%"), -1, "VCEF_chanceForSpecialsTooltip_Odyssey".Translate());
            VCEF_chanceForSpecials = (int)ls.Slider(VCEF_chanceForSpecials, 0, 100);
            ls.Gap(12f);

            ls.Label(new TaggedString("VCEF_minDaysBetweenRareCatches".Translate() + ": " + VCEF_minDaysBetweenRareCatches + "days"), -1, "VCEF_minDaysBetweenRareCatchesTootip".Translate());
            VCEF_minDaysBetweenRareCatches = (float)Math.Round(ls.Slider(VCEF_minDaysBetweenRareCatches, 0.01f, 10),2);
            ls.Gap(12f);



            ls.End();
        }



    }










}
