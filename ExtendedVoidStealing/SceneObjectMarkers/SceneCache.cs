using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExtendedVoidStealing.SceneObjectMarkers
{
    public static class SceneCache
    {
        static SceneIndex _abandonedAqueductSceneIndex;
        public static SceneIndex AbandonedAqueductSceneIndex => _abandonedAqueductSceneIndex;

        [SystemInitializer(typeof(SceneCatalog))]
        static void Init()
        {
            static void setSceneIndexField(string sceneName, out SceneIndex field)
            {
                if ((field = SceneCatalog.FindSceneIndex(sceneName)) == SceneIndex.Invalid)
                {
                    Log.Warning($"Could not find scene index for {sceneName}");
                }
            }

            setSceneIndexField("goolake", out _abandonedAqueductSceneIndex);
        }
    }
}
