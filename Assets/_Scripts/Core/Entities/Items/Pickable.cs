using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    [SerializeField] private GameObject _ItemUI;

    public void Hover() {
        _ItemUI.SetActive(true);
    }

    public void UnHover() {
        _ItemUI.SetActive(false);
    }
}
