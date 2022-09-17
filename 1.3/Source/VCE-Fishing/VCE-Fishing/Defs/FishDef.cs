using RimWorld;
using System;
using Verse;
using System.Collections.Generic;


namespace VCE_Fishing
{
    public class FishDef : Def
    {
        public ThingDef thingDef;       
        public List<string> allowedBiomes;
        public bool canBeFreshwater;
        public bool canBeSaltwater;
        public FishSizeCategory fishSizeCategory;
        public int baseFishingYield = 1;
        public float commonality = 1f;
        public bool dontScaleFishingYieldWithSkill = false;
        public List<string> preceptsRequired = null;

    }
}
