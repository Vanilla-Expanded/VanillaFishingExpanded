using RimWorld;
using Verse;

using UnityEngine;


namespace VCE_Fishing
{
    public static class FishToCatchSettableUtility
    {
        public static Command_SetFishList SetFishToCatchCommand(Zone_Fishing passingZone, Map passingMap)
        {
            return new Command_SetFishList()
            {
                
                hotKey = KeyBindingDefOf.Misc1,
                map = passingMap,
                zone = passingZone

            };
        }

       
    }
}

