using UnityEngine;
using Unity.Netcode;

namespace InkeepersKeep.Core.Network
{
    public class PlayerNetwork : NetworkBehaviour
    {
        private readonly NetworkVariable<PlayerNetworkData> _networkData = new(writePerm: NetworkVariableWritePermission.Owner);

        [SerializeField] private float _interpolationTime = 0.1f;
        private Vector3 _velocity;
        private float _rotationVelocity;

        //private void Update()
        //{
        //    if (IsOwner)
        //    {
        //        _networkData.Value = new PlayerNetworkData()
        //        {
        //            Position = transform.position,
        //            Rotation = transform.rotation.eulerAngles
        //        };
        //    } 
        //    else
        //    {
        //        transform.position = Vector3.SmoothDamp(transform.position, _networkData.Value.Position, ref _velocity, _interpolationTime);
        //        transform.rotation = Quaternion.Euler(0, Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, _networkData.Value.Rotation.y, ref _rotationVelocity, _interpolationTime), 0);
        //    }
        //}

        private struct PlayerNetworkData : INetworkSerializable
        {
            private Vector3 _position;
            private short _yRotation;

            internal Vector3 Position { get => _position; set => _position = value; }

            internal Vector3 Rotation
            {
                get => new Vector3(0, _yRotation, 0);
                set => _yRotation = (short)value.y;
            }

            public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
            {
                serializer.SerializeValue(ref _position);
                serializer.SerializeValue(ref _yRotation);
            }
        }
    }
}
