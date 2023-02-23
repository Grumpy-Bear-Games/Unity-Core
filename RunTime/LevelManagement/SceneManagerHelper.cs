using UnityEngine;

namespace Games.GrumpyBear.Core.LevelManagement
{
    internal class SceneManagerHelper : MonoBehaviour
    {
        private static SceneManagerHelper _instance;
        public static SceneManagerHelper Instance {
            get
            {
                if (_instance != null) return _instance;
                
                var go = new GameObject("[SceneManager Helper]", typeof(SceneManagerHelper))
                {
                    hideFlags = HideFlags.DontSave | HideFlags.NotEditable | HideFlags.HideAndDontSave | HideFlags.HideInHierarchy
                };
                DontDestroyOnLoad(go);
                _instance = go.GetComponent<SceneManagerHelper>();

                return _instance;
            }
        }
    }
}
