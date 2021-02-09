using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using libFSTest.MeshController;

public class Selected : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mainObj;
    private Positions mainSqript;
    Polygon.Mesh mainMesh;
    int selectedFaceNum;

    private Mesh selectedMesh;
    private Material Mat;
    private List<int> selectedTriangles;

    void Start()
    {
        mainSqript = mainObj.GetComponent<Positions>();
        mainMesh = mainSqript.sample;
        selectedTriangles = new List<int>();
        selectedMesh = GetComponent<MeshFilter>().mesh;
    }

    // Update is called once per frame
    void Update()
    {
        if(mainSqript.draw)
        {
            selectedFaceNum = mainSqript.SelectedFaceNum;
            selectedTriangles = mainMesh.faces[selectedFaceNum].Triangles;
            selectedMesh.SetVertices(mainSqript.vertics);
            selectedMesh.SetTriangles(selectedTriangles, 0);
            selectedMesh.RecalculateNormals();
            Graphics.DrawMesh(selectedMesh, transform.position, transform.rotation, Mat, 0);
        }
    }
}
