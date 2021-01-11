using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum gameState
{
    placing,
    rotating,
    building,
    playing
}

public enum playerState
{
    dm,
    player
}

public delegate void initDelegate();

public class GameManager : MonoBehaviour
{
    private gameState gState;
    private playerState pState;

    private ARSessionOrigin arOrigin;
    private Vector3 dungeonLocation;
    private Quaternion dungeonRotation;
    public int dungeonX;
    public int dungeonY;
    public GameObject[,] dungeon;
    private List<GameObject> vertices;
    private List<GameObject> closed;
    private List<GameObject> path;
    private List<GameObject> travelList;
    //private Dictionary
    private GameObject start;
    private GameObject end;
    private PriorityQueue openQueue;
    private bool pathSearching;
    GameObject current;

    public GameObject floorTile;
    public GameObject rotateText;
    public GameObject placeText;
    public GameObject confirmButton;
    public GameObject DebugText;
    public GameObject selectedTile;
    public GameObject previousSelectedTile;
    public bool shouldMove;
    public GameObject tileMenu;
    public GameObject npcMenu;
    public GameObject tileButton;
    public GameObject npcButton;
    public GameObject playerPrefab;
    public GameObject player1;
    public List<GameObject> entityList;
    public GameObject localUI;
    public GameObject mainMenu;
    public GameObject targetMenu;
    public GameObject targetMenuText;

    //debug buttons
    public GameObject startButton;
    public GameObject endButton;
    public GameObject findButton;

    public GameObject dungeonVisualizer;

    private int moveIndex;
    
    private MainMenuManager menuMngr;

    

    //public initDelegate initDel = 

    // Start is called before the first frame update
    void Start()
    {
        gState = gameState.placing;
        pState = playerState.dm;
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        //dungeonVisualizer.SetActive(false);
        dungeonRotation = new Quaternion();
        dungeonX = 10;
        dungeonY = 10;
        //BuildDungeon();
        //placeText.SetActive(true);
        pathSearching = false;
        path = new List<GameObject>();
        travelList = new List<GameObject>();
        
        shouldMove = false;
        //menuMngr = mainMenu.GetComponent<MainMenuManager>();
        //arOrigin.GetComponent<ARTrackedImageManager>().trackedImagesChanged += ConfirmRotation;
    }

    // Update is called once per frame
    void Update()
    {
        /*if(mainMenu && !menuMngr)
        {
            menuMngr = mainMenu.GetComponent<MainMenuManager>();
            menuMngr.currentPlayer = player1;
            menuMngr.populateButtonList();
        }*/

        /*if (gState == gameState.placing)
        {
            FindDungeonLocation();
        }*/

        //FindDungeonLocation();

        /*if (gState == gameState.rotating)
        {
            RotateDungeon();
        }*/

        if(dungeonVisualizer != null)
        {
            ConfirmRotation();
        }

        if (player1)
        {
            if (gState == gameState.building)
            {
                if (player1.activeSelf && localUI.activeSelf)
                {
                    localUI.transform.position = Camera.current.WorldToScreenPoint(player1.transform.position) + new Vector3(200, 500);
                }

                // Handles input from touch
                if (Input.touchCount > 0)
                {
                    RaycastHit hit;
                    Vector2 tLoc = Input.GetTouch(0).position;

                    // Raycasts from point on screen where first touch this frame is detected
                    if (Physics.Raycast(Camera.current.ScreenPointToRay(tLoc), out hit) && !EventSystem.current.IsPointerOverGameObject(0)/* && GraphicRaycaster.BlockingObjects.All*/)
                    {
                        // Dehighlights previously selected tile
                        if (selectedTile && selectedTile != start && selectedTile != end)
                        {
                            DehighlightTiles(selectedTile);
                        }

                        if (Input.GetTouch(0).phase == TouchPhase.Began)
                        {
                            previousSelectedTile = selectedTile;
                        }

                        if (previousSelectedTile && previousSelectedTile == hit.collider.gameObject.GetComponentInParent<Node>().gameObject)
                        {
                            shouldMove = true;
                        }

                        // Sets currently selected tile to whichever one was pressed.
                        selectedTile = hit.collider.gameObject.GetComponentInParent<Node>().gameObject;

                        // If start is marked, the currenly selected tile is within the maximum travel distance from
                        // the start tile, and the selcted tile is not a pillar, search for the shortest path to the
                        // start tile to the selected tile
                        if (start && travelList.Contains(selectedTile) && selectedTile.GetComponent<Node>().tileType != TileType.pillar && !player1.GetComponent<Player>().playerMoving)
                        {
                            FindEnd();
                            StartSearch();
                        }
                    }

                    if (shouldMove/*Input.GetTouch(0).deltaTime > 0 && Input.GetTouch(0).deltaTime < 0.5 && Input.GetTouch(0).phase == TouchPhase.Began && Input.GetTouch(0).deltaPosition.magnitude < 1.0f*/)
                    {
                        MovePlayer();
                        shouldMove = false;
                        previousSelectedTile = null;
                    }
                }

                // Dehighlights all tiles every frame in case something changed
                for (int i = 0; i < vertices.Count; i++)
                {
                    if (vertices[i].GetComponent<Node>().IsHighlighted)
                    {
                        DehighlightTiles(vertices[i]);
                    }
                }

                // Find the area that can be traveled to from the start
                if (start && player1.GetComponent<Player>().playState == GameplayState.moving)
                {
                    CalculateTravelArea(player1.GetComponent<Player>().tempSpeed);
                }

                // Highlight all tiles
                HighlightTiles(selectedTile);
                HighlightTiles(selectedTile, Color.blue);
                for (int i = 0; i < path.Count; i++)
                {
                    TileType temp = path[i].GetComponent<Node>().tileType;
                    HighlightTiles(path[i], Color.yellow);
                }
                HighlightTiles(start, Color.blue);
                HighlightTiles(end, Color.red);

                if (player1.GetComponent<Player>().playerMoving)
                {
                    moveIndex = player1.GetComponent<Player>().FollowPath(path[moveIndex], path[moveIndex - 1], path);
                }

                if (Vector3.Distance(player1.transform.position, end.transform.position) < 0.001)
                {
                    player1.GetComponent<Player>().playState = GameplayState.none;
                    player1.transform.SetPositionAndRotation(end.transform.position, end.transform.rotation);//player1.transform.position = end.transform.position;
                    player1.GetComponent<Player>().playerMoving = false;
                    player1.GetComponent<Player>().position = end;
                    end.GetComponent<Node>().Occupant = player1;
                    start.GetComponent<Node>().Occupant = null;
                    start.GetComponent<Node>().ContainsEntity = false;
                    start = end;
                    end = new GameObject();
                    start.GetComponent<Node>().Occupant = player1;
                    start.GetComponent<Node>().ContainsEntity = true;
                    player1.GetComponent<Player>().remainingSpeed--;
                    player1.GetComponent<Player>().tempSpeed = player1.GetComponent<Player>().remainingSpeed;
                    CalculateTravelArea(player1.GetComponent<Player>().tempSpeed);
                    travelList = new List<GameObject>();
                    path = new List<GameObject>();
                    player1.GetComponent<Player>().FindTargets(gameObject.GetComponent<Graph>());
                    localUI.SetActive(true);
                    ActiveMainMenu();

                    /*for (int i = 0; i < menuMngr.mainMenuButtons.Count; i++)
                    {
                        if (menuMngr.mainMenuButtons[i].GetComponent<Button>())
                        {
                            menuMngr.DeactivateButton(menuMngr.mainMenuButtons[i]);
                        }
                    }*/
                }

                /*if (player1.activeSelf && localUI.activeSelf)
                {
                    localUI.transform.position = Camera.current.WorldToScreenPoint(player1.transform.position) + new Vector3(410, 610);
                }*/

            }

            else if (gState == gameState.playing)
            {

            }

        }

        //localUI.transform.position = Camera.current.WorldToScreenPoint(player1.transform.position) + new Vector3(410, 610);
    }

    // Uses ground planes to find a place in AR space
    // to place the map.  Will probably change this to
    // recognize a QR code at some point for better
    // consistency and less bugs
    private void FindDungeonLocation()
    {
        /*Vector3 center = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        bool validPlacement = arOrigin.GetComponent<ARRaycastManager>().Raycast(center, hits, TrackableType.Planes);*/
        GameObject imageDetected = GameObject.Find("ImagePoint");

        if (imageDetected/*validPlacement*/)
        {
            if(dungeonVisualizer.activeSelf == true)
            {
                return;
            }

            /*dungeonVisualizer.SetActive(true);
            dungeonLocation = hits[0].pose.position;
            dungeonVisualizer.transform.SetPositionAndRotation(dungeonLocation, dungeonRotation);*/
            dungeonVisualizer.SetActive(true);
            dungeonLocation = imageDetected.transform.position;
            dungeonVisualizer.transform.SetPositionAndRotation(dungeonLocation, dungeonRotation);
        }

        else
        {
            dungeonVisualizer.SetActive(false);
        }
    }

    // Builds the dungeon map out of tile
    // prefabs.  Sets up neighbor relations
    // between tiles
    public void BuildDungeon()
    {
        float tileSize = 0.1f;

        dungeon = new GameObject[dungeonX, dungeonY];
        vertices = new List<GameObject>();

        float offsetX = (float)(dungeonX / 2) * tileSize * -1.0f;
        Debug.Log("offsetX: " + offsetX);
        float offsetY = (float)(dungeonY / 2) * tileSize * -1.0f;

        for (int i = 0; i < dungeonX; i++)
        {
            for (int j = 0; j < dungeonY; j++)
            {
                GameObject Vert = (Instantiate(floorTile, new Vector3(offsetX + (tileSize * i) + (tileSize / 2), 0.0f, offsetY + (tileSize * j) + (tileSize / 2)), new Quaternion(), dungeonVisualizer.transform));
                Vert.GetComponent<Node>().Position = Vert.transform.position;
                dungeon[i, j] = Vert;
                Vert.GetComponent<Node>().IndexX = i;
                Vert.GetComponent<Node>().IndexY = j;
                vertices.Add(Vert);
            }
        }

        //dungeonVisualizer.transform.localScale *= 0.5f;

        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i].GetComponent<Node>().findNeighbors(dungeon);
        }
        
        gameObject.GetComponent<Graph>().Vertices = vertices;

        player1 = Instantiate(playerPrefab, dungeonVisualizer.transform);
        player1.GetComponent<Player>().gMan = gameObject;
        player1.SetActive(false);
        entityList = new List<GameObject>();
        entityList.Add(player1);
        player1.GetComponent<Player>().targetListUI = targetMenu;// Text;
        localUI.transform.position = Camera.current.WorldToScreenPoint(player1.transform.position) + new Vector3(100, 200);

        if (mainMenu && !menuMngr)
        {
            menuMngr = mainMenu.GetComponent<MainMenuManager>();
            menuMngr.currentPlayer = player1;
            menuMngr.populateButtonList();
        }
    }

    // Locks the location of the dungeon and moves
    // to rotation phase
    private void PlaceDungeon()
    {
        if (dungeonVisualizer.activeInHierarchy)
        {
            gState = gameState.rotating;
            //placeText.SetActive(false);
            //rotateText.SetActive(true);
        }
    }

    // Handles rotating dungeon map
    private void RotateDungeon()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                float rotationMag = touch.deltaPosition.x;

                dungeonVisualizer.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), rotationMag);
            }
        }
    }

    // Ends rotation phase and moves to the build phase
    // where the dungeon/game master can edit the layout
    // of the dungeon
    private void ConfirmRotation()
    {
        gState = gameState.building;
        //rotateText.SetActive(false);
        confirmButton.SetActive(false);
        tileMenu.SetActive(true);
        tileButton.SetActive(true);
        npcButton.SetActive(true);
    }

    // Ends rotation phase and moves to the build phase
    // where the dungeon/game master can edit the layout
    // of the dungeon
    private void ConfirmRotation(ARTrackedImagesChangedEventArgs eventArgs)
    {
        gState = gameState.building;
        //rotateText.SetActive(false);
        confirmButton.SetActive(false);
        tileMenu.SetActive(true);
        tileButton.SetActive(true);
        npcButton.SetActive(true);
    }

    // Manages input from a confirmation button
    public void ConfirmButtonMngr()
    {
        if (gState == gameState.placing)
        {
            PlaceDungeon();
        }

        else if (gState == gameState.rotating)
        {
            ConfirmRotation();
        }
    }

    // Changes the type of the currently selected tile
    public void ChangeType(string type)
    {
        if (selectedTile)
        {
            selectedTile.GetComponent<Node>().tileType = (TileType)Enum.Parse(typeof(TileType), type);
            selectedTile.GetComponent<Node>().DisplayTile();
        }
    }

    // Marks the start tile for traveling
    public void FindStart()
    {
        start = selectedTile;
        travelList = new List<GameObject>();
        start.GetComponent<Node>().StartV = true;
        start.GetComponent<Node>().normalTile.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
        PlacePlayer();
        startButton.SetActive(false);
    }

    // Marks the end tile for movement
    public void FindEnd()
    {
        if (start != selectedTile)
        {
            end = selectedTile;
            end.GetComponent<Node>().EndV = true;
            end.GetComponent<Node>().normalTile.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }
    }

    // Searches for the shortest route between the start and end tiles
    public void StartSearch()
    {
        gameObject.GetComponent<Graph>().Vertices = vertices;

        gameObject.GetComponent<Graph>().AdjMatrix = new int[dungeonX * dungeonY, dungeonX * dungeonY];

        for(int i = 0; i < vertices.Count; i++)
        {
            for(int j = 0; j < vertices[i].GetComponent<Node>().neighbors.Count; j++)
            {
                if (vertices[i].GetComponent<Node>().neighbors[j].GetComponent<Node>().tileType != TileType.pillar && vertices[i].GetComponent<Node>().tileType != TileType.pillar)
                {
                    gameObject.GetComponent<Graph>().AddUndirectedEdge(i, vertices.IndexOf(vertices[i].GetComponent<Node>().neighbors[j]), 1);
                }
            }
        }

        GameObject current = end.GetComponent<Node>().PreviousVertex;

        path = new List<GameObject>();

        path.Add(end);

        while (current != start)
        {
            path.Add(current);
            current = current.GetComponent<Node>().PreviousVertex;
        }

        path.Add(start);

        moveIndex = path.Count - 1;
    }
    
    // Calculates the area that can be traveled to from the start tile
    // using Dijkstra's Shortest Path Algorithm
    private void CalculateTravelArea(int speed)
    {
        gameObject.GetComponent<Graph>().Vertices = vertices;

        gameObject.GetComponent<Graph>().AdjMatrix = new int[dungeonX * dungeonY, dungeonX * dungeonY];

        for (int i = 0; i < vertices.Count; i++)
        {
            for (int j = 0; j < vertices[i].GetComponent<Node>().neighbors.Count; j++)
            {
                if (vertices[i].GetComponent<Node>().neighbors[j].GetComponent<Node>().tileType != TileType.pillar && vertices[i].GetComponent<Node>().tileType != TileType.pillar)
                {
                    gameObject.GetComponent<Graph>().AddUndirectedEdge(i, vertices.IndexOf(vertices[i].GetComponent<Node>().neighbors[j]), 1);
                }

                else if (vertices[i].GetComponent<Node>().neighbors[j].GetComponent<Node>().tileType == TileType.pillar || vertices[i].GetComponent<Node>().tileType == TileType.pillar)
                {
                    gameObject.GetComponent<Graph>().AddUndirectedEdge(i, vertices.IndexOf(vertices[i].GetComponent<Node>().neighbors[j]), 1000000);
                }

                /*
                if (vertices[i].GetComponent<Node>().neighbors[j].GetComponent<Node>().tileType == TileType.normal && vertices[i].GetComponent<Node>().tileType != TileType.pillar)
                {
                    gameObject.GetComponent<Graph>().AddUndirectedEdge(i, vertices.IndexOf(vertices[i].GetComponent<Node>().neighbors[j]), 1);
                }

                else if (vertices[i].GetComponent<Node>().neighbors[j].GetComponent<Node>().tileType == TileType.spikeTrap && vertices[i].GetComponent<Node>().tileType != TileType.pillar)
                {
                    gameObject.GetComponent<Graph>().AddDirectedEdge(i, vertices.IndexOf(vertices[i].GetComponent<Node>().neighbors[j]), 10);
                    gameObject.GetComponent<Graph>().AddDirectedEdge(vertices.IndexOf(vertices[i].GetComponent<Node>().neighbors[j]), i, 1);
                }
                */
            }
        }

        travelList = gameObject.GetComponent<Graph>().FindShortestPath(start, speed);

        for (int i = 0; i < travelList.Count; i++)
        {
            HighlightTiles(travelList[i], Color.green);
        }
    }
    
    // Highlights a tile green
    public void HighlightTiles(GameObject tile)
    {
        tile.GetComponent<Node>().normalTile.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        tile.GetComponent<Node>().spikePit.GetComponentInChildren<Renderer>().material.SetColor("_Color", new Color(0.0f, 1.0f, 0.0f, 0.5f));
        tile.GetComponent<Node>().pillarObs.GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.green);

        tile.GetComponent<Node>().IsHighlighted = true;
    }

    // Highlights a tile whatever color is passed in.
    public void HighlightTiles(GameObject tile, Color color)
    {
        tile.GetComponent<Node>().normalTile.GetComponent<Renderer>().material.SetColor("_Color", color);
        tile.GetComponent<Node>().spikePit.GetComponentInChildren<Renderer>().material.SetColor("_Color", new Color(1.0f, 1.0f, 1.0f, 0.5f) * color);
        tile.GetComponent<Node>().pillarObs.GetComponentInChildren<Renderer>().material.SetColor("_Color", color);

        tile.GetComponent<Node>().IsHighlighted = true;
    }

    // Resets any highlight tile
    public void DehighlightTiles(GameObject tile)
    {
        tile.GetComponent<Node>().normalTile.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        tile.GetComponent<Node>().spikePit.GetComponentInChildren<Renderer>().material.SetColor("_Color", new Color(1.0f, 1.0f, 1.0f, 0.5f));
        tile.GetComponent<Node>().pillarObs.GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.white);

        tile.GetComponent<Node>().IsHighlighted = false;
    }

    // Switches to Dungeon/Game Master mode
    public void SwitchToDM()
    {
        pState = playerState.dm;

        startButton.SetActive(false);
        //findButton.SetActive(false);

        tileMenu.SetActive(true);
        npcMenu.SetActive(false);
        tileButton.SetActive(true);
        npcButton.SetActive(true);
    }

    // Switches to Player mode
    public void SwitchToPlayer()
    {
        pState = playerState.player;

        tileMenu.SetActive(false);
        npcMenu.SetActive(false);
        tileButton.SetActive(false);
        npcButton.SetActive(false);

        if (!start)
        {
            startButton.SetActive(true);
        }
        //findButton.SetActive(true);
    }

    private void PlacePlayer()
    {
        player1.transform.SetPositionAndRotation(start.transform.position, start.transform.rotation);
        player1.GetComponent<Player>().position = start;
        start.GetComponent<Node>().Occupant = player1;
        start.GetComponent<Node>().ContainsEntity = true;
        player1.SetActive(true);
        localUI.SetActive(true);
    }

    public void MovePlayer()
    {
        if (!player1.GetComponent<Player>().playerMoving)
        {
            player1.GetComponent<Player>().playerMoving = true;
        }
        
    }

    public void PlaceNPC(GameObject type)
    {
        if (selectedTile)
        {
            GameObject npc = GameObject.Instantiate(type, dungeonVisualizer.transform);
            npc.GetComponent<Player>().position = selectedTile;
            npc.GetComponent<Player>().position.GetComponent<Node>().ContainsEntity = true;
            selectedTile.GetComponent<Node>().Occupant = npc;

            npc.transform.SetPositionAndRotation(selectedTile.transform.position, selectedTile.transform.rotation);

            entityList.Add(npc);
        }
    }

    public void SwitchUI(GameObject element)
    {
        /*string[] types = type.Split(',');

        GameObject[] uiOfType = GameObject.FindGameObjectsWithTag(types[1]);

        for (int i = 0; i < uiOfType.Length; i++)
        {
            uiOfType[i].SetActive(false);
        }

        GameObject.Find(types[0]).SetActive(true);*/

        tileMenu.SetActive(false);
        npcMenu.SetActive(false);

        element.SetActive(true);
    }

    public void MovingPhase()
    {
        localUI.SetActive(false);

        player1.GetComponent<Player>().playState = GameplayState.moving;
    }

    public void AttackingPhase()
    {
        mainMenu.SetActive(false);
        targetMenu.SetActive(true);

        player1.GetComponent<Player>().playState = GameplayState.attacking;
    }

    // Not final implementation
    // only for testing
    public void EndTurn()
    {
        player1.GetComponent<Player>().ResetTurn();
        ActiveMainMenu();
    }

    public void ActiveMainMenu()
    {
        mainMenu.SetActive(true);

        if (player1.GetComponent<Player>().remainingSpeed == 0)
        {
            menuMngr.changeActiveButton(menuMngr.moveButton, false);
        }

        else
        {
            menuMngr.changeActiveButton(menuMngr.moveButton, true);
        }

        if (player1.GetComponent<Player>().targetList.Count == 0 || player1.GetComponent<Player>().currentAttacks == 0)
        {
            menuMngr.changeActiveButton(menuMngr.attackButton, false);
        }

        else
        {
            menuMngr.changeActiveButton(menuMngr.attackButton, true);
        }

        if (!player1.GetComponent<Player>().aClass.isMagical)
        {
            menuMngr.changeActiveButton(menuMngr.spellButton, false);
        }

        else
        {
            menuMngr.changeActiveButton(menuMngr.spellButton, true);
        }

        if (!player1.GetComponent<Player>().skillReady)
        {
            menuMngr.changeActiveButton(menuMngr.skillButton, false);
        }

        else
        {
            menuMngr.changeActiveButton(menuMngr.skillButton, true);
        }
    }
}
