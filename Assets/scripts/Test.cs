using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Linq;
using libFSTest;
using libFSTest.MeshController;
using System.Threading.Tasks;

//ボツになったやつのメインのところ

[RequireComponent(typeof(Camera))]
public class Test : MonoBehaviour
{
    public GameObject mouseManager;

    private Mesh mesh;
    private MeshCollider meshCollider;

    private Material pointMat;
    private Material lineMat;

    public int size = 10;
    private int pSize;
    private float _pointSize = 4.0f;
    public float Deg;
    private float Rad => pSize / (pSize - 1) * Deg * Mathf.Deg2Rad;
    public Vector3[] vert = new Vector3[4];

    public List<Vector3> mainVertics = new List<Vector3>();
    public List<int> mainTriangles = new List<int>();
    int[] points = new int[0];
    int[] lines = new int[0];
    private Material M;
    public Material sub;


    private MeshList.Screenedriangles smaple = new MeshList.Screenedriangles();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Camera.main.WorldToScreenPoint(new Vector3(0,0,0)));
        mesh = GetComponent<MeshFilter>().mesh;
        M = this.GetComponent<Renderer>().material;

        pointMat = new Material(Shader.Find("testSencePoints"));
        lineMat = new Material(Shader.Find("testSenceLine"));

        meshCollider = gameObject.GetComponent<MeshCollider>();
    }



    // Update is called once per frame
    void Update()
    {

        if (size != pSize) //変更があったときだけ
        {
            pSize = size;
            mainVertics = new List<Vector3>();
            mainTriangles = new List<int>();
            mainVertics.AddRange(rotation(bezier(vert, size), Rad, size));
            var t = triangle(size + 1, size + 1);
            mainTriangles.AddRange(t);
            ReculculatePoints();
            ReculcurateLines();
        }
        //描画
        mesh.SetVertices(mainVertics);
        mesh.SetTriangles(mainTriangles, 0);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        Graphics.DrawMesh(mesh, transform.position, transform.rotation, M, 0);

        DrawPoints(mesh);
        DrawLines(mesh);

        Resources.UnloadUnusedAssets();

        if(mouseManager.GetComponent<MouseManager>().MouseMode == MouseManager.selectMode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //meshcoliderに mesh　を割り当て
                meshCollider.sharedMesh = mesh; 
            }
        }
        
    }


    public static Vector3[] bezier(Vector3[] val, int num)
    {
        var result = new Vector3[num + 1];
        float n = (1f / (float)num);
        for (int i = 0; i < num + 1; i++)
        {
            float t = n * i;
            float t_ = 1 - t;
            Vector3 a = t_ * val[0] + t * val[1];
            Vector3 b = t_ * val[1] + t * val[2];
            Vector3 c = t_ * val[2] + t * val[3];

            result[i] = (t_ * a + t * b) * t_ + (b * t_ + t * c) * t;
        }
        return result;
    }
    public static Vector3[] rotation(Vector3[] val, float rad, int num)
    {
        var h = val.Length;
        var result = new Vector3[h * (num + 1)];
        var theta = rad / num;
        for (int i = 0; i < h * (num + 1); i++)
        {
            var p = i % h;
            var n = (int)(i / h);
            result[i].x = val[p].x * Mathf.Cos(theta * n) - val[p].z * Mathf.Sin(theta * n);
            result[i].z = val[p].x * Mathf.Sin(theta * n) + val[p].z * Mathf.Cos(theta * n);
            result[i].y = val[p].y;
        }
        return result;
    }
    public int[] triangle(int h, int w)
    {
        int[] result = new int[(h) * (w) * 6];
        for (int i = 0; i < w - 1; i++)
        {
            for (int j = 0; j < h - 1; j++)
            {
                result[(j + (h) * i) * 6] = (j + (h) * i);
                result[(j + (h) * i) * 6 + 1] = (j + (h) * i) + 1;
                result[(j + (h) * i) * 6 + 2] = (j + (h) * i) + h;
                result[(j + (h) * i) * 6 + 3] = (j + (h) * i) + 1;
                result[(j + (h) * i) * 6 + 4] = (j + (h) * i) + h + 1;
                result[(j + (h) * i) * 6 + 5] = (j + (h) * i) + h;
            }
        }
        return result;
    }

    void ReculculatePoints()
    {
        points = new int[mainVertics.Count];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = i;
        }
    }

    void DrawPoints(Mesh mesh)
    {
        var _pointsMesh = Instantiate(mesh);
        _pointsMesh.SetIndices(points, MeshTopology.Points, 0);

        var transformMatrix = Matrix4x4.TRS(Vector3.zero, transform.rotation, transform.lossyScale);
        pointMat.SetMatrix("_TransformMatrix", transformMatrix);
        pointMat.SetFloat("_PointSize", _pointSize);
        Graphics.DrawMesh(_pointsMesh, transform.position, Quaternion.identity, pointMat, 0);
    }

    void ReculcurateLines()
    {
        lines = new int[mainTriangles.Count * 2];
        for (int i = 0; i < lines.Length / 6; i++)
        {
            lines[i * 6] = mainTriangles[i * 3];
            lines[i * 6 + 1] = mainTriangles[i * 3 + 1];
            lines[i * 6 + 2] = mainTriangles[i * 3 + 1];
            lines[i * 6 + 3] = mainTriangles[i * 3 + 2];
            lines[i * 6 + 4] = mainTriangles[i * 3 + 2];
            lines[i * 6 + 5] = mainTriangles[i * 3];
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

    MeshList.V3 Vector3toV3(Vector3 v)
    {
        return new MeshList.V3(v.x, v.y, v.z);
    }
    Vector3 V3toVector3(MeshList.V3 v)
    {
        return new Vector3(v.x, v.y, v.z);
    }
}


/*
https://sleepygamersmemo.blogspot.com/2017/04/unity-mesh-plain.html colliderにアッタッチ
https://www.sejuku.net/blog/83620  raycast
*/
