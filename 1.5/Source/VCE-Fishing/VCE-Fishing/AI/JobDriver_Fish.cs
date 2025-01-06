using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;
using System.Linq;
using UnityEngine;



namespace VCE_Fishing
{
    class JobDriver_Fish : JobDriver
    {
        public float pawnFishingSkill = 10;
        public FishSizeCategory sizeAtBeginning = FishSizeCategory.Medium;
        public ThingDef fishCaught = InternalDefOf.VCEF_RawMackerel;
        public int fishAmount = 1;
        public int fishAmountWithSkill;
        public int minFishingSkillForMinYield = 6;
        public float skillGainperTick = 0.025f;
        public float fishingSkill = 1;
        Zone_Fishing fishingZone;
        public bool caughtSomethingSpecial = false;

        private System.Random rand = new System.Random();

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<float>(ref this.pawnFishingSkill, "pawnFishingSkill", 0f, true);
            Scribe_Values.Look<int>(ref this.fishAmount, "fishAmount", 1, true);
            Scribe_Values.Look<int>(ref this.fishAmountWithSkill, "fishAmountWithSkill", 1, true);
            Scribe_Values.Look<FishSizeCategory>(ref this.sizeAtBeginning, "sizeAtBeginning", FishSizeCategory.Medium, true);
            Scribe_Defs.Look<ThingDef>(ref this.fishCaught, "fishCaught");
            Scribe_References.Look<Zone_Fishing>(ref this.fishingZone, "fishingZone");
            Scribe_Values.Look<bool>(ref this.caughtSomethingSpecial, "caughtSomethingSpecial", false, true);
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            fishingZone = this.Map.zoneManager.ZoneAt(this.job.targetA.Cell) as Zone_Fishing;
            sizeAtBeginning = fishingZone.GetFishToCatch();
            caughtSomethingSpecial = false;
            fishCaught = SelectFishToCatch(fishingZone);
            bool dontScaleFishingYieldWithSkill = false;
            foreach (FishDef element in DefDatabase<FishDef>.AllDefs.Where(element => element.thingDef == fishCaught))
            {
                fishAmount = element.baseFishingYield;
                dontScaleFishingYieldWithSkill = element.dontScaleFishingYieldWithSkill;
            }
            fishAmountWithSkill = CalculateFishAmountWithSkillAndConditions(fishAmount, dontScaleFishingYieldWithSkill);
            Pawn pawn = this.pawn;
            LocalTargetInfo target = this.job.targetA;
            Job job = this.job;
            bool result;
            if (pawn.Reserve(target, job, 1, -1, null, errorOnFailed))
            {
                pawn = this.pawn;
                target = this.job.targetA.Cell;
                job = this.job;
                int index = 0;
                LocalTargetInfo moreTargetCells;
                while (index < fishingZone.cells.Count)
                {
                    moreTargetCells = fishingZone.cells[index];
                    pawn.Reserve(moreTargetCells, job, 1, -1, null, errorOnFailed);
                    index++;
                }
                result = pawn.Reserve(target, job, 1, -1, null, errorOnFailed);
            }
            else
            {
                result = false;
            }
            return result;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            if (fishCaught == null)
            {
                this.EndJobWith(JobCondition.Incompletable);
            }
            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.FailOnBurningImmobile(TargetIndex.A);
            yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
            this.pawn?.rotationTracker?.FaceTarget(base.TargetA);
            Toil fishToil = new Toil();
            if (this.pawn?.story?.traits?.HasTrait(InternalDefOf.VCEF_Fisherman)==true)
            {
                this.pawn?.needs?.mood?.thoughts?.memories?.TryGainMemory(InternalDefOf.VCEF_FishingThought);
            }
            fishToil.tickAction = delegate ()
            {
                this.pawn?.skills?.Learn(SkillDefOf.Animals, skillGainperTick);
                if (fishingZone != null)
                {
                    if (!fishingZone.isZoneBigEnough)
                    {
                        this.EndJobWith(JobCondition.Incompletable);
                    }
                }
            };
            Rot4 pawnRotation = pawn.Rotation;
            IntVec3 facingCell = pawnRotation.FacingCell;
            EffecterDef effecter = SelectFishingMote(facingCell);
            fishToil.WithEffect(() => effecter, () => this.TargetA.Cell + facingCell);
            fishToil.defaultCompleteMode = ToilCompleteMode.Delay;
            int duration;
            switch (sizeAtBeginning)
            {
                case FishSizeCategory.Small:
                    duration = (int)(-(Options.VCE_Fishing_Settings.VCEF_smallFishDurationFactor / 20) * fishingSkill + Options.VCE_Fishing_Settings.VCEF_smallFishDurationFactor * 1.5);
                    fishToil.defaultDuration = (int)(duration * pawn.GetStatValue(InternalDefOf.VCEF_FishingSpeedFactor));
                    break;
                case FishSizeCategory.Medium:
                    int mediumFishDurationFactor = Options.VCE_Fishing_Settings.VCEF_smallFishDurationFactor * 2;
                    duration = (int)(-(mediumFishDurationFactor / 20) * fishingSkill + mediumFishDurationFactor * 1.5);
                    fishToil.defaultDuration = (int)(duration * pawn.GetStatValue(InternalDefOf.VCEF_FishingSpeedFactor));
                    break;
                case FishSizeCategory.Large:
                    int largeFishDurationFactor = Options.VCE_Fishing_Settings.VCEF_smallFishDurationFactor * 3;
                    duration = (int)(-(largeFishDurationFactor / 20) * fishingSkill + largeFishDurationFactor * 1.5);
                    fishToil.defaultDuration = (int)(duration * pawn.GetStatValue(InternalDefOf.VCEF_FishingSpeedFactor));
                    break;
                default:
                    fishToil.defaultDuration = (int)(Options.VCE_Fishing_Settings.VCEF_smallFishDurationFactor * 2 * pawn.GetStatValue(InternalDefOf.VCEF_FishingSpeedFactor));
                    break;
            }
            List<Apparel> wornApparel = this.pawn?.apparel?.WornApparel;
            if (wornApparel != null) {
                for (int i = 0; i < wornApparel.Count; i++)
                {
                    if (wornApparel[i].def?.defName == "VME_TackleBox")
                    {
                        fishToil.defaultDuration = (int)(fishToil.defaultDuration * 0.8f);
                    }
                }
            }
            fishToil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            yield return fishToil.WithProgressBarToilDelay(TargetIndex.A, true);
            yield return new Toil
            {
                initAction = delegate
                {
                    if (ModLister.IdeologyInstalled && this.pawn?.ideo?.Ideo?.HasPrecept(DefDatabase<PreceptDef>.GetNamedSilentFail("VME_Recreation_Fishing"))==true)
                    {
                        this.pawn?.needs?.joy?.GainJoy(0.1f, JoyKindDefOf.Meditative);
                    }
                    Thing newFish = ThingMaker.MakeThing(fishCaught);
                    newFish.stackCount = fishAmountWithSkill;
                    GenSpawn.Spawn(newFish, pawn.Position + pawnRotation.Opposite.FacingCell, this.Map);
                    if (caughtSomethingSpecial)
                    {
                        Messages.Message("VCEF_CaughtSpecial".Translate(this.pawn.LabelCap, newFish.LabelCap), this.pawn, MessageTypeDefOf.NeutralEvent);
                        this.pawn?.health?.AddHediff(InternalDefOf.VCEF_CaughtSpecialHediff);
                        caughtSomethingSpecial = false;
                    }
                    StoragePriority currentPriority = StoreUtility.CurrentStoragePriorityOf(newFish);
                    if (StoreUtility.TryFindBestBetterStoreCellFor(newFish, this.pawn, this.Map, currentPriority, this.pawn.Faction, out IntVec3 c, true))
                    {
                        this.job.SetTarget(TargetIndex.C, c);
                        this.job.SetTarget(TargetIndex.B, newFish);
                        this.job.count = newFish.stackCount;
                    }
                    else
                    {
                        this.EndJobWith(JobCondition.Incompletable);
                    }
                },
                defaultCompleteMode = ToilCompleteMode.Instant
            };
            if (!Options.VCE_Fishing_Settings.VCEF_dropFish)
            {
                yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
                yield return Toils_Reserve.Reserve(TargetIndex.C, 1, -1, null);
                yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch);
                yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false);
                Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.C);
                yield return carryToCell;
                yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, carryToCell, true);
            }
            yield break;
        }

        public int CalculateFishAmountWithSkillAndConditions(int amount, bool dontScaleFishingYieldWithSkill)
        {
            if (dontScaleFishingYieldWithSkill)
            {
                return amount;
            }
            if (this.pawn.IsColonyMech)
            {
                fishingSkill = 10;
            } 
            else
            {
                fishingSkill = this.pawn.skills.AverageOfRelevantSkillsFor(InternalDefOf.VCEF_Fishing);
            }
            int fishAmountFinal;
            if (fishingSkill >= minFishingSkillForMinYield)
            {
                fishAmountFinal = amount + (int)((fishingSkill - minFishingSkillForMinYield) / 2);
            }
            else
            {
                fishAmountFinal = (int)(amount - (minFishingSkillForMinYield - fishingSkill));
            }
            float currentTempInMap = this.Map.mapTemperature.OutdoorTemp;
            if (currentTempInMap < Options.VCE_Fishing_Settings.VCEF_minMapTempForLowFish)
            {
                fishAmountFinal = (int)(fishAmountFinal * Mathf.InverseLerp(Options.VCE_Fishing_Settings.VCEF_minMapTempForLowFish - 20f, Options.VCE_Fishing_Settings.VCEF_minMapTempForLowFish, currentTempInMap));
            }
            else if (currentTempInMap > Options.VCE_Fishing_Settings.VCEF_maxMapTempForLowFish)
            {
                fishAmountFinal = (int)(fishAmountFinal * Mathf.InverseLerp(Options.VCE_Fishing_Settings.VCEF_maxMapTempForLowFish + 15, Options.VCE_Fishing_Settings.VCEF_maxMapTempForLowFish, currentTempInMap));
            }
            float statMultiplier = pawn.GetStatValue(InternalDefOf.VCEF_FishingYieldFactor);
            fishAmountFinal = (int)(fishAmountFinal*statMultiplier);

            if (fishAmountFinal < 1) { fishAmountFinal = 1; }
            return fishAmountFinal;
        }


        public ThingDef SelectFishToCatch(Zone_Fishing fishingZone)
        {
            if (fishingZone.fishInThisZone.Count > 0)
            {
                
                if (Rand.Chance((float)Options.VCE_Fishing_Settings.VCEF_chanceForSpecials / 100 + pawn.GetStatValue(InternalDefOf.VCEF_FishingLuckOffset)))
                {
                    List<FishDef> tempSpecialFish = new List<FishDef>();
                    tempSpecialFish.Clear();
                    foreach (FishDef element in DefDatabase<FishDef>.AllDefs.Where(element => element.fishSizeCategory == FishSizeCategory.Special))
                    {
                        tempSpecialFish.Add(element);
                    }
                    caughtSomethingSpecial = true;
                    return tempSpecialFish.RandomElementByWeight(((FishDef s) => s.commonality)).thingDef;
                }
                else
                {
                    ThingDef fishCaught = fishingZone.fishInThisZone.RandomElement();
                    return fishCaught;
                }
            }
            else return null;
        }

        public EffecterDef SelectFishingMote(IntVec3 facingCell)
        {
            string baseString = "VCEF_FishingEffect";
            string mechString = this.pawn.IsColonyMech ? "_Mech" : "";
            string mechNew = (this.pawn.IsColonyMech && this.pawn.ageTracker.AgeChronologicalYears<100) ? "_New" : "";
            string rotationString = "";
            if (facingCell == new IntVec3(0, 0, 1))
            {
                rotationString = "_North";
            }
            else if (facingCell == new IntVec3(1, 0, 0))
            {
                rotationString = "_East";
            }
            else if (facingCell == new IntVec3(0, 0, -1))
            {
                rotationString = "_South";
            }
            else if (facingCell == new IntVec3(-1, 0, 0))
            {
                rotationString = "_West";
            }
            return DefDatabase<EffecterDef>.GetNamed(baseString+ mechString+ mechNew+rotationString);
        }

    }
}
