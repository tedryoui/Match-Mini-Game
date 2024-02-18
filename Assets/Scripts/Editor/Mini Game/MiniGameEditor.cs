using Mini_Game;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor
{
    [CustomEditor(typeof(MiniGame))]
    public class MiniGameEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var ve = new VisualElement();
            
            InspectorElement.FillDefaultInspector(ve, serializedObject, this);

            var space = new VisualElement();
            space.style.height = new StyleLength(20);
            ve.Add(space);

            ve.Add(MiniGameEditorUtilities.BuildButton("Spawn", () => (target as MiniGame).Spawn()));
            
            return ve;
        }
    }
}