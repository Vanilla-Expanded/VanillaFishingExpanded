using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;
using UnityEngine;
using VCE_Fishing.Options;
using Verse.AI;
using System.Reflection;


namespace VCE_Fishing
{

    [HarmonyPatch]
    public static class VCE_Fishing_JobDriver_Fish_MakeNewToils_Patch
    {
        static MethodBase TargetMethod()
        {
            MethodBase method = AccessTools.EnumeratorMoveNext(AccessTools.Method(typeof(JobDriver_Fish), "MakeNewToils"));

           return method;
        }


        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            var codes = codeInstructions.ToList();

            FieldInfo field = AccessTools.Field(typeof(EffecterDefOf), nameof(EffecterDefOf.Fishing));
            var modifyEffecter = AccessTools.Method(typeof(VCE_Fishing_JobDriver_Fish_MakeNewToils_Patch), "ModifyEffecter");
            var getStatValue = AccessTools.Method(typeof(StatExtension), "GetStatValue");
            var getStatValueOptions = AccessTools.Method(typeof(VCE_Fishing_JobDriver_Fish_MakeNewToils_Patch), "GetStatValueOptions");

            for (var i = 0; i < codes.Count; i++)
            {

                if (codes[i].opcode == OpCodes.Ldsfld && codes[i].OperandIs(field))
                {


                    yield return new CodeInstruction(OpCodes.Ldloc_3);
                    yield return new CodeInstruction(OpCodes.Call, modifyEffecter);

                }else if (codes[i].opcode == OpCodes.Call && codes[i].OperandIs(getStatValue))
                {

                    yield return new CodeInstruction(OpCodes.Call, getStatValueOptions);


                }

                else yield return codes[i];
            }
        }


        public static EffecterDef ModifyEffecter(Toil toil)
        {
            Pawn pawn = toil.GetActor();
            if(pawn?.IsColonyMech == true)
            {
                if(pawn.ageTracker.AgeChronologicalYears < 100)
                {
                    return InternalDefOf.VCEF_Fishing_MechNew;
                }
                else
                {
                    return InternalDefOf.VCEF_Fishing_MechAncient;
                }
            }
            return EffecterDefOf.Fishing;
        }

        public static float GetStatValueOptions(Pawn pawn, StatDef stat, bool applyPostProcess, int cacheStaleAfterTicks)
        {

            return pawn.GetStatValue(stat, applyPostProcess, cacheStaleAfterTicks) * VCE_Fishing_Settings.VCEF_fishingSpeedMultiplier;
        }

    }
}