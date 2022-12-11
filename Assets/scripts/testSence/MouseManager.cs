using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    private int mouseMode = 1;
    public int MouseMode { get { return mouseMode; } }

    public static int selectMode = 0b_000;
    public static int rotationMode = 0b_001;
    public static int moveMode = 0b_010;

    // Start is called before the first frame update
    void Start()
    {
        mouseMode = rotationMode;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            mouseMode = selectMode;
        }
        if (Input.GetKey(KeyCode.R))
        {
            mouseMode = rotationMode;
        }
        if (Input.GetKey(KeyCode.M))
        {
            mouseMode = moveMode;
        }

    }

    public void setMode(int v)
    {
        mouseMode = v;
    }

    public void setMove()
    {
        setMode(moveMode);
    }
    public void setRotation()
    {
        setMode(rotationMode);
    }
    public void setSelect()
    {
        setMode(selectMode);
    }
}

//https://www.sejuku.net/blog/56265 ボタン