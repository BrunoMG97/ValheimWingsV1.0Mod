using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using System.Reflection;

namespace ValheimWings
{
    [BepInPlugin("com.github.valheimwings.plugin", "ValheimWings", "1.0.0")]
    public class ValheimWingsPlugin : BaseUnityPlugin
    {
        public static ValheimWingsPlugin Instance;
        
        // Cachear campos por reflexión para optimización
        public static FieldInfo AnimatorField;
        public static FieldInfo StaminaField;
        public static FieldInfo RunSpeedField;

        void Awake()
        {
            Instance = this;
            
            // Cachear campos
            AnimatorField = typeof(Character).GetField("m_animator", BindingFlags.NonPublic | BindingFlags.Instance);
            StaminaField = typeof(Player).GetField("m_stamina", BindingFlags.NonPublic | BindingFlags.Instance);
            RunSpeedField = typeof(Player).GetField("m_runSpeed", BindingFlags.NonPublic | BindingFlags.Instance);

            Harmony.CreateAndPatchAll(typeof(PlayerPatch));
            Logger.LogInfo("ValheimWings initialized with optimized caching.");
        }
    }
}
