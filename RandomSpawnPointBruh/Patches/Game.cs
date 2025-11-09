using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using HarmonyLib;
using JetBrains.Annotations;
using RandomSpawnPointBruh.Components;
using RandomSpawnPointBruh.Configuration;

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
            var result = new List<MethodInfo>();
            
            result.Add(AccessTools.DeclaredMethod(typeof(Game), nameof(Game.FindSpawnPoint)));

            return result;
        }
        
        [UsedImplicitly]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var patchedSuccess = false;
            var patchedSuccess2 = false;
            var instrs = instructions.ToList();

            var counter = 0;

            CodeInstruction LogMessage(CodeInstruction instruction)
            {
                RandomSpawnPointBruh.Log.Debug(
                    $"IL_{counter}: Opcode: {instruction.opcode} Operand: {instruction.operand}");
                return instruction;
            }

            var zoneSystemInstance = AccessTools.DeclaredPropertyGetter(typeof(ZoneSystem), nameof(ZoneSystem.instance));
            var getLocationMethod = AccessTools.DeclaredMethod(typeof(ZoneSystem), nameof(ZoneSystem.GetLocationIcon));
            var startLocationField = AccessTools.DeclaredField(typeof(Game), nameof(Game.m_StartLocation));

            for (int i = 0; i < instrs.Count; ++i)
            {
                if (!RandomSpawnPointBruh.Main.HasCompetingMods && i > 5 && instrs[i].opcode == OpCodes.Callvirt &&
                    instrs[i].operand.Equals(getLocationMethod) && instrs[i - 2].opcode == OpCodes.Ldfld &&
                    instrs[i - 2].operand.Equals(startLocationField))
                {
                    //Patch Calling Method
                    yield return LogMessage(new CodeInstruction(OpCodes.Call,
                        AccessTools.DeclaredMethod(typeof(SpawnPointGenerator),
                            nameof(SpawnPointGenerator.GetSpawnPoint))));
                    counter++;

                    patchedSuccess = true;
                } else if (RandomSpawnPointBruh.Main.HasCompetingMods && i > 5 && instrs[i].opcode == OpCodes.Callvirt 
                           && instrs[i-2].opcode == OpCodes.Ldfld && instrs[i-2].operand.Equals(startLocationField))
                {
                    //Patch Calling Method
                    yield return LogMessage(new CodeInstruction(OpCodes.Call,
                        AccessTools.DeclaredMethod(typeof(SpawnPointGenerator),
                            nameof(SpawnPointGenerator.GetSpawnPoint))));
                    counter++;

                    patchedSuccess = true;
                } else if (!RandomSpawnPointBruh.Main.HasCompetingMods && i > 5 && instrs[i].opcode == OpCodes.Call && instrs[i].operand.Equals(zoneSystemInstance)
                           && instrs[i+4].opcode == OpCodes.Callvirt && instrs[i+4].operand.Equals(getLocationMethod)
                           && instrs[i+2].opcode == OpCodes.Ldfld && instrs[i+2].operand.Equals(startLocationField))
                {
                    //Move Labels.
                    //Move Any Labels from the instruction position being patched to new instruction.
                    if (instrs[i].labels.Count > 0)
                        instrs[i].MoveLabelsTo(instrs[i+1]);

                    patchedSuccess2 = true;
                } else if (RandomSpawnPointBruh.Main.HasCompetingMods && i > 5 && instrs[i].opcode == OpCodes.Call && instrs[i].operand.Equals(zoneSystemInstance)
                           && instrs[i+4].opcode == OpCodes.Callvirt
                           && instrs[i+2].opcode == OpCodes.Ldfld && instrs[i+2].operand.Equals(startLocationField))
                {
                    //Move Labels.
                    //Move Any Labels from the instruction position being patched to new instruction.
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
}