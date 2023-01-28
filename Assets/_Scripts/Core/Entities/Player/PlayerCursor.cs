using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    private Pickable lastHover;

    [SerializeField] private Transform _hands;

    private GameObject _activeGameObject;

    private bool _dragging;

    public void Update() {
        if(_dragging == false) return;
        
        _activeGameObject.transform.position = _hands.transform.position;
    }

    public void StartDragging()
    {

        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward * 100f, Color.yellow);
        
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit)) {
            GameObject pickedGameObject = hit.collider.gameObject;
        
            if(pickedGameObject.GetComponent<Pickable>()) {
                _hands.transform.position = hit.point;

                _dragging = true;
                _activeGameObject = pickedGameObject;
            }
        }       
    }

    public void StopDragging()
    {
        _dragging = false;
    }
}
