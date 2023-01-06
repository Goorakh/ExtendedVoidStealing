using RoR2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ExtendedVoidStealing.SceneObjectMarkers
{
    public class AbandonedAqueductPlate : MonoBehaviour
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

            if (SceneCache.AbandonedAqueductSceneIndex == SceneIndex.Invalid || scene.sceneDefIndex != SceneCache.AbandonedAqueductSceneIndex)
                return;

            const string ENTRANCE_OBJ_PATH = "HOLDER: Secret Ring Area Content/Entrance";
            GameObject entrance = GameObject.Find(ENTRANCE_OBJ_PATH);
            if (!entrance)
            {
                Log.Warning($"unable to find entrance object ({ENTRANCE_OBJ_PATH})");
                return;
            }

            Transform entranceTransform = entrance.transform;
            for (int i = 0; i < entranceTransform.childCount; i++)
            {
                Transform childTransform = entranceTransform.GetChild(i);
                if (!childTransform)
                    continue;

                GameObject childObj = childTransform.gameObject;
                if (childObj.name.StartsWith("GLPressurePlate"))
                {
                    childObj.AddComponent<AbandonedAqueductPlate>();

#if DEBUG
                    Log.Debug($"Added {nameof(AbandonedAqueductPlate)} to {Util.GetGameObjectHierarchyName(childObj)}");
#endif
                }
            }
        }
    }
}
