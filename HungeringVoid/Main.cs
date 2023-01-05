using BepInEx;
using BepInEx.Bootstrap;
using HungeringVoid.Interactables;
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

namespace HungeringVoid
{
    // To add:
    // Logbook pickups
    // Cauldrons
    // HOLDER: Store/LunarShop/LunarTable/LunarShopTerminal
    // HOLDER: Store/LunarShop/LunarRecycler
    // Lunar Seers
    // Timed Chest
    // All preplaced chests on goldshores
    // Plates on aqueduct
    // Ring event gate on aqueduct
    // PortalDialer stuff on SM

    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(R2API.R2API.PluginGUID)]
    [BepInDependency(PluginGUIDs.RISK_OF_OPTIONS, BepInDependency.DependencyFlags.SoftDependency)]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    public class Main : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Gorakh";
        public const string PluginName = "HungeringVoid";
        public const string PluginVersion = "1.0.0";

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
            MiscConfig.Init(Config);

            stopwatch.Stop();
            Log.Info_NoCallerPrefix($"Initialized in {stopwatch.Elapsed.TotalSeconds:F2} seconds");
        }

        static void riskOfOptionsCompatInit()
        {
            RiskOfOptions.ModSettingsManager.SetModDescription("The void hungers for more...", PluginGUID, "Hungering Void");
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
