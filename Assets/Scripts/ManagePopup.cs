using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagePopup : MonoBehaviour
{
    public GameObject popupPrefab;
    public Canvas gameCanvas;
    public int popupWidth;
    public int popupHeight;
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

    public void GeneratePopup()
    {
        GameObject popup = Instantiate(popupPrefab, gameCanvas.transform);
        if(popupWidth > 0 && popupHeight > 0)
        {
            popup.GetComponent<RectTransform>().sizeDelta = new Vector2(popupWidth, popupHeight);
        }
    }

    public void ClosePopup()
    {
        Destroy(transform.parent.gameObject);
    }

    public void NewMap()
    {
        ClosePopup();
        gMan.ResetDungeon();
    }
}
