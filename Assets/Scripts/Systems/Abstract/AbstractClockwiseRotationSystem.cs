using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractClockwiseRotationSystem 
{
   protected void Rotate(Transform obj, float rotSpeed, float time)
    {
        obj.Rotate(Vector3.up * rotSpeed * time);
    }
}
