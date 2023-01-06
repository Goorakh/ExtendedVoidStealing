using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace ExtendedVoidStealing.Interactables
{
    public class VoidRemovableInteractable : MonoBehaviour
    {
        InteractableType _type;
        
        public bool CanRemove => InteractableConfig.CanRemoveInteractableFromVoidImplosion(_type);

        void OnEnable()
        {
            InstanceTracker.Add(this);
        }

        void OnDisable()
        {
            InstanceTracker.Remove(this);
        }

        internal static void AddToAllValidInteractables()
        {
            static VoidRemovableInteractable addTo_isc(string assetPath, InteractableType type)
            {
                InteractableSpawnCard isc = Addressables.LoadAssetAsync<InteractableSpawnCard>(assetPath).WaitForCompletion();
                if (!isc)
                {
                    Log.Warning($"null isc at path {assetPath}");
                    return null;
                }

                if (!isc.prefab)
                {
                    Log.Warning($"null isc prefab at path {assetPath}");
                    return null;
                }

                if (!isc.prefab.GetComponent<NetworkIdentity>())
                {
                    Log.Warning($"isc prefab at path {assetPath} does not have a net identity");
                }

                if (isc.prefab.TryGetComponent(out VoidRemovableInteractable voidRemovableInteractable))
                {
                    Log.Warning($"tried to add duplicate component for {assetPath}");
                    return voidRemovableInteractable;
                }

                voidRemovableInteractable = isc.prefab.AddComponent<VoidRemovableInteractable>();
                voidRemovableInteractable._type = type;
                return voidRemovableInteractable;
            }

            addTo_isc("RoR2/Base/Drones/iscBrokenDrone1.asset", InteractableType.Drone);
            addTo_isc("RoR2/Base/Drones/iscBrokenDrone2.asset", InteractableType.Drone);
            addTo_isc("RoR2/Base/Drones/iscBrokenEmergencyDrone.asset", InteractableType.Drone);
            addTo_isc("RoR2/Base/Drones/iscBrokenEquipmentDrone.asset", InteractableType.Drone);
            addTo_isc("RoR2/Base/Drones/iscBrokenFlameDrone.asset", InteractableType.Drone);
            addTo_isc("RoR2/Base/Drones/iscBrokenMegaDrone.asset", InteractableType.Drone);
            addTo_isc("RoR2/Base/Drones/iscBrokenMissileDrone.asset", InteractableType.Drone);
            addTo_isc("RoR2/Base/Drones/iscBrokenTurret1.asset", InteractableType.Drone);
            addTo_isc("RoR2/Base/Scav/iscScavBackpack.asset", InteractableType.ScavBackpack);
            addTo_isc("RoR2/Base/Scav/iscScavLunarBackpack.asset", InteractableType.ScavBackpack);
            addTo_isc("RoR2/Base/Barrel1/iscBarrel1.asset", InteractableType.Barrel);
            addTo_isc("RoR2/Base/CasinoChest/iscCasinoChest.asset", InteractableType.Chest);
            addTo_isc("RoR2/Base/CategoryChest/iscCategoryChestDamage.asset", InteractableType.Chest);
            addTo_isc("RoR2/Base/CategoryChest/iscCategoryChestHealing.asset", InteractableType.Chest);
            addTo_isc("RoR2/Base/CategoryChest/iscCategoryChestUtility.asset", InteractableType.Chest);
            addTo_isc("RoR2/Base/Chest1/iscChest1.asset", InteractableType.Chest);
            addTo_isc("RoR2/Base/Chest1StealthedVariant/iscChest1Stealthed.asset", InteractableType.Chest);
            addTo_isc("RoR2/Base/Chest2/iscChest2.asset", InteractableType.Chest);
            addTo_isc("RoR2/Base/Duplicator/iscDuplicator.asset", InteractableType.Printer);
            addTo_isc("RoR2/Base/DuplicatorLarge/iscDuplicatorLarge.asset", InteractableType.Printer);
            addTo_isc("RoR2/Base/DuplicatorMilitary/iscDuplicatorMilitary.asset", InteractableType.Printer);
            addTo_isc("RoR2/Base/DuplicatorWild/iscDuplicatorWild.asset", InteractableType.Printer);
            addTo_isc("RoR2/Base/EquipmentBarrel/iscEquipmentBarrel.asset", InteractableType.Chest);
            addTo_isc("RoR2/Base/GoldChest/iscGoldChest.asset", InteractableType.Chest);
            addTo_isc("RoR2/Base/LunarChest/iscLunarChest.asset", InteractableType.Chest);
            addTo_isc("RoR2/Base/RadarTower/iscRadarTower.asset", InteractableType.Shrine);
            addTo_isc("RoR2/Base/Scrapper/iscScrapper.asset", InteractableType.Scrapper);
            addTo_isc("RoR2/Base/ShrineBlood/iscShrineBlood.asset", InteractableType.Shrine);
            addTo_isc("RoR2/Base/ShrineBlood/iscShrineBloodSandy.asset", InteractableType.Shrine);
            addTo_isc("RoR2/Base/ShrineBlood/iscShrineBloodSnowy.asset", InteractableType.Shrine);
            addTo_isc("RoR2/Base/ShrineBoss/iscShrineBoss.asset", InteractableType.Shrine);
            addTo_isc("RoR2/Base/ShrineBoss/iscShrineBossSandy.asset", InteractableType.Shrine);
            addTo_isc("RoR2/Base/ShrineBoss/iscShrineBossSnowy.asset", InteractableType.Shrine);
            addTo_isc("RoR2/Base/ShrineChance/iscShrineChance.asset", InteractableType.Shrine);
            addTo_isc("RoR2/Base/ShrineChance/iscShrineChanceSandy.asset", InteractableType.Shrine);
            addTo_isc("RoR2/Base/ShrineChance/iscShrineChanceSnowy.asset", InteractableType.Shrine);
            addTo_isc("RoR2/Base/ShrineCleanse/iscShrineCleanse.asset", InteractableType.Shrine);
            addTo_isc("RoR2/Base/ShrineCleanse/iscShrineCleanseSandy.asset", InteractableType.Shrine);
            addTo_isc("RoR2/Base/ShrineCleanse/iscShrineCleanseSnowy.asset", InteractableType.Shrine);
            addTo_isc("RoR2/Base/ShrineCombat/iscShrineCombat.asset", InteractableType.Shrine);
            addTo_isc("RoR2/Base/ShrineCombat/iscShrineCombatSandy.asset", InteractableType.Shrine);
            addTo_isc("RoR2/Base/ShrineCombat/iscShrineCombatSnowy.asset", InteractableType.Shrine);
            addTo_isc("RoR2/Base/ShrineGoldshoresAccess/iscShrineGoldshoresAccess.asset", InteractableType.Shrine);
            addTo_isc("RoR2/Base/ShrineHealing/iscShrineHealing.asset", InteractableType.Shrine);
            addTo_isc("RoR2/Base/ShrineRestack/iscShrineRestack.asset", InteractableType.Shrine);
            addTo_isc("RoR2/Base/ShrineRestack/iscShrineRestackSandy.asset", InteractableType.Shrine);
            addTo_isc("RoR2/Base/ShrineRestack/iscShrineRestackSnowy.asset", InteractableType.Shrine);
            addTo_isc("RoR2/Base/TripleShop/iscTripleShop.asset", InteractableType.MultiShop);
            addTo_isc("RoR2/Base/TripleShopEquipment/iscTripleShopEquipment.asset", InteractableType.MultiShop);
            addTo_isc("RoR2/Base/TripleShopLarge/iscTripleShopLarge.asset", InteractableType.MultiShop);
            addTo_isc("RoR2/DLC1/CategoryChest2/iscCategoryChest2Damage.asset", InteractableType.Chest);
            addTo_isc("RoR2/DLC1/CategoryChest2/iscCategoryChest2Healing.asset", InteractableType.Chest);
            addTo_isc("RoR2/DLC1/CategoryChest2/iscCategoryChest2Utility.asset", InteractableType.Chest);
            addTo_isc("RoR2/DLC1/VoidChest/iscVoidChest.asset", InteractableType.Chest);
            addTo_isc("RoR2/DLC1/VoidCoinBarrel/iscVoidCoinBarrel.asset", InteractableType.Barrel);
            addTo_isc("RoR2/DLC1/VoidSuppressor/iscVoidSuppressor.asset", InteractableType.Shrine);
            addTo_isc("RoR2/DLC1/VoidTriple/iscVoidTriple.asset", InteractableType.MultiShop);
            addTo_isc("RoR2/DLC1/FreeChest/iscFreeChest.asset", InteractableType.MultiShop);
            addTo_isc("RoR2/DLC1/TreasureCacheVoid/iscLockboxVoid.asset", InteractableType.Lockbox);
            addTo_isc("RoR2/Junk/TreasureCache/iscLockbox.asset", InteractableType.Lockbox);
        }
    }
}
