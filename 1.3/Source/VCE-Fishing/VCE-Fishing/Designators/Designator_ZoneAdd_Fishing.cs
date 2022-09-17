using System;
using UnityEngine;
using Verse;
using RimWorld;

namespace VCE_Fishing
{
    public class Designator_ZoneAdd_Fishing : Designator_ZoneAdd
    {
        protected override string NewZoneLabel
        {
            get
            {
                return "VCEF_FishingGrowingZone".Translate();
            }
        }

        public Designator_ZoneAdd_Fishing()
        {

            this.zoneTypeToPlace = typeof(Zone_Fishing);
            this.defaultLabel = "VCEF_FishingGrowingZone".Translate();
            this.defaultDesc = "VCEF_FishingGrowingZoneDesc".Translate();
            this.icon = ContentFinder<Texture2D>.Get("UI/Designators/VCEF_ZoneCreate_Fishing", true);
            this.hotKey = KeyBindingDefOf.Misc2;

         
        }

        public override AcceptanceReport CanDesignateCell(IntVec3 c)
        {
            if (!base.CanDesignateCell(c).Accepted)
            {
                return false;
            }

            TerrainDef terrainDef = Map.terrainGrid.TerrainAt(c);

            foreach (FishableTerrainDef element in DefDatabase<FishableTerrainDef>.AllDefs)
            {
                foreach (string allowed in element.allowedTerrains)
                {
                    if ((allowed == terrainDef.defName)&& c.Walkable(Map))
                    {
                        return true;

                    }else if ((allowed == terrainDef.defName) && element.addEvenIfNotPassable)
                    {
                        return true;

                    }

                }
            }
            return false;



        }
                
        protected override Zone MakeNewZone()
        {
           
            return new Zone_Fishing(Find.CurrentMap.zoneManager);
        }
    }
}

