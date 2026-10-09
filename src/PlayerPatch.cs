using HarmonyLib;
using UnityEngine;

namespace ValheimWings
{
    [HarmonyPatch(typeof(Player))]
    public static class PlayerPatch
    {
        [HarmonyPatch("FixedUpdate")]
        [HarmonyPrefix]
        static void Prefix(Player __instance)
        {
            if (__instance != Player.m_localPlayer) return;

            if (UnityEngine.Input.GetKeyDown(KeyCode.N))
            {
                var fm = __instance.GetComponent<FlightManager>();
                if (fm == null) __instance.gameObject.AddComponent<FlightManager>();
                else fm.ToggleFlight();
            }
        }
    }
}
