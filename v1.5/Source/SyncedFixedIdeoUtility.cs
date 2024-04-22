using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using HarmonyLib;

namespace SyncedFixedIdeo
{
    [StaticConstructorOnStartup]
    public static class SyncedFixedIdeoUtility
    {
        static SyncedFixedIdeoUtility()
        {
            foreach (var factionDef in DefDatabase<FactionDef>.AllDefsListForReading)
            {
                var ext = factionDef.GetModExtension<ModExtension_SyncedIdeo>();
                if (ext != null)
                {
                    if (string.IsNullOrEmpty(ext.ideoGroup))
                    {
                        LogError("FactionDef", factionDef.defName, "with modExtension and empty or null ideoGroup. Ignoring");
                        continue;
                    }
                    if (!CachedIdeoGroups.ContainsKey(ext.ideoGroup))
                    {
                        CachedIdeoGroups.Add(ext.ideoGroup, new List<FactionDef>());
                    }
                    CachedIdeoGroups[ext.ideoGroup].Add(factionDef);
                }
            }
            var harmony = new Harmony("rakros.syncedfixedideo");
            harmony.PatchAll();
        }

        public static void LogWarning(params object[] messages)
        {
            var actualMessage = messages.Aggregate("[SyncedFixedIdeo]", (logMessage, message) => logMessage + " " + message.ToStringSafe());
            Log.Warning(actualMessage);
        }

        public static void LogError(params object[] messages)
        {
            var actualMessage = messages.Aggregate("[SyncedFixedIdeo]", (logMessage, message) => logMessage + " " + message.ToStringSafe());
            Log.Error(actualMessage);
        }

        public static void LogMessage(params object[] messages)
        {
            var actualMessage = messages.Aggregate("[SyncedFixedIdeo]", (logMessage, message) => logMessage + " " + message.ToStringSafe());
            Log.Message(actualMessage);
        }

        public static readonly Dictionary<string, List<FactionDef>> CachedIdeoGroups = new Dictionary<string, List<FactionDef>>();
    }
}
