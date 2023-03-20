using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{

    public Transform followTransform;
    public BoxCollider2D worldBounds;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    float camX;
    float camY;

    float camRatio;
    float camSize;

    Camera mainCam;

    Vector3 smoothPos;

    public float smoothRate;

    // Start is called before the first frame update
    void Start()
    {
        xMin = worldBounds.bounds.min.x;
        xMax = worldBounds.bounds.max.x;
        yMin = worldBounds.bounds.min.y;
        yMax = worldBounds.bounds.max.y;

        mainCam = gameObject.GetComponent<Camera>();

        camSize = mainCam.orthographicSize;
        camRatio = (xMax + camSize) / 8.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        camY = Mathf.Clamp(followTransform.position.y, yMin + camSize, yMax - camSize);
        camX = Mathf.Clamp(followTransform.position.x, xMin + camRatio, xMax - camRatio);

        smoothPos = Vector3.Lerp(gameObject.transform.position, new Vector3(camX, camY, gameObject.transform.position.z), smoothRate);
    
        gameObject.transform.position = smoothPos;
    }
}
