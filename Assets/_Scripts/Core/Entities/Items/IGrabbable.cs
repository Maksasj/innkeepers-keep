using UnityEngine;

namespace InkeepersKeep.Core.Entities.Items
{
    public interface IGrabbable
    {
        void Grab(Transform grabPoint);
        void Drop();
    }
}
