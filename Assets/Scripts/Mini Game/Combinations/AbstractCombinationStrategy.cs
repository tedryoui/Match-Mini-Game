using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace Mini_Game
{
    public abstract class AbstractCombinationStrategy
    {
        protected MiniGame _miniGame;

        public UnityEvent<int> cleared;
        
        protected AbstractCombinationStrategy(MiniGame miniGame)
        {
            _miniGame = miniGame;

            cleared = new UnityEvent<int>();
        }

        public abstract void Combine(MiniGameNode node);

        protected void ClearListOfNodes(List<MiniGameNode> nodes)
        {
            foreach (var node in nodes)
            {
                node.ClearPreset();
                
                AsyncDisableNodeTween(node);
            }
            
            cleared?.Invoke(nodes.Count);
        }

        private async void AsyncDisableNodeTween(MiniGameNode node)
        {
            await Task.Yield();
            
            node.node.StopIconTween();
            node.node.ShowExplosion();
        }
    }
}