using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerClass : MonoBehaviour
{
    public int maxHitPoints;
    public int dmgDie;
    public int numAttacks;
    //public int tempAttacks;
    public bool isMagical;

    public PlayerClass()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public void Attack(GameObject target)
    {

    }*/

    public void CastSpell()
    {

    }

    public abstract void UseAbility();
}
