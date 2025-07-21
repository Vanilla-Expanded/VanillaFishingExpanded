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
    [HarmonyPatch("GetNegativeFishingOutcomes")]

    public static class VCE_Fishing_FishingUtility_GetNegativeFishingOutcomes_Patch
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            var codes = codeInstructions.ToList();


            var modifyNegativeOutcomesChance = AccessTools.Method(typeof(VCE_Fishing_FishingUtility_GetNegativeFishingOutcomes_Patch), "ModifyNegativeOutcomesChance");
          

            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldc_R4 && codes[i].OperandIs(0.02f))
                {


                    yield return new CodeInstruction(OpCodes.Call, modifyNegativeOutcomesChance);

                }
              


                else yield return codes[i];
            }
        }


        public static float ModifyNegativeOutcomesChance()
        {         
            return VCE_Fishing_Settings.VCEF_chanceForNegativeOutcome;
        }

      

    }
}