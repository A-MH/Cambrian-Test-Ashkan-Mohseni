using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 NewPosition { get; set; }

    private void LateUpdate()
    {
        Vector3 velocity = new();
        transform.position = Vector3.SmoothDamp(transform.position, NewPosition, ref velocity, 0.03f);
    }
}
