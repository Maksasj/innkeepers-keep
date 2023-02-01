using UnityEngine;
using Unity.Netcode;

namespace InkeepersKeep.Core.Utils
{
    public class NetworkPhysicalObject : NetworkBehaviour
    {
        void OnCollisionEnter(Collision collision)
        {
            /* Since only host have rights to modify ownership,
             * we should firstly check if it is Host.
             * 
             * In the future we will need implement 
             * a way to allow transfering ownershipt 
             * from client to another client or host 
            */

            if (!IsHost) return;
            if (!collision.gameObject.TryGetComponent<NetworkObject>(out NetworkObject _object)) return;

            GetComponent<NetworkObject>().ChangeOwnership(_object.OwnerClientId);
        }
    }
}
