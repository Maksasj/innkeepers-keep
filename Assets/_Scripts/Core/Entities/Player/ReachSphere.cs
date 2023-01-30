using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InkeepersKeep.Core.Entities
{
   public class ReachSphere : MonoBehaviour
    {
        [SerializeField] private ItemDragging _itemDragging;

        void OnTriggerEnter(Collider collider)
        {
            if (!_itemDragging._isDragging) return;
            if (!collider.gameObject.TryGetComponent<ItemConnector>(out ItemConnector connector)) return;
                
            connector.MakeVisible(_itemDragging._currentItemMeshFilter);
        }

        void OnTriggerExit(Collider collider)
        {
            if (!_itemDragging._isDragging) return;
            if (!collider.gameObject.TryGetComponent<ItemConnector>(out ItemConnector connector)) return;

            connector.MakeInvisible();
        }
    }
}