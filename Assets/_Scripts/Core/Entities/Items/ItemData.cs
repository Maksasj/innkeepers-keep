using System;
using UnityEngine;

namespace InkeepersKeep.Core.Entities.Items
{
    [Serializable]
    public struct ItemData
    {
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private ItemType _type;

        public string Name => _name;
        public string Description => _description;
        public ItemType Type => _type;

        public ItemData(string name, string description, ItemType type)
        {
            _name = name;
            _description = description;
            _type = type;
        }
    }
}
