﻿using BattleTech.UI;
using Harmony;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace MechEngineer
{
    [HarmonyPatch(typeof(MechLabPanel), "LoadMech")]
    public static class MechLabPanel_LoadMech_Patch
    {
        public static void Postfix(MechLabPanel __instance)
        {
            try
            {
                var mechLabPanel = __instance;
                var Representation = mechLabPanel.transform.GetChild("Representation");
                var OBJ_mech = Representation.GetChild("OBJ_mech");

                var mechRectTransform = OBJ_mech.GetComponent<RectTransform>();
                // Unity (?) does not handle layout propagation properly, so we need to force several layout passes here
                // also allows us to calculate stuff for auto zoom without waiting for regular layout passes
                LayoutRebuilder.ForceRebuildLayoutImmediate(mechRectTransform);
                LayoutRebuilder.ForceRebuildLayoutImmediate(mechRectTransform);
                LayoutRebuilder.ForceRebuildLayoutImmediate(mechRectTransform);

                mechRectTransform.anchorMin = new Vector2(mechRectTransform.anchorMin.x, 1);
                mechRectTransform.anchorMax = new Vector2(mechRectTransform.anchorMin.x, 1);
                mechRectTransform.pivot = new Vector2(mechRectTransform.pivot.x, 1);
                
                var OBJ_actions = Representation.GetChild("OBJ_actions");
                mechRectTransform.position = new Vector3(OBJ_mech.position.x, OBJ_actions.position.y, OBJ_mech.position.z);

                {
                    var OBJ_cancelconfirm = Representation.GetChild("OBJ_cancelconfirm");
                    var confirmRectTransform = OBJ_cancelconfirm.GetComponent<RectTransform>();

                    var mechSize = mechRectTransform.sizeDelta.y;
                    var targetSize = mechRectTransform.localPosition.y
                                     - 40 // repair button height
                                     - confirmRectTransform.localPosition.y + confirmRectTransform.sizeDelta.y; // save button bottom

                    var scale = Mathf.Min(1, targetSize / mechSize);
                    mechRectTransform.localScale = new Vector3(scale, scale, 1);
                
                    Control.mod.Logger.LogDebug($"AutoZoom scale={scale} mechSize={mechSize} targetSize={targetSize}");
                }
            }
            catch (Exception e)
            {
                Control.mod.Logger.LogError(e);
            }
        }
    }
}
