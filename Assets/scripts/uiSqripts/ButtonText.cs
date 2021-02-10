using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  

//使用せず
public class ButtonText : MonoBehaviour
{
    public GameObject thisObject = null;
    Text txt;
    public GameObject main;
    private MainModel sqript;
    // Start is called before the first frame update
    void Start()
    {
        sqript = main.GetComponent<MainModel>();
        txt = this.thisObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        txt.text = sqript._editMode.type;
    }
}
