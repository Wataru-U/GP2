using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//カメラの焦点
//ここを平行移動させてカメラを並行移動させる
public class CametaTranslation : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mouseManager;
    public GameObject Camera;
    private CameraMove cameraSqript;
    Vector3 pos;
    float translationSpeed = 1.5f;
    void Start()
    {
        pos = transform.position;
        cameraSqript = Camera.GetComponent<CameraMove>();
    }

    // Update is called once per frame
    void Update()
    {
        var mouseSqript = mouseManager.GetComponent<MouseManager>();
        if(mouseSqript.MouseMode == MouseManager.moveMode )
        {
            if(Input.GetMouseButton(0))
            {
                Vector3 dir = pos - Camera.transform.position;
                float yDir = dir.y / Mathf.Abs(dir.y);
                dir.y = 0;
                dir = Vector3.Normalize(dir);
                pos.x +=  translationSpeed * (Input.GetAxis("Mouse Y") * dir.x  * yDir - Input.GetAxis("Mouse X") * dir.z);
                pos.z +=  translationSpeed * (Input.GetAxis("Mouse Y") * dir.z  * yDir + Input.GetAxis("Mouse X") * dir.x);
            }
        }
        transform.position = pos;
    }


}
