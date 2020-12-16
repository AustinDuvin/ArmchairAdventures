using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    private List<GameObject> vertices;
    private Dictionary<string, int> vertNameToIndex;
    private int[,] adjMatrix;
    private GameObject selectedVertex;

    public List<GameObject> Vertices { get { return vertices; } set { vertices = value; } }

    public Dictionary<string, int> VertNameToIndex { get { return vertNameToIndex; } set { vertNameToIndex = value; } }

    public int[,] AdjMatrix { get { return adjMatrix; } set { adjMatrix = value; } }

    public GameObject SelectedVertex { get { return selectedVertex; } set { selectedVertex = value; } }

    // Start is called before the first frame update
    void Start()
    {
        vertices = new List<GameObject>();
        vertNameToIndex = new Dictionary<string, int>();
        selectedVertex = null;

        // Set up the adjacency matrix
        adjMatrix = new int[GameObject.Find("GameManager").GetComponent<GameManager>().dungeonX * GameObject.Find("GameManager").GetComponent<GameManager>().dungeonY, GameObject.Find("GameManager").GetComponent<GameManager>().dungeonX * GameObject.Find("GameManager").GetComponent<GameManager>().dungeonY];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public void AddVertex(GameObject vert)
    {
        vertices.Add(vert);
    }*/

    public void AddDirectedEdge(int vert1, int vert2, int weight)
    {
        adjMatrix[vert1, vert2] = weight;
    }

    public void AddUndirectedEdge(int vert1, int vert2, int weight)
    {
        AddDirectedEdge(vert1, vert2, weight);
        AddDirectedEdge(vert2, vert1, weight);
    }

    public void ResetPaths()
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i].GetComponent<Node>().Reset();
        }
    }

    public void SelectVertex(GameObject start, GameObject end)
    {
        selectedVertex = start;

        if(selectedVertex != null)
        {
            FindShortestPath(start, end);
        }
    }

    public void SelectVertex(GameObject start, int distance)
    {
        selectedVertex = start;

        if (selectedVertex != null)
        {
            FindShortestPath(start, distance);
        }
    }

    public void FindShortestPath(GameObject start, GameObject end)
    {
        ResetPaths();

        int numPerm = 1;
        vertices[vertices.IndexOf(start)].GetComponent<Node>().Permanent = true;
        vertices[vertices.IndexOf(start)].GetComponent<Node>().Distance = 0;
        GameObject source = start;
        GameObject current = start;

        while (!end.GetComponent<Node>().Permanent)
        {
            int location = vertices.IndexOf(current);

            int temp = -1;

            for (int i = 0; i < adjMatrix.GetLength(1); i++)
            {
                if (adjMatrix[location, i] != 0 && !vertices[i].GetComponent<Node>().Permanent && vertices[location].GetComponent<Node>().Distance + adjMatrix[location, i] < vertices[i].GetComponent<Node>().Distance)
                {
                    vertices[i].GetComponent<Node>().Distance = current.GetComponent<Node>().Distance + adjMatrix[location, i];
                    vertices[i].GetComponent<Node>().PreviousVertex = current;
                }
            }

            int least = int.MaxValue;

            for (int i = 0; i < vertices.Count; i++)
            {
                int routeValue = vertices[i].GetComponent<Node>().Distance;
                if (routeValue < least && !vertices[i].GetComponent<Node>().Permanent)
                {
                    least = routeValue;
                    temp = i;
                }
            }

            if (temp > -1)
            {
                vertices[temp].GetComponent<Node>().Permanent = true;
                current = vertices[temp];
                current.GetComponent<Node>().Distance = least;
                numPerm++;
            }
        }
    }

    public List<GameObject> FindShortestPath(GameObject start, int distance)
    {
        List<GameObject> travelList = new List<GameObject>();

        ResetPaths();

        int numPerm = 1;

        /*for (int i = 0; i < vertices.Count; i++)
        {
            if (vertices[i].GetComponent<Node>().tileType == TileType.pillar)
            {
                vertices[i].GetComponent<Node>().Permanent = true;
                numPerm++;
            }
        }*/

        vertices[vertices.IndexOf(start)].GetComponent<Node>().Permanent = true;
        vertices[vertices.IndexOf(start)].GetComponent<Node>().Distance = 0;
        GameObject source = start;
        GameObject current = start;

        while (numPerm < vertices.Count)
        {
            int location = vertices.IndexOf(current);

            int temp = -1;

            for (int i = 0; i < adjMatrix.GetLength(1); i++)
            {
                if (adjMatrix[location, i] != 0 && !vertices[i].GetComponent<Node>().Permanent && vertices[location].GetComponent<Node>().Distance + adjMatrix[location, i] < vertices[i].GetComponent<Node>().Distance)
                {
                    vertices[i].GetComponent<Node>().Distance = current.GetComponent<Node>().Distance + adjMatrix[location, i];
                    vertices[i].GetComponent<Node>().PreviousVertex = current;
                }
            }

            int least = int.MaxValue;

            for (int i = 0; i < vertices.Count; i++)
            {
                int routeValue = vertices[i].GetComponent<Node>().Distance;
                if (routeValue < least && !vertices[i].GetComponent<Node>().Permanent)
                {
                    least = routeValue;
                    temp = i;
                }
            }

            if (temp > -1)
            {
                vertices[temp].GetComponent<Node>().Permanent = true;
                current = vertices[temp];
                current.GetComponent<Node>().Distance = least;
                numPerm++;
            }
        }

        for(int i = 0; i < vertices.Count; i++)
        {
            if(vertices[i].GetComponent<Node>().Distance != 0 && vertices[i].GetComponent<Node>().Distance <= distance)
            {
                travelList.Add(vertices[i]);
            }
        }

        return travelList;
    }
}
