using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ExtendedVoidStealing.SceneObjectMarkers
{
    public class RallyPointFan : MonoBehaviour
    {
        [SystemInitializer]
        static void Init()
        {
            SceneCatalog.onMostRecentSceneDefChanged += onSceneChanged;
        }

        static void onSceneChanged(SceneDef scene)
        {
            if (!scene)
                return;

            if (SceneCache.RallyPointSceneIndex == SceneIndex.Invalid || scene.sceneDefIndex != SceneCache.RallyPointSceneIndex)
                return;

            const string FANS_HOLDER = "PERMUTATION: Human Fan";
            GameObject fansHolder = GameObject.Find(FANS_HOLDER);
            if (!fansHolder)
            {
                Log.Warning($"unable to find fan holder object ({FANS_HOLDER})");
                return;
            }

            Transform fansHolderTransform = fansHolder.transform;
            for (int i = 0; i < fansHolderTransform.childCount; i++)
            {
                Transform childTransform = fansHolderTransform.GetChild(i);
                if (!childTransform)
                    continue;

                GameObject childObj = childTransform.gameObject;
                if (childObj.name.StartsWith("HumanFan"))
                {
                    childObj.AddComponent<RallyPointFan>();

#if DEBUG
                    Log.Debug($"Added {nameof(RallyPointFan)} to {Util.GetGameObjectHierarchyName(childObj)}");
#endif
                }
            }
        }
    }
}
