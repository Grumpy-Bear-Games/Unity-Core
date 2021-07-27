using Games.GrumpyBear.Core.Events;
using UnityEditor;
using UnityEngine;

namespace Games.GrumpyBear.Core.Editor.Events
{
    [CustomEditor(typeof(VoidEvent))]
    public class VoidEventEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!Application.isPlaying) return;
            GUILayout.Space(20);
            if (!GUILayout.Button("Trigger")) return;
            if (target is VoidEvent voidEvent) voidEvent.Invoke();
        }
    }
}
