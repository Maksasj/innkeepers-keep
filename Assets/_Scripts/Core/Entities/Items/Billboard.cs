using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform _camera;

    void LateUpdate()
    {
        transform.LookAt(transform.position + _camera.forward);
    }
}
