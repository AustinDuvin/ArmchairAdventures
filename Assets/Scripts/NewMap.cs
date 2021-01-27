using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMap : MonoBehaviour
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

    public void CreateNewMap()
    {
        gameObject.GetComponent<ManagePopup>().ClosePopup();
        gMan.ResetDungeon();
    }
}
