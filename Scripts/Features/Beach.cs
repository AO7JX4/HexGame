using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beach : Environment
{
    public override void ProduceResource(Player p)
    {
        p.ProduceResource(1, 3, 1);
    }
}
