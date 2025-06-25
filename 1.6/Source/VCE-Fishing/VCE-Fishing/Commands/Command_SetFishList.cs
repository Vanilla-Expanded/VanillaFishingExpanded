using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse.AI;
using Verse;
using System.Linq;



namespace VCE_Fishing
{
    [StaticConstructorOnStartup]
    public class Command_SetFishList : Command
    {

        public Map map;
        public Zone_Fishing zone;
       
       

        public Command_SetFishList()
        {

            defaultDesc = "VCEF_ChooseFishDesc".Translate();
            defaultLabel = "VCEF_ChooseFish".Translate();

            foreach (object obj in Find.Selector.SelectedObjects)
            {
                //Zone_Fishing zone = obj as Zone_Fishing;
                if (obj is Zone_Fishing zone)
                {
                    if (zone.GetFishToCatch() == FishSizeCategory.Small)
                    {
                        this.icon = ContentFinder<Texture2D>.Get("UI/Commands/VCEF_Command_ChooseSmallFish", true);

                    }
                    else if (zone.GetFishToCatch() == FishSizeCategory.Medium)
                    {
                        this.icon = ContentFinder<Texture2D>.Get("UI/Commands/VCEF_Command_ChooseMediumFish", true);

                    }
                    else if (zone.GetFishToCatch() == FishSizeCategory.Large)
                    {
                        this.icon = ContentFinder<Texture2D>.Get("UI/Commands/VCEF_Command_ChooseLargeFish", true);

                    }
                } else 
                this.icon = ContentFinder<Texture2D>.Get("UI/Commands/VCEF_Command_ChooseMediumFish", true);
            }


        }

        public void SetZoneFishList(Zone_Fishing zone)
        {
            zone.InitialSetZoneFishList();
            //if (zone.fishInThisZone != null) {
            //    zone.fishInThisZone.Clear();
            //}
            //bool considerPrecepts = false;
            //Ideo ideo = null;
            //if (ModsConfig.IdeologyActive)
            //{
            //    ideo = Current.Game.World.factionManager.OfPlayer.ideos.PrimaryIdeo;
            //    considerPrecepts = true;
            //}
            //zone.isZonePolluted = false;
            //if (ModsConfig.BiotechActive)
            //{
            //    foreach (IntVec3 cell in zone.cells)
            //    {
            //        if (cell.IsPolluted(zone.Map))
            //        {
            //            zone.isZonePolluted = true;
            //            break;
            //        }
            //    }
            //}
            //if (!zone.isZonePolluted) 
            //{
            //    foreach (FishDef element in DefDatabase<FishDef>.AllDefs.Where(element => element.fishSizeCategory == size))
            //    {
            //        bool flagNoPrecepts = false;
            //        if (considerPrecepts && element.preceptsRequired != null)
            //        {
            //            foreach (string requiredPrecept in element.preceptsRequired)
            //            {
            //                if (ideo.HasPrecept(DefDatabase<PreceptDef>.GetNamedSilentFail(requiredPrecept)))
            //                {
            //                    flagNoPrecepts = true;
            //                }
            //            }
            //        }
            //        else { flagNoPrecepts = true; }
            //        if (flagNoPrecepts)
            //        {
            //            foreach (string biomeTemp in element.allowedBiomes)
            //            {
            //                foreach (BiomeTempDef biometempdef in DefDatabase<BiomeTempDef>.AllDefs.Where(biometempdef => biometempdef.biomeTempLabel == biomeTemp))
            //                {
            //                    foreach (string biome in biometempdef.biomes)
            //                    {
            //                        if (map.Biome.defName == biome)
            //                        {
            //                            if (zone.isOcean && element.canBeSaltwater)
            //                            {
            //                                zone.fishInThisZone.Add(element.thingDef);
            //                            }
            //                            if (!zone.isOcean && element.canBeFreshwater)
            //                            {
            //                                zone.fishInThisZone.Add(element.thingDef);
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //if (zone.fishInThisZone.Count > 0)
            //{
            //    zone.isZoneEmpty = false;
            //}
            //else zone.isZoneEmpty = true;
        }

        public override void ProcessInput(Event ev)
        {
            base.ProcessInput(ev);
            List<FloatMenuOption> list = new List<FloatMenuOption>
            {
                new FloatMenuOption("VCEF_ChooseFishSmallLabel".Translate(), delegate
                {
                    zone.SetFishToCatch(FishSizeCategory.Small);
                    SetZoneFishList(zone);
                }, MenuOptionPriority.Default, null, null, 29f, null, null),
                new FloatMenuOption("VCEF_ChooseFishMediumLabel".Translate(), delegate
                {
                    zone.SetFishToCatch(FishSizeCategory.Medium);
                    SetZoneFishList(zone);
                }, MenuOptionPriority.Default, null, null, 29f, null, null),
                new FloatMenuOption("VCEF_ChooseFishLargeLabel".Translate(), delegate
                {
                    zone.SetFishToCatch(FishSizeCategory.Large);
                    SetZoneFishList(zone);
                }, MenuOptionPriority.Default, null, null, 29f, null, null)
            };
            Find.WindowStack.Add(new FloatMenu(list));
        }

       




    }


}


