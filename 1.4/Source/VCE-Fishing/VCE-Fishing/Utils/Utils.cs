using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VCE_Fishing;
using Verse;
using static RimWorld.BaseGen.SymbolStack;

namespace VCE_Fishing
{
    public class Utils
    {
        
        public List<ThingDef> GetFishList(byte fishSize, BiomeDef biomeToConsider, TerrainDef terrain)
        {
            List<ThingDef> fishList = new List<ThingDef>();

            FishSizeCategory fishSizeCategory = (FishSizeCategory)fishSize;

            foreach (FishDef element in DefDatabase<FishDef>.AllDefs.Where(element => element.fishSizeCategory == fishSizeCategory 
            && element.preceptsRequired.NullOrEmpty()))
            {
                foreach (string biomeTemp in element.allowedBiomes)
                {
                    foreach (BiomeTempDef biometempdef in DefDatabase<BiomeTempDef>.AllDefs.Where(biometempdef => biometempdef.biomeTempLabel == biomeTemp))
                    {
                        foreach (string biome in biometempdef.biomes)
                        {
                            if (biomeToConsider.defName == biome)
                            {
                                if (IsTerrainOcean(terrain) && element.canBeSaltwater)
                                {
                                    fishList.Add(element.thingDef);
                                }
                                if (!IsTerrainOcean(terrain) && element.canBeFreshwater)
                                {
                                    fishList.Add(element.thingDef);
                                }
                            }
                        }
                    }
                }
            }
            return fishList;
        }

        public bool IsTerrainOcean(TerrainDef terrain)
        {
            return (terrain == TerrainDefOf.WaterOceanDeep || terrain == TerrainDefOf.WaterOceanShallow);
            
        }

    
    }
}
