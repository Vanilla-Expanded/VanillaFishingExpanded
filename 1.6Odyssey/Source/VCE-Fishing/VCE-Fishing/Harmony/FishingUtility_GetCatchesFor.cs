using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;
using UnityEngine;
using System;
using VCE_Fishing.Options;


namespace VCE_Fishing
{

    [HarmonyPatch(typeof(FishingUtility))]
    [HarmonyPatch("GetCatchesFor")]

    public static class VCE_Fishing_FishingUtility_GetCatchesFor_Patch
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            var codes = codeInstructions.ToList();

           
            var modifyRareCatchesChance = AccessTools.Method(typeof(VCE_Fishing_FishingUtility_GetCatchesFor_Patch), "ModifyRareCatchesChance");
            var modifyMinRareCatchPeriod = AccessTools.Method(typeof(VCE_Fishing_FishingUtility_GetCatchesFor_Patch), "ModifyMinRareCatchPeriod");
            var getStatValue = AccessTools.Method(typeof(StatExtension), "GetStatValue");
            var getStatValueOptions = AccessTools.Method(typeof(VCE_Fishing_FishingUtility_GetCatchesFor_Patch), "GetStatValueOptions");

            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldc_I4 && codes[i].OperandIs(300000))
                {


                    yield return new CodeInstruction(OpCodes.Call, modifyMinRareCatchPeriod);

                }else
                if (codes[i].opcode == OpCodes.Ldc_R4 && codes[i].OperandIs(0.01f))
                {
                    var labels1 = codes[i].ExtractLabels();

                    yield return new CodeInstruction(OpCodes.Ldarg_0).WithLabels(labels1);
                    yield return new CodeInstruction(OpCodes.Call, modifyRareCatchesChance);

                }
                else
                if (codes[i].opcode == OpCodes.Call && codes[i].OperandIs(getStatValue))
                {
                   
                    yield return new CodeInstruction(OpCodes.Call, getStatValueOptions);

                }



                else yield return codes[i];
            }
        }


        public static float ModifyRareCatchesChance(Pawn pawn)
        {
            float rareChance = VCE_Fishing_Settings.VCEF_chanceForSpecials / 100;

            rareChance += pawn.GetStatValue(InternalDefOf.VCEF_FishingLuckOffset);

            return rareChance;
        }

        public static int ModifyMinRareCatchPeriod()
        {
           

            return (int)(VCE_Fishing_Settings.VCEF_minDaysBetweenRareCatches *60000);
        }

        public static float GetStatValueOptions(Pawn pawn, StatDef stat, bool applyPostProcess, int cacheStaleAfterTicks)
        {

            return pawn.GetStatValue(stat, applyPostProcess, cacheStaleAfterTicks) * VCE_Fishing_Settings.VCEF_fishingYieldMultiplier;
        }

    }
}