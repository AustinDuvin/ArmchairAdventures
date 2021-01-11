using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterClass : PlayerClass
{
    public FighterClass()
    {
        maxHitPoints = 20;
        dmgDie = 8;
        numAttacks = 2;
        //tempAttacks = 2;
        isMagical = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        maxHitPoints = 20;
        dmgDie = 8;
        numAttacks = 2;
        //tempAttacks = 2;
        isMagical = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UseAbility()
    {
        
    }
}
