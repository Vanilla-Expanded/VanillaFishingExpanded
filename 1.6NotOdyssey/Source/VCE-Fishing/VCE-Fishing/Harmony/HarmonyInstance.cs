using RimWorld;
using System.Reflection;
using Verse;
using HarmonyLib;

namespace VCE_Fishing
{
    //Setting the Harmony instance
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = new HarmonyLib.Harmony("com.VFEFishing");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
