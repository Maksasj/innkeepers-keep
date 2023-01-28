using UnityEngine;

namespace InkeepersKeep.Core.Entities
{
    public class GroundCheck : MonoBehaviour
    {
        [SerializeField] private Transform _groundCheckTransform;
        [SerializeField] private float _radius;
        [SerializeField] private LayerMask _groundLayer;

        public bool Check() => Physics.CheckSphere(_groundCheckTransform.position, _radius, _groundLayer);
    }
}
