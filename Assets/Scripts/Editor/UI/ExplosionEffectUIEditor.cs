using Mini_Game;
using UI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor.UI
{
    [CustomEditor(typeof(ExplosionEffectUI))]
    public class ExplosionEffectUIEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var ve = new VisualElement();
            
            InspectorElement.FillDefaultInspector(ve, serializedObject, this);

            var space = new VisualElement();
            space.style.height = new StyleLength(20);
            ve.Add(space);

            ve.Add(MiniGameEditorUtilities.BuildButton("Explode", () => (target as ExplosionEffectUI).Explode()));
            
            return ve;
        }
    }
}