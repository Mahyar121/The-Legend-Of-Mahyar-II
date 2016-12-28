using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public float xMax, yMax, xMin, yMin, xPadding, yPadding;
    public Transform target;
	
	private void LateUpdate ()
    {
        transform.position = new Vector3(
            Mathf.Clamp(target.position.x + xPadding, xMin, xMax),
            Mathf.Clamp(target.position.y + yPadding, yMin, yMax),
            transform.position.z
            );
    }
}
