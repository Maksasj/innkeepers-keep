using InkeepersKeep.Core.Entities.Items;
using UnityEngine;

namespace InkeepersKeep.Core.Entities
{
    public class ItemConnector : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private Mesh _defaultMesh;

        private bool _available = true;

        public void Attach(Item item)
        {
            if (!_available)
                return;

            item.transform.position = transform.position;
            item.transform.rotation = transform.rotation;
            
            _available = false;
        }

        public void Detach() => _available = true;

        public void MakeVisible(MeshFilter _itemMeshFilter)
        {
            if (!_available)
                return;

            _renderer.enabled = true;
            
            if (TryGetComponent(out MeshCollider _meshCollider))
            {
                _meshFilter.mesh = _itemMeshFilter.mesh;
                _meshCollider.sharedMesh = _itemMeshFilter.sharedMesh;
            }
        }

        public void MakeInvisible() => _renderer.enabled = false;
    }
}