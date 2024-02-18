using System.Collections.Generic;

namespace Mini_Game
{
    public class TwoElementsLineCombinationStrategy : AbstractCombinationStrategy
    {
        public TwoElementsLineCombinationStrategy(MiniGame miniGame) : base(miniGame)
        {
        }
        
        private MiniGameNode target;
        private int x;
        private int y;

        private int _requiredStreak = 2;

        public override void Combine(MiniGameNode node)
        {
            var indices = _miniGame.GetMiniGameNodeIndices(node);

            target = node;
            x = indices.x;
            y = indices.y;

            CheckHorizontalStreak();
            CheckVerticalStreak();
        }

        private void CheckVerticalStreak()
        {
            List<MiniGameNode> cache = new List<MiniGameNode>();
            int streak = 0;
            
            for (int i = y - 1; i <= y + 1; i++)
            {
                var node = _miniGame.GetMiniGameNode(x, i);
                
                if (node == null) continue;

                if (node.HasActivePreset && target.HasActivePreset && target.IsSame(node))
                {
                    streak++;
                    cache.Add(node);
                }
                else
                {
                    if (streak == _requiredStreak)
                    {
                        ClearListOfNodes(cache);
                        return;
                    }

                    streak = 0;
                }
            }
            
            if (streak == _requiredStreak) ClearListOfNodes(cache);
        }

        private void CheckHorizontalStreak()
        {
            List<MiniGameNode> cache = new List<MiniGameNode>();
            int streak = 0;
            
            for (int i = x - 1; i <= x + 1; i++)
            {
                var node = _miniGame.GetMiniGameNode(i, y);
                
                if (node == null) continue;

                if (node.HasActivePreset && target.HasActivePreset && target.IsSame(node))
                {
                    streak++;
                    cache.Add(node);
                }
                else
                {
                    if (streak == _requiredStreak)
                    {
                        ClearListOfNodes(cache);
                        return;
                    }

                    streak = 0;
                }
            }
            
            if (streak == _requiredStreak) ClearListOfNodes(cache);
        }

    }
}