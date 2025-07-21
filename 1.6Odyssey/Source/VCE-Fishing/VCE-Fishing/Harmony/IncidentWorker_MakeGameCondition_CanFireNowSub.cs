using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;
using Verse.AI;
using VCE_Fishing.Options;
using RimWorld.Planet;




namespace VCE_Fishing
{

    [HarmonyPatch(typeof(IncidentWorker_MakeGameCondition))]
    [HarmonyPatch("CanFireNowSub")]
    public static class VCE_Fishing_IncidentWorker_MakeGameCondition_CanFireNowSub_Patch
    {

        [HarmonyPostfix]
        public static void RemoveGillRot(IncidentParms parms, IncidentWorker_MakeGameCondition __instance,ref bool __result)
        {
            GameConditionDef gameConditionDef = __instance.GetGameConditionDef(parms);
            if (VCE_Fishing_Settings.VCEF_DisableGillRotBase && gameConditionDef == GameConditionDefOf.GillRot) {

                __result = false;
            }

        }


    }

}