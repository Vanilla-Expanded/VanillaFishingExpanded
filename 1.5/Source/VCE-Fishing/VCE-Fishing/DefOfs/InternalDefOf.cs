﻿
using RimWorld;
using Verse;


namespace VCE_Fishing
{
	[DefOf]
	public static class InternalDefOf
	{
		public static ThingDef VCEF_RawMackerel;

		public static TraitDef VCEF_Fisherman;

		public static ThoughtDef VCEF_FishingThought;

		public static HediffDef VCEF_CaughtSpecialHediff;

		public static JobDef VCEF_FishJob;

		public static WorkTypeDef VCEF_Fishing;

		public static ThingCategoryDef VCEF_RawFishCategory;

		public static StatDef VCEF_FishingSpeedFactor;
        public static StatDef VCEF_FishingYieldFactor;
        public static StatDef VCEF_FishingLuckOffset;

        static InternalDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
		}
	}
}
