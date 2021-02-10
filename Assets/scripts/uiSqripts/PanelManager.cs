using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PanelManager : MonoBehaviour
{
    public GameObject main;
    //３つのPanelを格納する変数
    //インスペクターウィンドウからゲームオブジェクトを設定する
    [SerializeField] GameObject setButton;
    [SerializeField] GameObject Edit2Panel;
    private MainModel mainSqript;
    private EditMode editMode;
    void Start()
    {
        mainSqript = main.GetComponent<MainModel>();
        
    }

    // Update is called once per frame
    void Update()
    {
        editMode = mainSqript._editMode;
        if(editMode.compare(EditMode.select))
        {
            Edit2Panel.SetActive(true);
            setButton.SetActive(false);
        }
        else
        {
            setButton.SetActive(true);
            Edit2Panel.SetActive(false);
        }
    }
}

//https://xr-hub.com/archives/11782 UI Active