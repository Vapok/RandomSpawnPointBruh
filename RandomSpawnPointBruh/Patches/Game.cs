using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using HarmonyLib;
using JetBrains.Annotations;
using Jotunn.Managers;
using RandomSpawnPointBruh.Components;
using RandomSpawnPointBruh.Configuration;
using UnityEngine;

namespace RandomSpawnPointBruh.Patches;

public static class GamePatches
{
    [HarmonyPatch]
    static class GameFindSpawnPointPatch
    {
        [UsedImplicitly]
        private static IEnumerable<MethodInfo> TargetMethods() => GetMethods();

        private static IEnumerable<MethodInfo> GetMethods()
        {
            List<MethodInfo> result = new();
            
            result.Add(AccessTools.DeclaredMethod(typeof(Game), nameof(Game.FindSpawnPoint)));

            return result;
        }
        
        [UsedImplicitly]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            bool patchedSuccess = false;
            bool patchedSuccess2 = false;
            List<CodeInstruction> instrs = instructions.ToList();

            int counter = 0;

            CodeInstruction LogMessage(CodeInstruction instruction)
            {
                RandomSpawnPointBruh.Log.Debug(
                    $"IL_{counter}: Opcode: {instruction.opcode} Operand: {instruction.operand}");
                return instruction;
            }

            MethodInfo zoneSystemInstance = AccessTools.DeclaredPropertyGetter(typeof(ZoneSystem), nameof(ZoneSystem.instance));
            MethodInfo getLocationMethod = AccessTools.DeclaredMethod(typeof(ZoneSystem), nameof(ZoneSystem.GetLocationIcon));
            FieldInfo startLocationField = AccessTools.DeclaredField(typeof(Game), nameof(Game.m_StartLocation));

            for (int i = 0; i < instrs.Count; ++i)
            {
                if (!RandomSpawnPointBruh.Main.HasCompetingMods && i > 5 && instrs[i].opcode == OpCodes.Callvirt &&
                    instrs[i].operand.Equals(getLocationMethod) && instrs[i - 2].opcode == OpCodes.Ldfld &&
                    instrs[i - 2].operand.Equals(startLocationField))
                {
                    yield return LogMessage(new CodeInstruction(OpCodes.Call,
                        AccessTools.DeclaredMethod(typeof(SpawnPointGenerator),
                            nameof(SpawnPointGenerator.GetSpawnPoint))));
                    counter++;

                    patchedSuccess = true;
                } else if (RandomSpawnPointBruh.Main.HasCompetingMods && i > 5 && instrs[i].opcode == OpCodes.Callvirt 
                           && instrs[i-2].opcode == OpCodes.Ldfld && instrs[i-2].operand.Equals(startLocationField))
                {
                    yield return LogMessage(new CodeInstruction(OpCodes.Call,
                        AccessTools.DeclaredMethod(typeof(SpawnPointGenerator),
                            nameof(SpawnPointGenerator.GetSpawnPoint))));
                    counter++;

                    patchedSuccess = true;
                } else if (!RandomSpawnPointBruh.Main.HasCompetingMods && i > 5 && instrs[i].opcode == OpCodes.Call && instrs[i].operand.Equals(zoneSystemInstance)
                           && instrs[i+4].opcode == OpCodes.Callvirt && instrs[i+4].operand.Equals(getLocationMethod)
                           && instrs[i+2].opcode == OpCodes.Ldfld && instrs[i+2].operand.Equals(startLocationField))
                {
                    if (instrs[i].labels.Count > 0)
                        instrs[i].MoveLabelsTo(instrs[i+1]);

                    patchedSuccess2 = true;
                } else if (RandomSpawnPointBruh.Main.HasCompetingMods && i > 5 && instrs[i].opcode == OpCodes.Call && instrs[i].operand.Equals(zoneSystemInstance)
                           && instrs[i+4].opcode == OpCodes.Callvirt
                           && instrs[i+2].opcode == OpCodes.Ldfld && instrs[i+2].operand.Equals(startLocationField))
                {
                    if (instrs[i].labels.Count > 0)
                        instrs[i].MoveLabelsTo(instrs[i+1]);

                    patchedSuccess2 = true;
                }
                else
                {
                    yield return LogMessage(instrs[i]);
                    counter++;
                }
            }

            if (!patchedSuccess || !patchedSuccess2)
            {
                RandomSpawnPointBruh.Log.Error($"Game.FindSpawnPoint Transpiler Failed To Patch");
                RandomSpawnPointBruh.Log.Warning($"Patch1: {patchedSuccess} - Patch 2: {patchedSuccess2}");
                Thread.Sleep(15000);
            }
        }
    }

    [HarmonyPatch(typeof(Game), "SpawnPlayer")]
    internal static class GameSpawnPlayerPatch
    {
        [HarmonyPrefix]
        private static void Prefix(Game __instance, bool spawnValkyrie)
        {
            if (GUIManager.IsHeadless())
            {
                return;
            }

            if (ConfigRegistry.EnableStartingKits.Value && ((__instance.m_playerProfile != null && __instance.m_playerProfile.m_firstSpawn) || spawnValkyrie))
            {
                StartingKitManager.QueueKitAward();
            }
        }

        [HarmonyPostfix]
        private static void Postfix(Player __result)
        {
            if (GUIManager.IsHeadless())
            {
                return;
            }

            if (__result != null && StartingKitManager.HasPendingKit)
            {
                StartingKitManager.StartAwardCoroutine();
            }
        }
    }

    [HarmonyPatch(typeof(Terminal), "InitTerminal")]
    internal static class TerminalInitTerminalPatch
    {
        [HarmonyPostfix]
        private static void Postfix()
        {
            if (GUIManager.IsHeadless())
            {
                return;
            }

            new Terminal.ConsoleCommand("rspb_resetkit", "Queues starting kit to re-award on local player (Requires devcommands)", delegate(Terminal.ConsoleEventArgs args)
            {
                if (Player.m_localPlayer != null)
                {
                    StartingKitManager.ResetKitQueue();
                    args.Context.AddString("RSPB: Starting kit queued for local player.");
                }
                else
                {
                    args.Context.AddString("RSPB: No active local player.");
                }
            }, isCheat: true);
        }
    }

    [HarmonyPatch(typeof(Humanoid), "GiveDefaultItem")]
    internal static class HumanoidGiveDefaultItemPatch
    {
        private static readonly HashSet<string> VanillaStartingItems = new HashSet<string>
        {
            "ArmorRagsLegs",
            "ArmorRagsChest",
            "Torch"
        };

        [HarmonyPrefix]
        private static bool Prefix(Humanoid __instance, GameObject prefab)
        {
            if (GUIManager.IsHeadless())
            {
                return true;
            }

            if (__instance is Player && ConfigRegistry.EnableStartingKits.Value && ConfigRegistry.ClearVanillaStartingItems.Value)
            {
                if (prefab != null)
                {
                    string cleanName = prefab.name.Replace("(Clone)", "").Trim();
                    if (VanillaStartingItems.Contains(cleanName))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}