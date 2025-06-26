
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

		public static EffecterDef VCEF_Fishing_MechAncient;
        public static EffecterDef VCEF_Fishing_MechNew;

        [MayRequire("VanillaExpanded.VMemesE")]
        public static ThingDef VCEF_Crayfish;
        [MayRequire("VanillaExpanded.VMemesE")]
        public static ThingDef VCEF_ButterFish;
        [MayRequire("VanillaExpanded.VMemesE")]
        public static ThingDef VCEF_FreshwaterStingray;
        [MayRequire("VanillaExpanded.VMemesE")]
        public static ThingDef VCEF_FlyingFish;
        [MayRequire("VanillaExpanded.VMemesE")]
        public static ThingDef VCEF_Arapaima;
        [MayRequire("VanillaExpanded.VMemesE")]
        public static ThingDef VCEF_ShortfinMakoShark;

        static InternalDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
		}
	}
}
