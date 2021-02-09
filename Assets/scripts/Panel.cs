using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    public GameObject s;
    public GameObject mouseManager;

    RectTransform rect;
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent <RectTransform> ();
    }

    // Update is called once per frame
    void Update()
    {
        var screenSize = new Vector2(-Screen.width,-Screen.height);
        rect.sizeDelta = screenSize;
        var mouseSqript = mouseManager.GetComponent<MouseManager>();
        if(mouseSqript.MouseMode == MouseManager.selectMode)
        {
            if(Input.GetMouseButton(0))
            {
                var startPos = s.GetComponent<SelectedMesh>().mouseStartPos;
                var P = Input.mousePosition;
                var left = new Vector2(Mathf.Min(startPos.x,P.x),Mathf.Min(startPos.y,P.y));
                var right = new Vector2(Mathf.Max(startPos.x,P.x),Mathf.Max(startPos.y,P.y));
                rect.anchoredPosition = (left + right + screenSize) / 2;
                rect.sizeDelta += right - left;
                Debug.Log(right-left);
            }
        }
    }
}
