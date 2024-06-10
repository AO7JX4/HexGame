using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample
{
    private Vector3 negPos;
    private Vector3 posPos;
    private bool isBetween=false;
    
    public Sample(Vector3 negativePosition, Vector3 positivePosition)
    {
        negPos = negativePosition;
        posPos = positivePosition;
    }

    public bool GetIsBetween()
    { return isBetween; }

    public void SetIsBetween(bool b)
    { isBetween = b; }
   
    public Vector3 GetNegativePosition()
    {
        return negPos;
    }

  
    public Vector3 GetPositivePosition()
    {
        return posPos;
    }
}
