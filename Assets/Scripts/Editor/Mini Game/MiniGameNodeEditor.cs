#if UNITY_EDITOR

using System;
using Mini_Game;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor
{
    [CustomEditor(typeof(MiniGameNodeObject))]
    public class MiniGameNodeEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var ve = new VisualElement();
            
            InspectorElement.FillDefaultInspector(ve, serializedObject, this);

            var space = new VisualElement();
            space.style.height = new StyleLength(20);
            ve.Add(space);

            ve.Add(MiniGameEditorUtilities.BuildButton("Success", () => (target as MiniGameNodeObject).Success()));
            ve.Add(MiniGameEditorUtilities.BuildButton("Stop Success", () => (target as MiniGameNodeObject).StopHighlightTween()));
            ve.Add(MiniGameEditorUtilities.BuildButton("Notify", () => (target as MiniGameNodeObject).Notify()));
            ve.Add(MiniGameEditorUtilities.BuildButton("Error", () => (target as MiniGameNodeObject).Error()));
            ve.Add(MiniGameEditorUtilities.BuildButton("Show Current", () => (target as MiniGameNodeObject).ShowCurrent()));
            ve.Add(MiniGameEditorUtilities.BuildButton("Show Next", () => (target as MiniGameNodeObject).ShowNext()));
            ve.Add(MiniGameEditorUtilities.BuildButton("Hide Icon", () => (target as MiniGameNodeObject).DisableIcon()));
            
            return ve;
        }

        
    }
}

#endif