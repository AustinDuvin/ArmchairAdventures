using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum playerClass
{
    Fighter,
    Rogue,
    Mage
}

public enum GameplayState
{
    none,
    moving,
    attacking
}

public class Player : MonoBehaviour
{
    private playerClass pClass;
    public GameplayState playState;
    public string displayName;
    public int hitPoints;
    public int speed;
    public int tempSpeed;
    public int remainingSpeed;
    public int magicPoints;
    public int defense;
    public GameObject position;
    public bool playerMoving;
    public int atkRange;
    public List<GameObject> targetList;
    public GameObject targetListUI;
    public GameObject gMan;
    public bool skillReady;
    public PlayerClass aClass;
    public int currentAttacks;

    static System.Random rand;

    // Start is called before the first frame update
    void Start()
    {
        pClass = playerClass.Fighter;
        //gameObject.AddComponent<FighterClass>();//new FighterClass();
        //aClass = gameObject.GetComponent<FighterClass>();
        aClass = new FighterClass();

        /*switch (pClass)
        {
            case playerClass.Fighter:
                aClass = new FighterClass();
                break;
            case playerClass.Rogue:
                break;
            case playerClass.Mage:
                break;
        }*/


        hitPoints = aClass.maxHitPoints;
        currentAttacks = aClass.numAttacks;
        playerMoving = false;
        speed = 5;
        remainingSpeed = 5;
        tempSpeed = 5;
        atkRange = 1;
        defense = 15;
        targetList = new List<GameObject>();
        skillReady = false;
        if (displayName == "")
        {
            displayName = "Player";
        }
        playState = GameplayState.none;

        rand = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int FollowPath(GameObject current, GameObject next, List<GameObject> path)
    {
        /*GameObject current = position;
        GameObject next = path[path.Count - 1];

        if(current != position)
        {
            return;
        }

        int i = path.Count;

        while (i >= 0)//for (int i = path.Count; i > 0; i--)
        {
            if (Vector3.Distance(transform.position, next.transform.position) < 0.001)
            {
                transform.position = next.transform.position;

                if (path[i - 1])
                {
                    //transform.position = next.transform.position;
                    next = path[i - 1];
                    i--;
                }

                //else
                //{
                    //return;
                //}
            }

            else
            {
                transform.Translate(Vector3.Normalize(next.transform.position - transform.position) * Time.deltaTime * 0.001f);
            }
        }*/

        int index = path.IndexOf(current);

        if (Vector3.Distance(transform.position, next.transform.position) < 0.001)
        {
            //transform.position = next.transform.position;
            transform.SetPositionAndRotation(next.transform.position, next.transform.rotation);
            remainingSpeed--;

            if (path[index - 1])
            {
                //transform.position = next.transform.position;
                next = path[index - 1];
                index--;
                position = path[index];
            }

            //else
            //{
            //return;
            //}
        }

        else
        {
            transform.Translate(Vector3.Normalize(next.transform.position - transform.position) * Time.deltaTime * 0.5f);
        }

        return index;
    }

    public void FindTargets(Graph graph)
    {
        targetListUI.GetComponent<PopulateList>().ClearList();

        //targetListUI.GetComponent<Text>().text = "";

        targetList = new List<GameObject>();

        List<GameObject> searchList = graph.FindShortestPath(position, atkRange);

        for(int i = 0; i < searchList.Count; i++)
        {
            if (searchList[i].GetComponent<Node>().ContainsEntity)
            {
                targetList.Add(searchList[i]);

                targetListUI.GetComponent<PopulateList>().AddOption(searchList[i].GetComponent<Node>().Occupant, gameObject);

                //targetListUI.GetComponent<Text>().text = targetListUI.GetComponent<Text>().text + searchList[i].GetComponent<Player>().displayName + "\n";
            }
        }

        /*for (int i = 0; i < targetList.Count; i++)
        {
            //targetListUI.GetComponent<Text>().text = targetListUI.GetComponent<Text>().text + targetList[i].GetComponent<Node>().Occupant.GetComponent<Player>().displayName + "\n";
            targetListUI.GetComponent<PopulateList>().AddOption(targetList[i].GetComponent<Node>().Occupant, gameObject);
        }*/
        
        List<Transform> tList = new List<Transform>();

        foreach (Transform child in targetListUI.transform)
        {
            tList.Add(child);
        }

        if (targetList.Count == 0)
        {
            //attackButton.SetActive(false);
            //attackButton.GetComponent<Button>().interactable = false;
        }

        else
        {
            //attackButton.SetActive(true);
            //attackButton.GetComponent<Button>().interactable = true;
        }
    }

    public void Attack(GameObject target)
    {
        //target.GetComponent<Player>().position.GetComponent<Node>().Occupant = null;
        //target.GetComponent<Player>().position.GetComponent<Node>().ContainsEntity = false;

        int atkRoll = rand.Next(1, 21);
        //gMan.GetComponent<GameManager>().DebugText.GetComponent<Text>().text = "Attack Roll: " + atkRoll;

        //Destroy(target);
        if (/*rand.Next(1, 21)*/atkRoll >= target.GetComponent<Player>().defense)
        {
            target.GetComponent<Player>().TakeDamage(rand.Next(1, aClass.dmgDie + 1));
        }

        //targetListUI.GetComponent<PopulateList>().ClearList();
        FindTargets(gMan.GetComponent<Graph>());

        currentAttacks--;

        gMan.GetComponent<GameManager>().ActiveMainMenu();
        gMan.GetComponent<GameManager>().targetMenu.SetActive(false);

        playState = GameplayState.none;
    }

    public void TakeDamage(int damage)
    {
        //gMan.GetComponent<GameManager>().DebugText.GetComponent<Text>().text += "Damage Roll: " + damage;

        hitPoints -= damage;

        if (hitPoints <= 0)
        {
            position.GetComponent<Node>().Occupant = null;
            position.GetComponent<Node>().ContainsEntity = false;
            Destroy(gameObject);
        }
    }

    public void ResetTurn()
    {
        FindTargets(gMan.GetComponent<Graph>());
        currentAttacks = aClass.numAttacks;
        remainingSpeed = speed;
        tempSpeed = speed;
        skillReady = false;
    }
}
