using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camcam : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(target.position);
        Debug.Log("target is " + screenPos.x + " pixels from the left");
    }
}
