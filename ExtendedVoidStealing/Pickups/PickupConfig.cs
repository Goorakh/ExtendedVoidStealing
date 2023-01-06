using BepInEx.Bootstrap;
using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExtendedVoidStealing.Pickups
{
    public static class PickupConfig
    {
        static ConfigEntry<bool> _canRemovePickups;
        public static bool CanRemovePickups
        {
            get
            {
                if (_canRemovePickups == null)
                {
                    Log.Warning($"{nameof(_canRemovePickups)} is null");
                    return false;
                }

                return _canRemovePickups.Value;
            }
        }

        internal static void Init(ConfigFile file)
        {
            _canRemovePickups = file.Bind(new ConfigDefinition("Pickups", "Can Remove Pickups"), true, new ConfigDescription("If void implosions should be able to remove pickups (items, equipment, etc)"));

            if (Chainloader.PluginInfos.ContainsKey(PluginGUIDs.RISK_OF_OPTIONS))
            {
                riskOfOptionsCompat();
            }
        }

        static void riskOfOptionsCompat()
        {
            RiskOfOptions.ModSettingsManager.AddOption(new RiskOfOptions.Options.CheckBoxOption(_canRemovePickups));
        }
    }
}
