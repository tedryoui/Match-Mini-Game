using System;
using UnityEngine.UIElements;

namespace Editor
{
    public static class MiniGameEditorUtilities
    {
        public static Button BuildButton(string name, Action callback)
        {
            var button = new Button();
            button.text = name;
            button.clicked += callback;

            return button;
        }
    }
}