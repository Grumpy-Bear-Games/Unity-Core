using System.Collections;
using Games.GrumpyBear.Core.LevelManagement;
using UnityEngine;

public class LoadSpy : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log($"[{Time.frameCount}] {gameObject.name}.Awake()");
    }

    private void OnEnable()
    {
        Debug.Log($"[{Time.frameCount}] {gameObject.name}.OnEnable()");
    }

    private IEnumerator Start()
    {
        Debug.Log($"[{Time.frameCount}] {gameObject.name}.Start()");
        yield return SceneManager.WaitForLoadingCompleted();
        Debug.Log($"[{Time.frameCount}] SceneGroup finished loading");
    }
}
