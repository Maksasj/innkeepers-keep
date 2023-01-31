using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SphereNetworking : NetworkBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if(IsHost)
        {
            Debug.Log("Transfering ownership to client");
            if (!collision.gameObject.TryGetComponent<NetworkObject>(out NetworkObject _object)) return;

            GetComponent<NetworkObject>().ChangeOwnership(_object.OwnerClientId);
        }
        //if (!IsHost) return;
        //if (!collision.gameObject.TryGetComponent<NetworkBehaviour>(out NetworkBehaviour _object)) return;
        //GetComponent<NetworkObject>().ChangeOwnership(_object.OwnerClientId);
    }
}
