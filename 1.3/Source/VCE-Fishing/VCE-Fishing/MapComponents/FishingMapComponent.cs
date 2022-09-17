using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using System.Linq;
using UnityEngine;
using RimWorld.Planet;

namespace VCE_Fishing
{
    public class FishingMapComponent : MapComponent
    {

        private int fishTickProgress = 0;
        private int fishTickMax = 128;

        public FishingMapComponent(Map map) : base(map)
        {
            
        }
        public override void MapComponentTick()
        {
            base.MapComponentTick();

            if (fishTickProgress>= fishTickMax) {
                foreach (Zone zone in this.map.zoneManager.AllZones.Where(zone => zone.GetType() == typeof(Zone_Fishing)))
                {
                    
                    Zone_Fishing zoneFishing = zone as Zone_Fishing;

                    if (zoneFishing.cells.Count < Options.VCE_Fishing_Settings.VCEF_minimumZoneSize)
                    {
                        zoneFishing.isZoneBigEnough = false;
                    } else zoneFishing.isZoneBigEnough = true;

                    int index = 0;
                    while (index < zoneFishing.cells.Count)
                    {
                       if (zoneFishing.cells[index].GetTerrain(this.map).defName== "WaterOceanDeep"|| zoneFishing.cells[index].GetTerrain(this.map).defName == "WaterOceanShallow")
                       {
                           
                            zoneFishing.isOcean = true;
                            zoneFishing.initialSetZoneFishList();
                            break;
                       }
                       index++;
                       zoneFishing.isOcean = false;
                    }
                }
                fishTickProgress = 0;
            }
            fishTickProgress++;



        }
    }
}