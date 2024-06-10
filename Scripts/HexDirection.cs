using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexDirection 
{
    

   public static HexCoordinates[] directionVectors = {
        new HexCoordinates(0,1),
        new HexCoordinates(1,0),
        new HexCoordinates(1,-1),
        new HexCoordinates(0,-1),
        new HexCoordinates(-1,0),
        new HexCoordinates(-1,1),

    };

}
