using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private ItemData _itemData;

    [Header("Rendering")]
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private Renderer _renderer;

    private void Awake()
    {
        _meshFilter.mesh = _itemData.mesh;
        _renderer.material = _itemData.material;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
