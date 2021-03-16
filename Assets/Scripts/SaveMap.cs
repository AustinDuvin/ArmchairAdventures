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

        if(!Directory.Exists(Application.persistentDataPath + "/Maps"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Maps");
        }

        if (name.GetComponent<Text>().text == "")
        {
            filePath = Application.persistentDataPath + "/Maps/NewMap";
            saveStream = File.CreateText(filePath + ".txt");
            //npcStream = File.CreateText(filePath + "_npc.txt");
        }

        else
        {
            filePath = Application.persistentDataPath + "/Maps/" + name.GetComponent<Text>().text;
            saveStream = File.CreateText(filePath + ".txt");
            //npcStream = File.CreateText(filePath + "_npc.txt");
        }

        int dungeonX = gMan.dungeonX;
        int dungeonY = gMan.dungeonY;

        saveStream.WriteLine(name.GetComponent<Text>().text);
        saveStream.WriteLine(dungeonX + "X" + dungeonY);

        //npcStream.WriteLine(name.GetComponent<Text>().text);
        //npcStream.WriteLine(dungeonX + "X" + dungeonY);

        for (int i = 0; i < dungeonX; i++)
        {
            for (int j = 0; j < dungeonY; j++)
            {
                TileType tMan = gMan.dungeon[i, j].GetComponent<Node>().tileType;
                saveStream.Write((TileAbreviation)((int)gMan.dungeon[i, j].GetComponent<Node>().tileType) + " ");

                /*if (gMan.dungeon[i, j].GetComponent<Node>().ContainsEntity)
                {
                    npcStream.Write(gMan.dungeon[i, j].GetComponent<Player>().displayName);
                }

                else
                {
                    npcStream.Write("null ");
                }*/
            }

            saveStream.Write("\n");
            //npcStream.Write("\n");
        }

        saveStream.Close();

        StreamWriter npcStream;

        if (name.GetComponent<Text>().text == "")
        {
            npcStream = File.CreateText(filePath + "_npc.txt");
        }

        else
        {
            npcStream = File.CreateText(filePath + "_npc.txt");
        }

        for (int i = 0; i < dungeonX; i++)
        {
            for (int j = 0; j < dungeonY; j++)
            {
                if (gMan.dungeon[i, j].GetComponent<Node>().ContainsEntity)
                {
                    npcStream.Write(gMan.dungeon[i, j].GetComponent<Node>().Occupant.GetComponent<Player>().displayName + " ");
                }

                else
                {
                    npcStream.Write("null ");
                }
            }

            npcStream.Write("\n");
        }

        npcStream.Close();

        gameObject.GetComponent<ManagePopup>().ClosePopup();
    }
}
