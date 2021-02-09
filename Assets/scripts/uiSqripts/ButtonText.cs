using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  

public class ButtonText : MonoBehaviour
{
    public GameObject thisObject = null;
    Text txt;
    public GameObject main;
    private Positions sqript;
    // Start is called before the first frame update
    void Start()
    {
        sqript = main.GetComponent<Positions>();
        txt = this.thisObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        txt.text = sqript._editMode.type;
    }
}
