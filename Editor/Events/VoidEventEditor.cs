using Games.GrumpyBear.Core.Events;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Games.GrumpyBear.Core.Editor.Events
{
    [CustomEditor(typeof(VoidEvent))]
    public class VoidEventEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            var triggerButton = new Button
            {
                text = "Trigger event"
            };
            triggerButton.clicked += () =>
            {
                if (target is VoidEvent voidEvent) voidEvent.Invoke();
            };
            
            triggerButton.SetEnabled(EditorApplication.isPlayingOrWillChangePlaymode);
            root.Add(triggerButton);

            return root;
        }
    }
}
