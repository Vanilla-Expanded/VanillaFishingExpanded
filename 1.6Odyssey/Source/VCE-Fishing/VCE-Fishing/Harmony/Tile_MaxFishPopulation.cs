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

    [HarmonyPatch(typeof(Tile), nameof(Tile.MaxFishPopulation), MethodType.Getter)]
    public static class VCE_Fishing_Tile_MaxFishPopulation_Patch
    {
               
        [HarmonyPostfix]
        public static void MultiplyMaxFish(ref float __result)
        {
            __result *= VCE_Fishing_Settings.VCEF_maxFishPopulationMultiplier;
           

        }


    }

}