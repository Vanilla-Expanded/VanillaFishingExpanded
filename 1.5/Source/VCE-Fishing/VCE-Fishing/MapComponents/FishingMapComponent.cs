using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

using UnityEngine;
using RimWorld.Planet;

namespace VCE_Fishing
{
    public class FishingMapComponent : MapComponent
    {

        private int fishTickProgress = 0;
        private int fishTickMax = 1200;

        public FishingMapComponent(Map map) : base(map)
        {
            
        }
        public override void MapComponentTick()
        {
            //base.MapComponentTick();
            if (fishTickProgress >= fishTickMax)
            {
                //foreach (Zone zone in this.map.zoneManager.AllZones.Where(zone => zone.GetType() == typeof(Zone_Fishing)))
                //{
                //    Zone_Fishing zoneFishing = zone as Zone_Fishing;
                //    zoneFishing.isZonePolluted = false;
                //    if (ModsConfig.BiotechActive)
                //    {
                //        foreach (IntVec3 cell in zoneFishing.cells)
                //        {
                //            if (cell.IsPolluted(zoneFishing.Map))
                //            {
                //                zoneFishing.isZonePolluted = true;
                //                break;
                //            }
                //        }
                //    }
                //    if (zoneFishing.cells.Count < Options.VCE_Fishing_Settings.VCEF_minimumZoneSize)
                //    {
                //        zoneFishing.isZoneBigEnough = false;
                //    } else zoneFishing.isZoneBigEnough = true;
                //    int index = 0;
                //    while (index < zoneFishing.cells.Count)
                //    {
                //       if (zoneFishing.cells[index].GetTerrain(this.map) == TerrainDefOf.WaterOceanDeep|| zoneFishing.cells[index].GetTerrain(this.map) == TerrainDefOf.WaterOceanShallow)
                //       {
                //            zoneFishing.isOcean = true;
                //            zoneFishing.InitialSetZoneFishList();
                //            break;
                //       }
                //       index++;
                //       zoneFishing.isOcean = false;
                //    }
                //}
                foreach (Zone zone in this.map.zoneManager.AllZones)
                {
                    if (zone is Zone_Fishing zoneFishing)
                    {
                        zoneFishing.InitialSetZoneFishList();
                    }
                }
                fishTickProgress = 0;
            }
            fishTickProgress++;
        }
    }
}