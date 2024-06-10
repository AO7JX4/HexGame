using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mountain : Environment
{
    public override void ProduceResource(Player p)
    {
        p.ProduceResource(1, 1, 3);
    }
}
