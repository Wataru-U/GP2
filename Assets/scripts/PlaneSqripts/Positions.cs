using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using libFSTest;
using libFSTest.MeshController;

public class Positions : MonoBehaviour
{
    public bool draw = false;
    public GameObject mouseManager;
    private MouseManager mouseSqript;
    public int SelectedFaceNum = 0;
    private MeshCollider meshCollider;
    private Mesh mesh;
    public Material M;
    public Material Mat;
    private Material lineMat;
    
    private bool LineDraw = false;

    private float _pointSize = 6.0f;

    public List<Vector3> vertics = new List<Vector3>();
    private List<int> triangles = new List<int>();
    int[] lines = new int[0];

    public Polygon.Mesh sample = new Polygon.Mesh();


    private int EditMode = 0;
    public static EditMode makeface = new EditMode(0,"makeFace");
    public static EditMode select = new EditMode(2,"select");

    public EditMode _editMode;



    
    // Start is called before the first frame update
    void Start()
    {
        _editMode = makeface;
        mouseSqript = mouseManager.GetComponent<MouseManager>();
        mesh = GetComponent<MeshFilter>().mesh;
        lineMat = new Material(Shader.Find("MainLines"));
        sample.AddNewFace();
        meshCollider = gameObject.GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_editMode.type);
        

        if(mouseSqript.MouseMode == MouseManager.selectMode && onScreen())
        {
            if(Input.GetMouseButtonDown(0))
            {
                //面をつくるために頂点と辺を追加する
                if(_editMode.compare(makeface))
                {
                    var p = new Vector3();
                    GetPosition(out p);
                    sample.AddVertex(p);
                    sample.AddEdgeTail(SelectedFaceNum,sample.verticiesCount-1);
                }
                else if (_editMode.compare(select))
                {
                    //面番号を所得して編集できるようにする
                    //meshcoliderに mesh　を割り当て
                    meshCollider.sharedMesh = mesh; 
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                        
                    if (Physics.Raycast(ray, out hit, 20.0f))
                    {
                        var index = hit.triangleIndex;
                        SelectedFaceNum = sample.triangleindecies[index];
                    }
                }
            }
            //平行移動
            if(_editMode.compare(select))
            {
                var value = 0f;
                if(Input.GetMouseButton(0))
                {
                    value += Input.GetAxis("Mouse Y") * 1.5f;
                }
                sample.FaceTranslation(SelectedFaceNum,value);
            }
        }
        //本来の描画
        if(draw)
        {
            mesh.SetVertices(vertics);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            Graphics.DrawMesh(mesh, transform.position, transform.rotation, Mat, 0);
        }
        //頂点の描画
        if(sample.verticiesCount != 0)
        {
            if(!draw)
            {
                //meshをコピーしているので本体を描画していないときは頂点もセットされていない
                mesh.SetVertices(sample.vertics);
            }
            var _pointsMesh = Instantiate(mesh);
            var pointIndex = new List<int>();
            for(int i=1;i<sample.verticiesCount;i++)
            {
                pointIndex.Add(i-1);  //頂点全てを描画
            }
            _pointsMesh.SetIndices(pointIndex.ToArray(), MeshTopology.Points, 0);
        
            var transformMatrix = Matrix4x4.TRS(Vector3.zero, transform.rotation, transform.lossyScale);
            M.SetMatrix("_TransformMatrix", transformMatrix);
            M.SetFloat("_PointSize", _pointSize);
            Graphics.DrawMesh(_pointsMesh, transform.position, Quaternion.identity, M, 0);
        }
        if(LineDraw)
        {
            DrawLines(mesh);
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            sample.MakePillar(SelectedFaceNum);
            sample.FaceTranslation(SelectedFaceNum,2);
            vertics = sample.vertics;
            triangles = sample.Triangles();
            lines = sample.SarchEdge().ToArray();
            LineDraw = true;
            draw = true;
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            sample.Push(SelectedFaceNum);
            triangles = sample.Triangles();
            lines = sample.SarchEdge().ToArray();
        }
        
    }

    public void Push()
    {
        sample.Push(SelectedFaceNum);
        triangles = sample.Triangles();
        lines = sample.SarchEdge().ToArray();
    }

    public void MakeFace()
    {
        _editMode = makeface;
        SelectedFaceNum = sample.faceCount;
        sample.AddNewFace();
        foreach (var item in sample.faces)
        {
            Debug.Log(item.num);
        }
    }

    public void MakePilar()
    {
        
            sample.MakePillar(SelectedFaceNum);
            vertics = sample.vertics;
            triangles = sample.Triangles();
            lines = sample.SarchEdge().ToArray();
            LineDraw = true;
            _editMode = select;

        draw = true;

        foreach (var item in sample.faces)
        {
            Debug.Log(item.num);
        }
    }

    public bool GetPosition(out Vector3 result)
    {
        // カメラはメインカメラを使う
        var camera = Camera.main;
        // クリック位置を取得
        var touchPosition = Input.mousePosition;
        // XZ平面を作る
        var plane = new Plane(Vector3.up, 0);
        // カメラからのRayを作成
        var ray = camera.ScreenPointToRay(touchPosition);
        // rayと平面の交点を求める（交差しない可能性もある）
        if (plane.Raycast(ray, out float enter))
        {
            result = ray.GetPoint(enter);
            return true;
        }
        else
        {
            // rayと平面が交差しなかったので座標が取得できなかった
            result = Vector3.zero;
            return false;
        }
    }

    void DrawLines(Mesh mesh)
    {
        var _linesMesh = Instantiate(mesh);
        _linesMesh.SetIndices(lines, MeshTopology.Lines, 0);

        var transformMatrix = Matrix4x4.TRS(Vector3.zero, transform.rotation, transform.lossyScale);
        lineMat.SetMatrix("_TransformMatrix", transformMatrix);
        lineMat.SetFloat("_PointSize", _pointSize);
        Graphics.DrawMesh(_linesMesh, transform.position, Quaternion.identity, lineMat, 0);
    }

    bool onScreen()
    {
        var condition = new Vector3(Screen.width,Screen.height,0)*0.85f ;
        return condition.x > Input.mousePosition.x && condition.y > Input.mousePosition.y;
    }

}

public struct EditMode
{
    public int ModeNum;
    public string type;

    public EditMode(int n,string t)
    {
        ModeNum = n;
        type = t;
    }


    public bool compare(EditMode m)
    {
        return ModeNum == m.ModeNum;
    }

    public static EditMode makeface = new EditMode(0,"makeFace");
    public static EditMode select = new EditMode(2,"select");
}

// https://nyama41.hatenablog.com/entry/click_position_to_xy_plane_position