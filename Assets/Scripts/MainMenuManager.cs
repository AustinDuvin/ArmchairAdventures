using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject currentPlayer;

    public GameObject moveButton;
    public GameObject attackButton;
    public GameObject spellButton;
    public GameObject skillButton;
    public GameObject endTurnButton;

    public List<GameObject> mainMenuButtons;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void populateButtonList()
    {
        mainMenuButtons.Add(moveButton);
        mainMenuButtons.Add(attackButton);
        mainMenuButtons.Add(spellButton);
        mainMenuButtons.Add(skillButton);
        mainMenuButtons.Add(endTurnButton);
    }

    public void changeActiveButton(GameObject btn, bool state)
    {
        btn.GetComponent<Button>().interactable = state;
    }
}
