using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Character_Control : MonoBehaviour
{
    private Vector3 dragOffset;
    private Camera _camera;

    void Awake()
    {
        _camera = Camera.main;
    }

    void OnMouseDown()
    {
        dragOffset = transform.position - GetMousePosition();
    }

    void OnMouseDrag()
    {
        {
            transform.position = GetMousePosition() + dragOffset;
        }
    }

    Vector3 GetMousePosition()
    {
        Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }
}
