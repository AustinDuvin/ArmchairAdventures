using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapData : MonoBehaviour
{
    public string filepath;
    public string mapName;
    public string size;

    public GameObject titleText;
    public GameObject sizeText;

    public static string LoadedName;
    public static string LoadedSize;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FillTextBox()
    {
        GameObject.Find("InputField").GetComponent<InputField>().text = mapName;
    }

    public void SelectData()
    {
        LoadedName = mapName;
        LoadedSize = size;

        GameObject.Find("OpenButton").GetComponent<Button>().interactable = true;
    }
}
