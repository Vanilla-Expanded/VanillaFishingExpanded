using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;
using Verse.AI;

using RimWorld.Planet;
using VCE_Fishing.Options;



namespace VCE_Fishing
{

    [HarmonyPatch(typeof(WaterBody), nameof(WaterBody.Population), MethodType.Getter)]
    public static class VCE_WaterBody_Population_Patch
    {

        [HarmonyPostfix]
        public static void MultiplyMaxFish(ref float __result)
        {
            __result *= VCE_Fishing_Settings.VCEF_fishAmountMultiplier;


        }


    }

}