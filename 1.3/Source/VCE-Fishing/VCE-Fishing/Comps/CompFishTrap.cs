using System;
using System.Collections.Generic;
using Verse;
using RimWorld;
using System.Linq;
using UnityEngine;

namespace VCE_Fishing
{
    public class CompFishTrap : ThingComp
    {
        public List<ThingDef> fishList = new List<ThingDef>();

        public bool isOcean = false;

        public bool rebuild = true;

        public int fishTrapUsesInternal;

        public CompProperties_FishTrap PropsSpawner
        {
            get
            {
                return (CompProperties_FishTrap)this.props;
            }
        }

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            fishTrapUsesInternal = PropsSpawner.fishTrapUses;
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            Building building = this.parent as Building;

            foreach (IntVec3 cell in building.OccupiedRect().Cells)
            {
                if (cell.GetTerrain(building.Map).defName == "WaterOceanDeep" || cell.GetTerrain(building.Map).defName == "WaterOceanShallow")
                {
                    isOcean = true;
                    break;
                }
            }

            SetZoneFishList();

            if (!respawningAfterLoad)
            {
                this.ResetCountdown();
            }
        }

        public void SetZoneFishList()
        {
            fishList.Clear();
            bool considerPrecepts = false;
            Ideo ideo = null;

            if (ModLister.IdeologyInstalled)
            {
                ideo = Current.Game.World.factionManager.OfPlayer.ideos.PrimaryIdeo;
                considerPrecepts = true;

            }

            foreach (FishDef element in DefDatabase<FishDef>.AllDefs.Where(element => element.fishSizeCategory == PropsSpawner.fishSize))
            {
                bool flagNoPrecepts = false;
                if (considerPrecepts && element.preceptsRequired != null)
                {

                    foreach (string requiredPrecept in element.preceptsRequired)
                    {
                        if (ideo.HasPrecept(DefDatabase<PreceptDef>.GetNamedSilentFail(requiredPrecept)))
                        {
                            flagNoPrecepts = true;
                        }
                    }

                }
                else { flagNoPrecepts = true; }

                if (flagNoPrecepts)
                {
                    foreach (string biomeTemp in element.allowedBiomes)
                    {

                        foreach (BiomeTempDef biometempdef in DefDatabase<BiomeTempDef>.AllDefs.Where(biometempdef => biometempdef.biomeTempLabel == biomeTemp))
                        {
                            foreach (string biome in biometempdef.biomes)
                            {


                                if (this.parent.Map.Biome.defName == biome)
                                {
                                    if (this.isOcean && element.canBeSaltwater)
                                    {
                                        fishList.Add(element.thingDef);
                                    }
                                    if (!this.isOcean && element.canBeFreshwater)
                                    {
                                        fishList.Add(element.thingDef);

                                    }
                                }
                            }
                        }
                    }


                }

            }

        }




        public override void CompTickRare()
        {
            if (!this.parent.Spawned)
            {
                return;
            }

            else if (this.parent.Position.Fogged(this.parent.Map))
            {
                return;
            }

            this.ticksUntilSpawn -= 250;
            this.CheckShouldSpawn();
        }


        private void CheckShouldSpawn()
        {
            if (this.ticksUntilSpawn <= 0)
            {
                this.TryDoSpawn();
                this.ResetCountdown();
            }
        }



        public bool TryDoSpawn()
        {
            if (!this.parent.Spawned)
            {
                return false;
            }


            Thing thing = ThingMaker.MakeThing(this.fishList.RandomElement(), null);
            FishDef fishDef = DefDatabase<FishDef>.AllDefs.Where(element => element.thingDef == thing.def).FirstOrDefault();            

            thing.stackCount = CalculateFishAmountWithConditions(fishDef.baseFishingYield);

            if (thing == null)
            {
                Log.Error("Could not spawn anything for " + this.parent);
            }
            
            Thing t;
            GenPlace.TryPlaceThing(thing, this.parent.InteractionCell, this.parent.Map, ThingPlaceMode.Direct, out t, null, null, default(Rot4));
            if (this.PropsSpawner.spawnForbidden)
            {
                t.SetForbidden(true, true);
            }
            fishTrapUsesInternal--;
            if (fishTrapUsesInternal < 1)
            {
               
                if (rebuild)
                {
                   
                        GenConstruct.PlaceBlueprintForBuild_NewTemp(this.parent.def, this.parent.Position, this.parent.Map,this.parent.Rotation, Faction.OfPlayer, this.parent.Stuff, null, null);

                        
                }
               
                this.parent.Destroy(DestroyMode.Vanish);
            }
          
            return true;

        }

        public int CalculateFishAmountWithConditions(int amount)
        {
            int fishAmountFinal = amount;
           
            float currentTempInMap = this.parent.Map.mapTemperature.OutdoorTemp;

            if (currentTempInMap < Options.VCE_Fishing_Settings.VCEF_minMapTempForLowFish)
            {
                fishAmountFinal = (int)(amount * Mathf.InverseLerp(Options.VCE_Fishing_Settings.VCEF_minMapTempForLowFish - 20f, Options.VCE_Fishing_Settings.VCEF_minMapTempForLowFish, currentTempInMap));
            }
            else if (currentTempInMap > Options.VCE_Fishing_Settings.VCEF_maxMapTempForLowFish)
            {
                fishAmountFinal = (int)(amount * Mathf.InverseLerp(Options.VCE_Fishing_Settings.VCEF_maxMapTempForLowFish + 15, Options.VCE_Fishing_Settings.VCEF_maxMapTempForLowFish, currentTempInMap));
            }

            if (fishAmountFinal < 1) { fishAmountFinal = 1; }

            return fishAmountFinal;

        }


        private void ResetCountdown()
        {

            this.ticksUntilSpawn = this.PropsSpawner.spawnIntervalRange.RandomInRange;


        }

        public override void PostExposeData()
        {
            string str = this.PropsSpawner.saveKeysPrefix.NullOrEmpty() ? null : (this.PropsSpawner.saveKeysPrefix + "_");
            Scribe_Values.Look<int>(ref this.ticksUntilSpawn, str + "ticksUntilSpawn", 0, false);
            Scribe_Values.Look<int>(ref this.fishTrapUsesInternal, str + "fishTrapUsesInternal", 0, false);
            Scribe_Values.Look<bool>(ref this.rebuild, str + "rebuild", true, false);


        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (Prefs.DevMode)
            {
                yield return new Command_Action
                {
                    defaultLabel = "DEBUG: Spawn fish",
                    icon = TexCommand.DesirePower,
                    action = delegate ()
                    {
                        this.TryDoSpawn();
                        this.ResetCountdown();
                    }
                };
            }

            /*yield return new Command_Action
            {
                defaultLabel = "DEBUG: turn rebuild off",
                icon = TexCommand.DesirePower,
                action = delegate ()
                {
                    this.rebuild=false;
                   
                }
            };*/

            yield break;
        }

        public override string CompInspectStringExtra()
        {
            string returnString = "";

            returnString += PropsSpawner.fishTrapDescriptionInspect.Translate();

            if (this.fishList != null)
            {
                returnString += " ";
                IList<string> fishInThisZoneString = new List<string>();
                foreach (ThingDef fish in this.fishList)
                {
                    fishInThisZoneString.Add(fish.label);
                }
                string[] array = fishInThisZoneString.ToArray();
                string joined = string.Join(", ", array);
                returnString += joined;
            }

            returnString += "\n" + "VCEF_IsZoneOceanZone".Translate();
            if (this.isOcean)
            {
                returnString += "VCEF_Yes".Translate();
            }
            else returnString += "VCEF_No".Translate();

            if (PropsSpawner.writeTimeLeftToSpawn)
            {

                returnString+= "\n"+PropsSpawner.fishSpawnedIn.Translate() + ": " + this.ticksUntilSpawn.ToStringTicksToPeriod(true, false, true, true);


            }
            return returnString+= "\n" + "VME_UsesLeftUntilItBreaks".Translate(fishTrapUsesInternal);


        }

        private int ticksUntilSpawn;
    }
}
