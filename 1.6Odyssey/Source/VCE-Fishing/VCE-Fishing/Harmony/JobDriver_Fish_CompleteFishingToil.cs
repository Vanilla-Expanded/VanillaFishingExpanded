using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;
using Verse.AI;
using System;
using VCE_Fishing.Options;
using System.Reflection;
using RimWorld.QuestGen;
using System.Security.Cryptography;
using System.Security.Principal;


namespace VCE_Fishing
{

    [HarmonyPatch]
    public static class VCE_Fishing_JobDriver_Fish_CompleteFishingToil_Patch
    {
        static MethodBase TargetMethod()
        {
            MethodBase method = typeof(JobDriver_Fish).GetMethod("<CompleteFishingToil>b__4_0", BindingFlags.Instance | BindingFlags.NonPublic);
            return method;
        }
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            var codes = codeInstructions.ToList();

           
            FieldInfo field = AccessTools.Field(typeof(WaterBodyTracker), nameof(WaterBodyTracker.lastRareCatchTick));
            var createRareCatchHediff = AccessTools.Method(typeof(VCE_Fishing_JobDriver_Fish_CompleteFishingToil_Patch), "CreateRareCatchHediff");

            for (var i = 0; i < codes.Count; i++)
            {

                if (codes[i].opcode == OpCodes.Stfld && codes[i].OperandIs(field))
                {

                    yield return codes[i];
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Call, createRareCatchHediff);

                }

                else yield return codes[i];
            }
        }


        public static void CreateRareCatchHediff(JobDriver_Fish job)
        {
           job.pawn?.health?.AddHediff(InternalDefOf.VCEF_CaughtSpecialHediff);
        }

    }

    [HarmonyPatch(typeof(JobDriver_Fish))]
    [HarmonyPatch("CompleteFishingToil")]
    public static class VCE_Fishing_JobDriver_Fish_CompleteFishingToil_PostFix_Patch
    {

        public static List<ThingDef> adeptCatches = new List<ThingDef>() { InternalDefOf.VCEF_Crayfish,InternalDefOf.VCEF_ButterFish,
            InternalDefOf.VCEF_FreshwaterStingray,InternalDefOf.VCEF_FlyingFish,InternalDefOf.VCEF_Arapaima,InternalDefOf.VCEF_ShortfinMakoShark};

        [HarmonyPostfix]
        public static void CompleteFishingToilPostActions(ref Toil __result, JobDriver_Fish __instance)
        {

            __result.AddFinishAction(delegate
            {
                if (ModLister.IdeologyInstalled && __instance.pawn?.ideo?.Ideo?.HasPrecept(DefDatabase<PreceptDef>.GetNamedSilentFail("VME_Recreation_Fishing")) == true)
                {
                    __instance.pawn?.needs?.joy?.GainJoy(0.1f, JoyKindDefOf.Meditative);
                }

                if (ModLister.IdeologyInstalled && __instance.pawn?.ideo?.Ideo?.HasPrecept(DefDatabase<PreceptDef>.GetNamedSilentFail("VME_Fishing_Adept")) == true)
                {
                    if (Rand.Chance(0.2f))
                    {
                        Thing thing = ThingMaker.MakeThing(adeptCatches.RandomElement());
                        thing.stackCount = 10;
                        GenPlace.TryPlaceThing(thing, __instance.pawn.Position, __instance.pawn.Map, ThingPlaceMode.Near);
                    }
                    
                }

                if (__instance.pawn?.story?.traits?.HasTrait(InternalDefOf.VCEF_Fisherman) == true)
                {
                    __instance.pawn?.needs?.mood?.thoughts?.memories?.TryGainMemory(InternalDefOf.VCEF_FishingThought);
                }

            });

        }


    }

}