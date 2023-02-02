using System;
using UnityEngine;

namespace InkeepersKeep.Core.Entities.Items
{
    [CreateAssetMenu(fileName = "newItemData", menuName = "Item")]
    public class ItemData : ScriptableObject
    {
        [Header("General")]
        public string name;
        public string description;

        public ItemType type;
        public ItemSubType subType;
    }
}
