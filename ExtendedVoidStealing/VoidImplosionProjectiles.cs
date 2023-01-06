using ExtendedVoidStealing.Interactables;
using ExtendedVoidStealing.Pickups;
using ExtendedVoidStealing.Misc;
using RiskOfOptions.Components.Panel;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace ExtendedVoidStealing
{
    public static class VoidImplosionProjectiles
    {
        static readonly GameObject _voidDeathEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Nullifier/NullifierExplosion.prefab").WaitForCompletion();

        static int[] _voidImplosionProjectileIndices;

        [SystemInitializer(typeof(ProjectileCatalog))]
        static void InitProjectileIndices()
        {
            _voidImplosionProjectileIndices = new string[]
            {
                "NullifierDeathBombProjectile",
                "VoidJailerDeathBombProjectile",
                "VoidJailerDeathBombProjectileStill",
                "VoidMegaCrabDeathBombProjectile"
            }.Select(ProjectileCatalog.FindProjectileIndex).Where(n => n >= 0).OrderBy(n => n).ToArray();

            On.RoR2.Projectile.ProjectileExplosion.DetonateServer += ProjectileExplosion_DetonateServer;
        }

        public static bool IsVoidImplosionProjectile(int projectileIndex)
        {
            if (_voidImplosionProjectileIndices == null)
            {
                Log.Warning($"{nameof(IsVoidImplosionProjectile)} called, but indices array is null");
                return false;
            }

            return Array.BinarySearch(_voidImplosionProjectileIndices, projectileIndex) >= 0;
        }

        static void ProjectileExplosion_DetonateServer(On.RoR2.Projectile.ProjectileExplosion.orig_DetonateServer orig, ProjectileExplosion self)
        {
            if (IsVoidImplosionProjectile(ProjectileCatalog.GetProjectileIndex(self.gameObject)))
            {
                removeAllObjectsInVoidDeathZone(self.transform.position, self.blastRadius);
            }

            orig(self);
        }

        static bool canRemoveMiscNetObject(NetworkIdentity netObj)
        {
            if (netObj.TryGetComponent<PortalStatueBehavior>(out PortalStatueBehavior portalStatueBehavior))
            {
                return portalStatueBehavior.portalType == PortalStatueBehavior.PortalType.Shop && InteractableConfig.CanRemoveNewtStatues;
            }
            else if (netObj.TryGetComponent<SurvivorPodController>(out SurvivorPodController survivorPodController))
            {
                return MiscConfig.CanRemoveSurvivorPods;
            }

#if DEBUG
            Log.Debug("Unhandled net obj " + Util.GetGameObjectHierarchyName(netObj.gameObject));
#endif

            return false;
        }

        static void removeAllObjectsInVoidDeathZone(Vector3 position, float radius)
        {
            float sqrRadius = radius * radius;

            bool isInRange(Vector3 pos)
            {
                return (position - pos).sqrMagnitude <= sqrRadius;
            }

            List<VoidRemovableInteractable> interactables = InstanceTracker.GetInstancesList<VoidRemovableInteractable>();
            for (int i = interactables.Count - 1; i >= 0; i--)
            {
                if (interactables[i].CanRemove && isInRange(interactables[i].transform.position))
                {
                    removeObjectInVoidDeathZone(interactables[i].gameObject);
                }
            }

            if (PickupConfig.CanRemovePickups)
            {
                List<GenericPickupController> pickups = InstanceTracker.GetInstancesList<GenericPickupController>();
                for (int i = pickups.Count - 1; i >= 0; i--)
                {
                    if (isInRange(pickups[i].transform.position))
                    {
                        removeObjectInVoidDeathZone(pickups[i].gameObject);
                    }
                }
            }

            foreach (NetworkIdentity miscNetObject in (from col in Physics.OverlapSphere(position, radius, ~0b0, QueryTriggerInteraction.Collide)
                                                       select col.GetComponentInParent<NetworkIdentity>() into netIdentity
                                                       where netIdentity != null
                                                       select netIdentity).Distinct())
            {
                if (canRemoveMiscNetObject(miscNetObject))
                {
                    removeObjectInVoidDeathZone(miscNetObject.gameObject);
                }
            }
        }

        static void removeObjectInVoidDeathZone(GameObject obj)
        {
            EffectManager.SpawnEffect(_voidDeathEffect, new EffectData
            {
                origin = obj.transform.position,
                scale = 10f
            }, true);

            NetworkServer.Destroy(obj);
        }
    }
}
