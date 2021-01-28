using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class SaveMap : MonoBehaviour
{
    private GameManager gMan;

    // Start is called before the first frame update
    void Start()
    {
        gMan = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveCurrentMap(GameObject name)
    {
        StreamWriter saveStream;
        string filePath;

        if (name.GetComponent<Text>().text == "")
        {
            filePath = Application.persistentDataPath + "/NewMap.txt";

            //if (!File.Exists(filePath))
            //{
                saveStream = File.CreateText(filePath);
            //}

            //else
            //{
                //saveStream = new StreamWriter(Application.persistentDataPath + "/NewMap.txt");
            //}
        }

        else
        {
            filePath = Application.persistentDataPath + "/" + name.GetComponent<Text>().text + ".txt";

            //if (!File.Exists(filePath))
            //{
                saveStream = File.CreateText(filePath);
            //}

            //else
            //{
                //saveStream = new StreamWriter(Application.persistentDataPath + "/NewMap.txt");
            //}
        }

        int dungeonX = gMan.dungeonX;
        int dungeonY = gMan.dungeonY;
        GameObject[,] dungeon = gMan.dungeon;

        saveStream.WriteLine(dungeonX + "X" + dungeonY);

        for (int i = 0; i < dungeonX; i++)
        {
            for (int j = 0; j < dungeonY; j++)
            {
                saveStream.Write((TileAbreviation)((int)dungeon[i, j].GetComponent<Node>().tileType) + " ");
            }

            saveStream.Write("\n");
        }

        saveStream.Close();

        gameObject.GetComponent<ManagePopup>().ClosePopup();
    }
}
