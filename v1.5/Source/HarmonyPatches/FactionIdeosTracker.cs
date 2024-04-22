using HarmonyLib;
using RimWorld;
using Verse;

namespace SyncedFixedIdeo.HarmonyPatches
{
    [HarmonyPatch(typeof(FactionIdeosTracker), nameof(FactionIdeosTracker.ChooseOrGenerateIdeo))]
    public class FactionIdeosTracker_ChooseOrGenerateIdeo
    {
        public static bool Prefix(FactionIdeosTracker __instance, IdeoGenerationParms parms)
        {
            var ext = parms.forFaction.GetModExtension<ModExtension_SyncedIdeo>();
            if (ext != null && !string.IsNullOrEmpty(ext.ideoGroup))
            {
#if DEBUG
                SyncedFixedIdeoUtility.LogMessage("Found ideoGroup on ChooseOrGenerateIdeo");
                SyncedFixedIdeoUtility.LogMessage("Searching for faction in ideoGroup");
#endif
                foreach (var faction in Find.FactionManager.AllFactionsListForReading)
                {
                    if (faction.ideos.PrimaryIdeo != null && SyncedFixedIdeoUtility.CachedIdeoGroups.TryGetValue(ext.ideoGroup, out var factionDefs) && factionDefs.Contains(faction.def))
                    {
#if DEBUG
                        SyncedFixedIdeoUtility.LogMessage("Found faction", faction.GetUniqueLoadID());
#endif
                        __instance.SetPrimary(faction.ideos.PrimaryIdeo);
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
