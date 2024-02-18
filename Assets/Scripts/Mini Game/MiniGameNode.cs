using System;
using UnityEngine;

namespace Mini_Game
{
    [Serializable]
    public class MiniGameNode
    {
        public MiniGameNodeObject node;
        public MiniGameNodePreset next;
        public MiniGameNodePreset preset;

        public bool HasActivePreset => preset != null;
        public bool HasNextPreset => next != null;
        public bool HasPreset => preset != null || next != null;

        public void ClearPreset()
        {
            next = null;
            preset = null;
        }
        
        public void SetNextPreset(MiniGameNodePreset value)
        {
            next = value;
            preset = null;
            
            node.SetIcon(next.icon);
        }

        private void SetPreset(MiniGameNodePreset value)
        {
            preset = value;
            
            node.SetIcon(preset.icon);
        }
        
        public void SetPreset()
        {
            preset = next;
            next = null;
            
            node.SetIcon(preset.icon);
        }

        public void Shift(MiniGameNode other)
        {
            other.SetPreset(preset);
            other.node.ShowCurrent();
            
            ClearPreset();
            node.DisableIcon();
        }

        public bool IsSame(MiniGameNode other)
        {
            return preset.Equals(other.preset);
        }
    }
}