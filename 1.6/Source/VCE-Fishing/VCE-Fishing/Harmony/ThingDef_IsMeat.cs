using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using RimWorld.BaseGen;



namespace VCE_Fishing
{


    [HarmonyPatch(typeof(ThingDef))]
    [HarmonyPatch("IsMeat", MethodType.Getter)]


    public static class VCE_Fishing_ThingDef_IsMeat_Patch
    {


        [HarmonyPostfix]
        public static void FishAreMeat(ThingDef __instance, ref bool __result)

        {

            if (__instance.category == ThingCategory.Item && __instance.thingCategories != null)
            {
                if (__instance.thingCategories.Contains(InternalDefOf.VCEF_RawFishCategory))
                {
                    __result = true;
                }

               
            }


        }

    }


}