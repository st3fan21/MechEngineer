﻿using System.Collections.Generic;
using BattleTech;
using Harmony;

namespace MechEngineer
{
    [HarmonyPatch(typeof(Mech), "OnLocationDestroyed")]
    internal static class Mech_OnLocationDestroyed_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return instructions.MethodReplacer(
                AccessTools.Property(typeof(MechComponent), nameof(MechComponent.Location)).GetGetMethod(),
                AccessTools.Method(typeof(DamageIgnoreHelper), nameof(DamageIgnoreHelper.OverrideLocation))
            );
        }
    }
}