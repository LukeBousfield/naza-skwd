using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow1 : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    private void LateUpdate()
    {
        Vector3 desiredPosition = new Vector3(target.position.x, (target.position.y / 2) + 90, -1 * (target.position.y) - 140);
        transform.position = desiredPosition + offset;
    }
}
