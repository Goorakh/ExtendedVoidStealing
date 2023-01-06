using BepInEx.Bootstrap;
using BepInEx.Configuration;
using HG;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExtendedVoidStealing.Interactables
{
    public static class InteractableConfig
    {
        static ConfigEntry<bool>[] _canRemoveInteractableTypeConfig;

        static ConfigEntry<bool> _canRemoveNewtStatues;
        public static bool CanRemoveNewtStatues
        {
            get
            {
                if (_canRemoveNewtStatues == null)
                {
                    Log.Warning($"{nameof(_canRemoveNewtStatues)} is null");
                    return false;
                }

                return _canRemoveNewtStatues.Value;
            }
        }

        internal static void Init(ConfigFile file)
        {
            _canRemoveInteractableTypeConfig = new ConfigEntry<bool>[(int)InteractableType.Count];
            _canRemoveInteractableTypeConfig[(int)InteractableType.Drone] = file.Bind(new ConfigDefinition("Interactables", "Can Remove Drones"), true, new ConfigDescription("If void implosions should be able to remove unpurchased drones"));
            _canRemoveInteractableTypeConfig[(int)InteractableType.ScavBackpack] = file.Bind(new ConfigDefinition("Interactables", "Can Remove Scavenger Backpacks"), true, new ConfigDescription("If void implosions should be able to remove scavenger backpacks (both lunar and normal variants)"));
            _canRemoveInteractableTypeConfig[(int)InteractableType.Barrel] = file.Bind(new ConfigDefinition("Interactables", "Can Remove Money Barrels"), true, new ConfigDescription("If void implosions should be able to money barrels"));
            _canRemoveInteractableTypeConfig[(int)InteractableType.Chest] = file.Bind(new ConfigDefinition("Interactables", "Can Remove Chests"), true, new ConfigDescription("If void implosions should be able to remove chests"));
            _canRemoveInteractableTypeConfig[(int)InteractableType.Printer] = file.Bind(new ConfigDefinition("Interactables", "Can Remove Printers"), true, new ConfigDescription("If void implosions should be able to remove printers"));
            _canRemoveInteractableTypeConfig[(int)InteractableType.Shrine] = file.Bind(new ConfigDefinition("Interactables", "Can Remove Shrines"), true, new ConfigDescription("If void implosions should be able to remove shrines"));
            _canRemoveInteractableTypeConfig[(int)InteractableType.Scrapper] = file.Bind(new ConfigDefinition("Interactables", "Can Remove Scrappers"), true, new ConfigDescription("If void implosions should be able to remove scrappers"));
            _canRemoveInteractableTypeConfig[(int)InteractableType.MultiShop] = file.Bind(new ConfigDefinition("Interactables", "Can Remove Multishops"), true, new ConfigDescription("If void implosions should be able to remove multishops"));
            _canRemoveInteractableTypeConfig[(int)InteractableType.Lockbox] = file.Bind(new ConfigDefinition("Interactables", "Can Remove Lockboxes"), true, new ConfigDescription("If void implosions should be able to remove lockboxes"));

            _canRemoveNewtStatues = file.Bind(new ConfigDefinition("Interactables", "Can Remove Newt Altars"), true, new ConfigDescription("If void implosions should be able to remove Newt altars"));

            if (Chainloader.PluginInfos.ContainsKey(PluginGUIDs.RISK_OF_OPTIONS))
            {
                riskOfOptionsCompat();
            }
        }

        static void riskOfOptionsCompat()
        {
            foreach (ConfigEntry<bool> canRemoveConfig in _canRemoveInteractableTypeConfig)
            {
                RiskOfOptions.ModSettingsManager.AddOption(new RiskOfOptions.Options.CheckBoxOption(canRemoveConfig));
            }

            RiskOfOptions.ModSettingsManager.AddOption(new RiskOfOptions.Options.CheckBoxOption(_canRemoveNewtStatues));
        }

        public static bool CanRemoveInteractableFromVoidImplosion(InteractableType interactable)
        {
            if (interactable <= InteractableType.Invalid || interactable >= InteractableType.Count)
            {
                return false;
            }

            ConfigEntry<bool> canRemoveConfig = ArrayUtils.GetSafe(_canRemoveInteractableTypeConfig, (int)interactable);
            if (canRemoveConfig == null)
            {
                Log.Warning($"{nameof(CanRemoveInteractableFromVoidImplosion)} null config entry for type {interactable}");
                return false;
            }

            return canRemoveConfig.Value;
        }
    }
}
