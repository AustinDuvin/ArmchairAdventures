using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum TileType
{
    normal,
    spikeTrap,
    pillar
}

public class Node : MonoBehaviour
{
    public TileType tileType;
    public GameObject normalTile;
    public GameObject spikePit;
    public GameObject pillarObs;
    private bool isHighlighted;
    private GameObject pathNode;

    public List<GameObject> neighbors;
    private Vector3 position;
    private int g;
    private int h;

    private int indexX;
    private int indexY;

    private bool start;
    private bool end;

    private bool isOpen;
    private bool isClosed;

    private int distance;
    private bool permanent;
    private GameObject previousVertex;

    private GameObject occupant;

    private bool containsEntity;

    public Vector3 Position { get { return position; } set { position = value; } }

    public int G { get { return g; } set { g = value; } }

    public int H { get { return h; } set { h = value; } }

    public int IndexX { get { return indexX; } set { indexX = value; } }

    public int IndexY { get { return indexY; } set { indexY = value; } }

    public bool StartV { get { return start; } set { start = value; } }

    public bool EndV { get { return end; } set { end = value; } }

    public bool IsOpen { get { return isOpen; } set { isOpen = value; } }

    public bool IsClosed { get { return isClosed; } set { isClosed = value; } }

    public bool IsHighlighted { get { return isHighlighted; } set { isHighlighted = value; } }

    public GameObject PathNode { get { return pathNode; } set { pathNode = value; } }

    public int Distance { get { return distance; } set { distance = value; } }

    public bool Permanent { get { return permanent; } set { permanent = value; } }

    public GameObject PreviousVertex { get { return previousVertex; } set { previousVertex = value; } }

    public bool ContainsEntity { get { return containsEntity; } set { containsEntity = value; } }

    public GameObject Occupant { get { return occupant; } set { occupant = value; } }

    // Start is called before the first frame update
    void Start()
    {
        Position = transform.position;
        tileType = TileType.normal;
        IsHighlighted = false;
        ContainsEntity = false;
    }

    // Update is called once per frame
    void Update()
    {
        //DisplayTile();
    }

    public void findNeighbors(GameObject[,] dungeon/*, GameObject DebugText*/)
    {
        if (indexX > 0)
        {
            neighbors.Add(dungeon[indexX - 1, indexY]);
        }

        if (indexY > 0)
        {
            neighbors.Add(dungeon[indexX, indexY - 1]);
        }

        if (indexX < dungeon.GetLength(0) - 1)
        {
            neighbors.Add(dungeon[indexX + 1, indexY]);
        }

        if (indexY < dungeon.GetLength(1) - 1)
        {
            neighbors.Add(dungeon[indexX, indexY + 1]);
        }
    }

    public void DisplayTile()
    {
        if (tileType == TileType.normal)
        {
            normalTile.SetActive(true);
            spikePit.SetActive(false);
            pillarObs.SetActive(false);
        }

        else if (tileType == TileType.spikeTrap)
        {
            normalTile.SetActive(false);
            spikePit.SetActive(true);
            pillarObs.SetActive(false);
        }

        else if (tileType == TileType.pillar)
        {
            normalTile.SetActive(false);
            spikePit.SetActive(false);
            pillarObs.SetActive(true);
        }
    }

    public void SetHG(GameObject goal, GameObject start)
    {
        H = Math.Abs(goal.GetComponent<Node>().IndexX - IndexX) + Math.Abs(goal.GetComponent<Node>().IndexY - IndexY);
        G = Math.Abs(start.GetComponent<Node>().IndexX - IndexX) + Math.Abs(start.GetComponent<Node>().IndexY - IndexY);
    }

    public void Reset()
    {
        distance = int.MaxValue;
        permanent = false;
        previousVertex = null;
    }
}
