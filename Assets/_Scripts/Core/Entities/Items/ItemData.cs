using System;
using UnityEngine;

[CreateAssetMenu(fileName = "newItemData", menuName = "Item")]
public class ItemData : ScriptableObject
{
    [Header("General")]
    public string name;
    public string description;

    [Header("Rendering")]
    public Mesh mesh;
    public Material material;
    public Vector3 scale;
}