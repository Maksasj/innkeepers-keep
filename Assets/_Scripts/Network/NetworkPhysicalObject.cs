using System;
using UnityEngine;
using Unity.Netcode;

namespace InkeepersKeep.Network
{
    public class NetworkPhysicalObject : NetworkBehaviour
    {
        void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.TryGetComponent(out NetworkObject _object)) return;

            if (OwnerClientId == _object.OwnerClientId) return;

            GiveOwnershipServerRpc(_object.OwnerClientId);
        }

        public void TakeOwnership(ulong clientId)
        {
            if (clientId == OwnerClientId) return;

            GiveOwnershipServerRpc(clientId);
        }

        [ServerRpc(RequireOwnership = false)]
        private void GiveOwnershipServerRpc(ulong clientId)
        {
            GetComponent<NetworkObject>().ChangeOwnership(clientId);
        }
    }
}
