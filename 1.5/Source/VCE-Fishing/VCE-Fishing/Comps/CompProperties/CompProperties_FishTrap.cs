using System;
using Verse;

namespace VCE_Fishing
{
	public class CompProperties_FishTrap : CompProperties
	{
		public CompProperties_FishTrap()
		{
			this.compClass = typeof(CompFishTrap);
		}

		public IntRange spawnIntervalRange = new IntRange(100, 100);

		public bool spawnForbidden = false;

		public bool writeTimeLeftToSpawn = true;

		public string saveKeysPrefix;

	

		public string fishTrapDescriptionInspect = "";

		public string fishSpawnedIn = "";

		public int fishTrapUses = 10;

		public FishSizeCategory fishSize = FishSizeCategory.Small;
	}
}

