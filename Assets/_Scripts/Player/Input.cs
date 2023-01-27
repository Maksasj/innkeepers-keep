using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input : MonoBehaviour
{
    private Controls _controls;

    public Controls controls => _controls;

    public void Initialize()
    {
        _controls = new Controls();
    }

    public void Enable()
    {
        _controls.Enable();
    }

    public void Disable()
    {
        _controls.Disable();
    }

    public Vector3 GetMovement()
    {
        return _controls.Player.WASD.
    }
}
