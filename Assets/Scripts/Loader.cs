using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    [SerializeField] private List<string> _scenes = new List<string>();
    
    public void Load()
    {
        StartCoroutine(Load_CO());
    }

    private IEnumerator Load_CO()
    {
        var ops = new List<SceneOp>();

        SceneManager.sceneLoaded += LogSceneLoaded;

        foreach (var scene in _scenes)
        {
            Debug.Log($"[{Time.frameCount}] Loading {scene}");
            var op = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            op.completed += _ =>
            {
                Debug.Log($"[{Time.frameCount}] {scene} completed");
                Debug.Log(FindObjectOfType(typeof(LoadSpy), true));
            };
            op.allowSceneActivation = false;
            ops.Add(new SceneOp(scene, op));
        }

        foreach (var sceneOp in ops)
        {
            while (!sceneOp.op.isDone)
            {
                if (sceneOp.op.progress >= 0.9f)
                {
                    Debug.Log($"[{Time.frameCount}] {sceneOp.scene} Ready for activation");
                    sceneOp.op.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }

    private class SceneOp
    {
        public string scene;
        public AsyncOperation op;

        public SceneOp(string scene, AsyncOperation op)
        {
            this.scene = scene;
            this.op = op;
        }
    }

    private static void LogSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[{Time.frameCount}] {scene.name} loaded callback");
        Debug.Log(FindObjectOfType(typeof(LoadSpy), true));
    }
}
