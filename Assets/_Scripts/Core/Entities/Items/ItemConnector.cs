using InkeepersKeep.Core.Entities.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InkeepersKeep.Core.Entities
{
    public class ItemConnector : MonoBehaviour
    {
        [SerializeField] private Renderer       _renderer;
        [SerializeField] private MeshFilter     _meshFilter;
        [SerializeField] private Mesh           _defaultMesh;
        [SerializeField] private MeshCollider   _meshCollider;

        public bool _colliding;

        public void Connect(Item item)
        {
            item.transform.position = transform.position;
        }

        public void MakeVisible(MeshFilter _itemMeshFilter)
        {
            _renderer.enabled = true;
            
            if (!TryGetComponent<MeshCollider>(out MeshCollider _meshCollider)) return;

            _meshFilter.mesh = _itemMeshFilter.mesh;
            _meshCollider.sharedMesh = _itemMeshFilter.sharedMesh;
        }

        public void MakeInvisible()
        {
            _renderer.enabled = false;
        }

        void OnTriggerEnter(Collider collider)
        {
            if (!collider.gameObject.TryGetComponent<Item>(out Item item)) return;
            _colliding = true;
            
            item._isCollidingWithItemConnector = true;
            item._itemConnector = GetComponent<ItemConnector>();
        }

        void OnTriggerExit(Collider collider)
        {
            if (!collider.gameObject.TryGetComponent<Item>(out Item item)) return;
            
            _colliding = false;
            
            item._isCollidingWithItemConnector = false;
        }
    }
}