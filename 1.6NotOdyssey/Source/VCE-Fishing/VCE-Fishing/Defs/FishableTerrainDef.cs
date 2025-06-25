using RimWorld;
using System;
using Verse;
using System.Collections.Generic;

namespace VCE_Fishing
{
    public class FishableTerrainDef: Def
    {
        public List<string> allowedTerrains;
        public bool isOcean;
        public bool addEvenIfNotPassable = false;
    }
}
