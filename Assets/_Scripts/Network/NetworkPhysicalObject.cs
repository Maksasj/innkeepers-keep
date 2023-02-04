using UnityEngine;
using Unity.Netcode;

namespace InkeepersKeep.Core.Network
{
    public class NetworkPhysicalObject : NetworkBehaviour
    {
        void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.TryGetComponent(out NetworkObject _object)) return;

            if(OwnerClientId != _object.OwnerClientId)
                GiveOwnershipServerRpc(_object.OwnerClientId);
        }

        public void TakeOwnership(ulong clientId) => GiveOwnershipServerRpc(clientId);

        [ServerRpc(RequireOwnership = false)]
        private void GiveOwnershipServerRpc(ulong clientId)
        {
            Debug.Log("Server gets message about changing ownership, executed by: " + OwnerClientId);
            GetComponent<NetworkObject>().ChangeOwnership(clientId);
        }
    }
}
