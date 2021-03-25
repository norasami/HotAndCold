using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followTransform;
    public BoxCollider2D mapBounds;
    float xMin, xMax, yMin, yMax, camYBounds, camXBounds, camOrthSize, camRatio;
    Camera mainCam;

    void Start()
    {
        xMin = mapBounds.bounds.min.x;
        xMax = mapBounds.bounds.max.x;
        yMin = mapBounds.bounds.min.y;
        yMax = mapBounds.bounds.max.y;
        mainCam = GetComponent<Camera>();
        camOrthSize = mainCam.orthographicSize;
        camRatio = (xMax + camOrthSize) / 2.0f;
    }

    void Update()
    {
        camYBounds = Mathf.Clamp(followTransform.position.y, yMin + camOrthSize, yMax - camOrthSize);
        camXBounds = Mathf.Clamp(followTransform.position.x, xMin + camRatio, xMax - camRatio);
        this.transform.position = new Vector3(camXBounds, camYBounds, this.transform.position.z);
    }
}
