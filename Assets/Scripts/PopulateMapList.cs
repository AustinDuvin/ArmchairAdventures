using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PopulateMapList : PopulateList
{

    // Start is called before the first frame update
    void Start()
    {
        GenerateMapList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddOption(string filePath, GameObject optionPrefab)
    {
        GameObject add = Instantiate(optionPrefab, /*new Vector3(0.0f, -120.0f * transform.childCount, 0.0f), new Quaternion(),*/ transform);
        //add.GetComponent<RectTransform>().anchoredPosition/*.transform.localPosition*/ = new Vector3(0.0f, -120.0f - (213.2238f * (transform.childCount - 1)), 0.0f);
        optionList.Add(add);
        add.GetComponent<MapData>().filepath = filePath;

        if (ManagePopup.currentPopup == "SaveMapPopup")
        {
            add.GetComponent<Button>().onClick.AddListener(delegate { add.GetComponent<MapData>().FillTextBox(); });
        }

        else if (ManagePopup.currentPopup == "OpenMapPopup")
        {
            add.GetComponent<Button>().onClick.AddListener(delegate { add.GetComponent<MapData>().SelectData(); });
        }

        StreamReader read = new StreamReader(filePath);

        add.GetComponent<MapData>().mapName = read.ReadLine();
        add.GetComponent<MapData>().size = read.ReadLine();

        read.Close();

        add.GetComponent<MapData>().titleText.GetComponent<Text>().text = add.GetComponent<MapData>().mapName;
        add.GetComponent<MapData>().sizeText.GetComponent<Text>().text = add.GetComponent<MapData>().size;
    }

    public void GenerateMapList()
    {
        foreach (string fileName in Directory.GetFiles(Application.persistentDataPath + "/Maps"))
        {
            if (!fileName.Contains("_npc"))
            {
                AddOption(fileName, optionPrefab);
            }
        }
    }
}
