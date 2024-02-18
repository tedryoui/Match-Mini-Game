using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using DG.Tweening;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Mini_Game
{
    public class MiniGame : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private MiniGameReward _miniGameReward;
        [SerializeField] private GridNodeSpawner _gridSpawner;

        [Header("Mini Game Settings")] 
        [SerializeField] private CombinationFabric.CombinationType _combinationType;
        [SerializeField] private int _spawnCount;
        [SerializeField] private MiniGameNodePreset[] _presets;

        [Header("Grid Settings")] 
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private Vector2 _nodeSizes;
        [SerializeField] private float _betweenNodesSpace;

        [Header("Tween Settings")] 
        [SerializeField] private float _showDelay;

        [Header("Callbacks")] 
        [SerializeField, Space(10)] private UnityEvent<int> _cleared;
        [SerializeField, Space(10)] private UnityEvent<int> _scoreChanged;
        [SerializeField, Space(10)] private UnityEvent _gameOver;

        private AbstractCombinationStrategy _combinationStrategy;
        private MiniGameNode _activeNode;
        private MiniGameNode[,] _nodes;
        private (MiniGameNode node, MiniGameNodePreset preset)[] _nodesToSpawn;

        private int _score;

        #region Initialization

        private void InitializeGridSpawnerParameters()
        {
            // Set grid spawner settings
            _gridSpawner.SetWidthAndHeight(_width, _height);
            _gridSpawner.SetSizesAndSpaces(_nodeSizes, _betweenNodesSpace);
            
            // Invoke creating grid nodes
            _gridSpawner.SpawnNodes();
        }

        private void RebuildNodeObjectCallbacks()
        {
            _activeNode = null;
            
            foreach (var node in _nodes)
                node.node.clicked.AddListener(() => OnNodeClicked(node));
        }

        private void InitializeNodesArray()
        {
            // Get every MiniGameNode in grid spawner object
            var children = _gridSpawner.transform.GetComponentsInChildren<MiniGameNodeObject>();
            
            // Create 2D array of MiniGameNodes
            _nodes = new MiniGameNode[_width, _height];
            
            for (var i = 0; i < children.Length; i++)
            {
                var localIndex = i;
                var child = children[i];

                // Convert localIndex to x, y indices
                int x = localIndex % _width;
                int y = localIndex / _width;

                // Write node in x, y indices of 2D array
                _nodes[x, y] = new MiniGameNode()
                {
                    node = child,
                    preset = null
                };
            }
        }

        #endregion

        #region Unity Events

        private void Start()
        {
            StartGame();
        }

        #endregion
        
        public void StartGame()
        {
            _score = 0;
            _nodesToSpawn = null;
            
            _combinationStrategy = CombinationFabric.GetCombinationStrategy(this, _combinationType);
            _combinationStrategy.cleared.AddListener(AddScore);
            _combinationStrategy.cleared.AddListener((c) => _cleared.Invoke(c));
            
            InitializeGridSpawnerParameters();

            InitializeNodesArray();
            
            RebuildNodeObjectCallbacks();
            
            Spawn();
        }
        
        private void GameOver()
        {
            if (_nodesToSpawn.Length == 0 || !HasAnyNullNodes())
            {
                _miniGameReward.Open(null, _score.ToString());
                
                _gameOver?.Invoke();
            }
        }

        private void AddScore(int count)
        {
            _score += count * 3;
            
            _scoreChanged?.Invoke(_score);
        }
        
        #region Nodes

        private void OnNodeClicked(MiniGameNode node)
        {
            if (_activeNode == null)
                SelectNode(node);
            else if (_activeNode.Equals(node))
                DeselectActiveNode();
            else
                TriggerNodeAction(node);
        }

        private void TriggerNodeAction(MiniGameNode node)
        {
            if (node.HasPreset)
            {
                node.node.Notify();
            }
            else
            {
                _activeNode.Shift(node);
                
                DeselectActiveNode();
                
                Spawn();
            }
        }

        private void DeselectActiveNode()
        {
            _activeNode.node.StopHighlightTween();

            _activeNode = null;
        }

        private void SelectNode(MiniGameNode node)
        {
            if (node.HasActivePreset)
            {
                _activeNode = node;

                _activeNode.node.Success();
            }
            else
            {
                node.node.Notify();
            }
        }

        public MiniGameNode GetMiniGameNode(int x, int y)
        {
            if (x >= 0 && x < _width && y >= 0 && y < _height)
                return _nodes[x, y];

            return null;
        }

        public (int x, int y) GetMiniGameNodeIndices(MiniGameNode node)
        {
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    if (node.Equals(_nodes[i, j])) return (i, j);
                }
            }

            throw new Exception("There is no such node in the nodes array");
        }

        private bool HasSelectableNodes()
        {
            foreach (var node in _nodes)
                if (node.HasActivePreset)
                    return true;

            return false;
        }

        public void Combine()
        {
            foreach (var node in _nodes)
                if (node.HasPreset) _combinationStrategy.Combine(node);
            
            if (!HasSelectableNodes()) Spawn();
        }
        
        private bool HasAnyNullNodes()
        {
            foreach (var node in _nodes)
                if (!node.HasActivePreset && !node.HasNextPreset) return true;

            return false;
        }
        
        #endregion

        #region Spawning

        public void Spawn()
        {
            SpawnNext();
            PrepareNext();
            Combine();
            GameOver();
        }

        private void SpawnNext()
        {
            if (_nodesToSpawn == null) return;

            for (var i = 0; i < _nodesToSpawn.Length; i++)
            {
                var value = _nodesToSpawn[i];
                
                value.node.SetPreset();
                value.node.node.ShowCurrent(i * _showDelay);
            }
        }

        private void PrepareNext()
        {
            var list = new List<(MiniGameNode node, MiniGameNodePreset preset)>();
            var randomPresets = GetRandomPresets(_spawnCount);
            var randomNodes = GetRandomNodes(_spawnCount);

            for (int i = 0; i < randomNodes.Length; i++)
            {
                var preset = randomPresets[i];
                var node = randomNodes[i];
                var value = (node: node, preset: preset);
                
                node.SetNextPreset(preset);
                node.node.ShowNext(i * _showDelay);
                list.Add(value);
            }

            _nodesToSpawn = list.ToArray();
        }

        private MiniGameNode[] GetRandomNodes(int count)
        {
            var list = new List<MiniGameNode>();

            for (int i = 0; i < count; i++)
            {
                var rndX = UnityEngine.Random.Range(0, _width);
                var rndY = UnityEngine.Random.Range(0, _height);

                var node = SearchNextAvailableNode(rndX, rndY, (n) => !n.HasPreset && !list.Contains(n));
                list.Add(node);
            }

            return list.Where(x => x != null).ToArray();
        }

        private enum SearchingDirection { Forward = 1, Backward = -1 }
        
        private MiniGameNode SearchNextAvailableNode(int x, int y, Func<MiniGameNode, bool> predicate, SearchingDirection direction = SearchingDirection.Forward, int deep = 0)
        {
            for (int i = x; i >= 0 && i < _width; i += (int)direction)
            {
                for (int j = y; j >= 0 && j < _height; j += (int)direction)
                {
                    var node = _nodes[i, j];

                    var availability = predicate?.Invoke(node);
                    if (availability != null && availability.Value) return node;
                }
            }

            if (deep == 0)
                return SearchNextAvailableNode(x, y,
                    predicate,
                    (direction is SearchingDirection.Forward)
                        ? SearchingDirection.Backward
                        : SearchingDirection.Forward, deep + 1);
                
            return null;
        }

        private MiniGameNodePreset[] GetRandomPresets(int count)
        {
            var list = new List<MiniGameNodePreset>();
            
            for (int i = 0; i < count; i++)
                list.Add(_presets[UnityEngine.Random.Range(0, _presets.Length)]);

            return list.ToArray();
        }

        #endregion
    }
}