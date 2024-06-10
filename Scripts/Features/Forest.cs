using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest : Environment
{
    public override void ProduceResource(Player p)
    {
        p.ProduceResource(3, 1, 1);
    }
}
