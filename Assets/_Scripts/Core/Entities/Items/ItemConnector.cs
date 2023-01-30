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
        private bool _available = true;

        private Item _connectedItem;

        public void FixedUpdate()
        {
            if (_available) return;

            _connectedItem.transform.position = transform.position;
            _connectedItem.transform.rotation = transform.rotation;
        }

        public void Attach(Item item)
        {
            if (!_available) return;

            item.transform.position = transform.position;
            item.transform.rotation = transform.rotation;
            
            _connectedItem = item;
            _available = false;
        }

        public void Detach()
        {
            _available = true;
            _connectedItem = null;
        } 

        public bool IsAttached(Item item)
        {
            if (_available) return false;
            if (item == _connectedItem) return true;

            return false;
        }

        public void MakeVisible(MeshFilter _itemMeshFilter)
        {
            if (!_available) return;

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