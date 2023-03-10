using BepInEx;
using BepInEx.Bootstrap;
using ExtendedVoidStealing.Interactables;
using ExtendedVoidStealing.Pickups;
using ExtendedVoidStealing.Misc;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.Projectile;
using System;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace ExtendedVoidStealing
{
    // To add:
    // Logbook pickups
    // Cauldrons
    // HOLDER: Store/LunarShop/LunarTable/LunarShopTerminal
    // HOLDER: Store/LunarShop/LunarRecycler
    // Lunar Seers
    // All preplaced chests on goldshores
    // PortalDialer stuff on SM

    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(R2API.R2API.PluginGUID)]
    [BepInDependency(PluginGUIDs.RISK_OF_OPTIONS, BepInDependency.DependencyFlags.SoftDependency)]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    public class Main : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Gorakh";
        public const string PluginName = "ExtendedVoidStealing";
        public const string PluginVersion = "1.1.1";

        void Awake()
        {
            Log.Init(Logger);

            Stopwatch stopwatch = Stopwatch.StartNew();

            VoidRemovableInteractable.AddToAllValidInteractables();

            if (Chainloader.PluginInfos.ContainsKey(PluginGUIDs.RISK_OF_OPTIONS))
            {
                riskOfOptionsCompatInit();
            }

            InteractableConfig.Init(Config);
            PickupConfig.Init(Config);
            MiscConfig.Init(Config);

            stopwatch.Stop();
            Log.Info_NoCallerPrefix($"Initialized in {stopwatch.Elapsed.TotalSeconds:F2} seconds");
        }

        static void riskOfOptionsCompatInit()
        {
            RiskOfOptions.ModSettingsManager.SetModDescription(string.Empty, PluginGUID, "Extended Void Stealing");
        }

#if DEBUG
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                foreach (NetworkIdentity netIdentity in FindObjectsOfType<NetworkIdentity>())
                {
                    if (Util.IsDontDestroyOnLoad(netIdentity.gameObject))
                        continue;

                    Log.Debug(Util.GetGameObjectHierarchyName(netIdentity.gameObject));
                }
            }
        }
#endif
    }
}
