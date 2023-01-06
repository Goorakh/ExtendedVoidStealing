using BepInEx.Bootstrap;
using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExtendedVoidStealing.Misc
{
    public static class MiscConfig
    {
        static ConfigEntry<bool> _canRemoveSurvivorPods;
        public static bool CanRemoveSurvivorPods
        {
            get
            {
                if (_canRemoveSurvivorPods == null)
                {
                    Log.Warning($"{nameof(_canRemoveSurvivorPods)} is null");
                    return false;
                }

                return _canRemoveSurvivorPods.Value;
            }
        }

        static ConfigEntry<bool> _canRemoveAqueductPlates;
        public static bool CanRemoveAqueductPlates
        {
            get
            {
                if (_canRemoveAqueductPlates == null)
                {
                    Log.Warning($"{nameof(_canRemoveAqueductPlates)} is null");
                    return false;
                }

                return _canRemoveAqueductPlates.Value;
            }
        }

        internal static void Init(ConfigFile file)
        {
            _canRemoveSurvivorPods = file.Bind(new ConfigDefinition("Misc", "Remove Survivor Pods"), true, new ConfigDescription("If void implosions should be able to remove survivor pods"));
            _canRemoveAqueductPlates = file.Bind(new ConfigDefinition("Misc", "Remove Abandoned Aqueduct Plates"), true, new ConfigDescription("If void implosions should be able to remove the pressure plates on Abandoned Aqueduct (if this happens, the gate can no longer be opened)"));

            if (Chainloader.PluginInfos.ContainsKey(PluginGUIDs.RISK_OF_OPTIONS))
            {
                riskOfOptionsCompat();
            }
        }

        static void riskOfOptionsCompat()
        {
            RiskOfOptions.ModSettingsManager.AddOption(new RiskOfOptions.Options.CheckBoxOption(_canRemoveSurvivorPods));
            RiskOfOptions.ModSettingsManager.AddOption(new RiskOfOptions.Options.CheckBoxOption(_canRemoveAqueductPlates));
        }
    }
}
