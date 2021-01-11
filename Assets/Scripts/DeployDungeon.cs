using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployDungeon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().dungeonVisualizer = this.gameObject;
        GameObject.Find("GameManager").GetComponent<GameManager>().BuildDungeon();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
