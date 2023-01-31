using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SphereNetworking : NetworkBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (!IsHost) return;
        if (!collision.gameObject.TryGetComponent<NetworkBehaviour>(out NetworkBehaviour _object)) return;
        GetComponent<NetworkObject>().ChangeOwnership(_object.OwnerClientId);
    }
}
