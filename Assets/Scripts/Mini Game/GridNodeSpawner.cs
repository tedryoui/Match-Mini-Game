using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mini_Game
{
    public class GridNodeSpawner : MonoBehaviour
    {
        [FormerlySerializedAs("_nodePrefab")]
        [Header("Reference")]
        [SerializeField] private MiniGameNodeObject nodeObjectPrefab;

        [Header("Grid Settings")]
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private Vector2 _nodeSizes;
        [SerializeField] private float _betweenNodesSpace;
        
        private Vector2 GridSize =>
            new Vector2(_nodeSizes.x * _width, _nodeSizes.y * _height) +
            new Vector2(_betweenNodesSpace * (_width - 1), _betweenNodesSpace * (_height - 1));

        public void SetWidthAndHeight(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public void SetSizesAndSpaces(Vector2 sizes, float space)
        {
            _nodeSizes = sizes;
            _betweenNodesSpace = space;
        }
        
        public void SpawnNodes()
        {
            ClearChildren();
            
            for(int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    // Create node from prefab
                    var node = Instantiate(nodeObjectPrefab, transform);
                    
                    // Set node sizes
                    (node.transform as RectTransform).sizeDelta = _nodeSizes;

                    PlaceNodeInPosition(node, j, i);
                }
            }
        }

        private void ClearChildren()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
                DestroyImmediate(transform.GetChild(i).gameObject);
        }

        private void PlaceNodeInPosition(MiniGameNodeObject nodeObject, int x, int y)
        {
            // Compute offset values
            var spaceBetweenOffset = new Vector2(x * _betweenNodesSpace, y * _betweenNodesSpace);
            var positionOffset = new Vector2(x * _nodeSizes.x, y * _nodeSizes.y);
            var centerOffset = _nodeSizes / 2.0f - GridSize / 2.0f;

            (nodeObject.transform as RectTransform).localPosition = spaceBetweenOffset + positionOffset + centerOffset;
        }
    }
}