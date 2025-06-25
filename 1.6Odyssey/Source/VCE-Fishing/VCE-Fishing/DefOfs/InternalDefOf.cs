
using RimWorld;
using Verse;


namespace VCE_Fishing
{
	[DefOf]
	public static class InternalDefOf
	{
	

		public static TraitDef VCEF_Fisherman;

		public static ThoughtDef VCEF_FishingThought;

		public static HediffDef VCEF_CaughtSpecialHediff;

        public static StatDef VCEF_FishingLuckOffset;

        static InternalDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
		}
	}
}
