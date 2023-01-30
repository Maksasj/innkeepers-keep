using InkeepersKeep.Core.Entities.Items;
using UnityEngine;

namespace InkeepersKeep.Core.Entities
{
    public class ItemHovering : MonoBehaviour
    {
        [SerializeField] private float _grabDistance;
        [SerializeField] private LayerMask _grabbableObjectLayer;

        private IHoverable previousItem;

        private void Update()
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (!Physics.Raycast(ray, out hit, _grabDistance, _grabbableObjectLayer))
            {
                previousItem?.Unhover();
                return;
            }
            if (!hit.collider.TryGetComponent(out IHoverable item))
            {
                previousItem?.Unhover();
                return;
            }

            item.Hover();

            if (previousItem == item) return;

            previousItem?.Unhover();
            previousItem = item;
        }
    }
}
