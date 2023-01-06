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

        internal static void Init(ConfigFile file)
        {
            _canRemoveSurvivorPods = file.Bind(new ConfigDefinition("Misc", "Remove Survivor Pods"), true, new ConfigDescription("If void implosions should be able to remove survivor pods"));

            if (Chainloader.PluginInfos.ContainsKey(PluginGUIDs.RISK_OF_OPTIONS))
            {
                riskOfOptionsCompat();
            }
        }

        static void riskOfOptionsCompat()
        {
            RiskOfOptions.ModSettingsManager.AddOption(new RiskOfOptions.Options.CheckBoxOption(_canRemoveSurvivorPods));
        }
    }
}
