using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateList : MonoBehaviour
{
    public GameObject optionPrefab;
    public List<GameObject> optionList;
    //public Text optionText;

    // Start is called before the first frame update
    void Start()
    {
        optionList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddOption(GameObject target, GameObject seeker)
    {
        GameObject gObj = GameObject.Instantiate(optionPrefab, new Vector3(0.0f, 0.0f/*(-100.0f * optionList.Count) - 50.0f*/, 0.0f), new Quaternion(), gameObject.transform);
        optionList.Add(gObj);
        //gObj.transform.position = new Vector3(0.0f, (-100.0f * optionList.Count) - 50.0f, 0.0f);
        gObj.GetComponent<RectTransform>().localPosition = new Vector3(0.0f, 450 - (100.0f * optionList.Count)/*(100.0f * optionList.Count) - 50.0f*/, 0.0f);//.anchorMax = new Vector2(0.0f, 0.0f);

        gObj.GetComponentInChildren<Text>().text = target.GetComponent<Player>().displayName;

        gObj.GetComponent<Button>().onClick.AddListener(delegate { seeker.GetComponent<Player>().Attack(target); });
    }

    public void ClearList()
    {
        /*for(int i = 0; i < optionList.Count; i++)
        {
            Destroy(optionList[i]);
        }*/

        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }

        optionList = new List<GameObject>();
    }
}
