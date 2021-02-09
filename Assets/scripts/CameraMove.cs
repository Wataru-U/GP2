using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//回転するところ
public class CameraMove : MonoBehaviour
{
    public GameObject target;
    public GameObject mouseManager;

    public float r;
    public float horiDeg;
    public float horiRad => (horiDeg - 90) * Mathf.Deg2Rad;
    public float vertDeg;
    public float vertRad =>　vertDeg * Mathf.Deg2Rad;

    public float rotateSpeed = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = target.transform.position;
        Vector3 pos = new Vector3(r,r,r);

        var mouseSqript = mouseManager.GetComponent<MouseManager>();
        if(mouseSqript.MouseMode == MouseManager.rotationMode )
        {
            if(Input.GetMouseButton(0))
            {
                vertDeg += -Input.GetAxis("Mouse Y") * rotateSpeed;
                horiDeg += -Input.GetAxis("Mouse X") * rotateSpeed;
            }
        }
        vertDeg = Mathf.Clamp(vertDeg,-90,90);
        if(Input.GetKey(KeyCode.Z))
        {
            r -= 0.3f;
            r = Mathf.Max(r,0);
        }
        if(Input.GetKey(KeyCode.X))
        {
            r += 0.3f;
            r = Mathf.Min(r,100);
        }

        pos.x *= Mathf.Cos(horiRad) * Mathf.Cos(vertRad);
        pos.y *= Mathf.Sin(vertRad);
        pos.z *= Mathf.Sin(horiRad) * Mathf.Cos(vertRad);
        
        transform.position = pos + targetPos;
        transform.LookAt(targetPos);
    }
}

//http://phisz.blog.fc2.com/blog-entry-25.html まうす移動りょう
