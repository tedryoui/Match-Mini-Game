using System;
using UnityEngine;

namespace Mini_Game
{
    [Serializable]
    public class CombinationFabric
    {
        public enum CombinationType { TwoElements }
        
        public static AbstractCombinationStrategy GetCombinationStrategy(MiniGame miniGame, CombinationType type)
        {
            switch (type)
            {
                case CombinationType.TwoElements:
                    return new TwoElementsLineCombinationStrategy(miniGame);
            }

            throw new Exception();
        }
    }
}