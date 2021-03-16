using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class LoadMap : MonoBehaviour
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

    public void OpenMap()
    {
        StreamReader input = new StreamReader(Application.persistentDataPath + "/Maps/" + MapData.LoadedName + ".txt");
        //StreamReader inputNPC = new StreamReader(Application.persistentDataPath + "/Maps/" + MapData.LoadedName + "_npc.txt");

        string name = /*MapData.LoadedName;*/input.ReadLine();

        string size = /*MapData.LoadedSize;*/input.ReadLine();

        string[] sizes = size.Split('X');

        gMan.DestroyDungeon();

        int dungeonX = gMan.dungeonX = int.Parse(sizes[0]);
        int dungeonY = gMan.dungeonY = int.Parse(sizes[1]);

        gMan.BuildDungeon();

        GameObject[,] dungeon = gMan.dungeon;

        for (int i = 0; i < dungeonX; i++)
        {
            string inp = input.ReadLine();

            string[] row = inp.Split(' ');

            //string inpNPC = inputNPC.ReadLine();

            //string[] rowNPC = inpNPC.Split(' ');

            for (int j = 0; j < dungeonY; j++)
            {
                gMan.ChangeType(((TileType)(int)Enum.Parse(typeof(TileAbreviation), row[j])).ToString(), i, j);
                /*if (rowNPC[j] != "null")
                {
                    gMan.PlaceNPC(row[j]);
                }*/
            }
        }

        input.Close();
        //inputNPC.Close();

        StreamReader inputNPC = new StreamReader(Application.persistentDataPath + "/Maps/" + MapData.LoadedName + "_npc.txt");

        for (int i = 0; i < dungeonX; i++)
        {
            string inpNPC = inputNPC.ReadLine();

            string[] rowNPC = inpNPC.Split(' ');

            for (int j = 0; j < dungeonY; j++)
            {
                if (rowNPC[j] != "null")
                {
                    gMan.selectedTile = dungeon[i, j];
                    gMan.PlaceNPC(rowNPC[j]);
                }
            }
        }

        inputNPC.Close();

        gameObject.GetComponent<ManagePopup>().ClosePopup();
    }
}
