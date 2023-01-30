using UnityEngine;

namespace InkeepersKeep.Core.Entities.Items
{
    public interface IColorTintable
    {
        void ChangeTint();
        void ReturnDefaultTint();
    }
}
