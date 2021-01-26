using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeployDungeon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().dungeonVisualizer = this.gameObject;
        GameObject.Find("GameManager").GetComponent<GameManager>().BuildDungeon();

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(2))
        {
            GameObject.Find("NewButton").GetComponent<Button>().interactable = true;
            GameObject.Find("LoadButton").GetComponent<Button>().interactable = true;
            GameObject.Find("SaveButton").GetComponent<Button>().interactable = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
