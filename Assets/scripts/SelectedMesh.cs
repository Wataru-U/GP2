using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using libFSTest;
using libFSTest.MeshController;
using System.Linq;

public class SelectedMesh : MonoBehaviour
{
    public GameObject mouseManager;
    private MouseManager mouseManagerCom;

    public GameObject main;
    private Mesh mainMesh;
    private Test mainSqr;

    private Mesh thisMesh;

    public Material selctedMat;

    private MeshList.Screenedriangles smaple = new MeshList.Screenedriangles();

    private bool MouseClick = false;
    private Vector2 MouseStartPos;
    public Vector2 mouseStartPos {get {return MouseStartPos;}}
    private Vector2 MousePos;

    // Start is called before the first frame update
    void Start()
    {
        mainMesh = main.GetComponent<MeshFilter>().mesh;
        mainSqr = main.GetComponent<Test>();
        thisMesh = GetComponent<MeshFilter>().mesh;
        mouseManagerCom = mouseManager.GetComponent<MouseManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mouseManagerCom.MouseMode == MouseManager.selectMode)
        {
            //マウスが押された
            if (Input.GetMouseButtonDown(0))
            {
                MouseClick = true;
                MouseStartPos = Input.mousePosition;
                //複数
                for(var i = 0;i<mainSqr.mainTriangles.Count / 3;i++)
                {
                    var (a, b, c) = (mainMesh.vertices[mainMesh.triangles[i * 3]], mainMesh.vertices[mainMesh.triangles[i * 3 + 1]], mainMesh.vertices[mainMesh.triangles[i * 3 + 2]]);
                    var (x, y, z) = (Vector3toV3(a), Vector3toV3(b), Vector3toV3(c));

                    var t =new MeshList.Triangle(x,y,z);
                    var CameraDir = V3toVector3(t.CenterV3) - Camera.main.transform.position;
                    if(Vector3.Dot(CameraDir,V3toVector3(t.Normal)) < 0)
                    {
                        Ray ray = Camera.main.ViewportPointToRay(t.ScreenPos);
                        RaycastHit hit;
                        
                        if (Physics.Raycast(ray, out hit, 20.0f))
                        {
                            var p_z = hit.point.z;
                            smaple.Add(t,p_z);
                        }
                    }
                }
            }

            //離れたとき
            if(Input.GetMouseButtonUp(0))
            {
                MousePos = Input.mousePosition;
            }
            // 配列の処理
            if(MouseClick == true && Input.GetMouseButton(0) == false)
            {
                MouseClick = false;
                smaple.Clear(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
                if(MouseStartPos == MousePos)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 10.0f))
                    {
                        var n = hit.triangleIndex;

                        
                        var (a, b, c) = (mainMesh.vertices[mainMesh.triangles[n * 3]], mainMesh.vertices[mainMesh.triangles[n * 3 + 1]], mainMesh.vertices[mainMesh.triangles[n * 3 + 2]]);
                        var (x, y, z) = (Vector3toV3(a), Vector3toV3(b), Vector3toV3(c));

                        smaple.Select(new MeshList.Triangle(x, y, z));
                    }
                }
                else
                {
                    smaple.SelectRange(MouseStartPos,MousePos);
                }
            }
        }
        thisMesh.SetVertices(smaple.Mesh.Select(x => V3toVector3(x)).ToList());
        thisMesh.SetTriangles(smaple.Triangles, 0);
        thisMesh.RecalculateNormals();
        Graphics.DrawMesh(thisMesh, transform.position, transform.rotation, selctedMat, 0);
    }


    MeshList.V3 Vector3toV3(Vector3 v)
    {
        return new MeshList.V3(v.x, v.y, v.z);
    }
    Vector3 V3toVector3(MeshList.V3 v)
    {
        return new Vector3(v.x, v.y, v.z);
    }
}



