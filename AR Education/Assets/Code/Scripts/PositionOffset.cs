using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionOffset : MonoBehaviour
{

    public Transform obj;
    public Vector3 offset = Vector3.zero;
    

    // Update is called once per frame
    protected virtual void Update()
    {
        obj.position=transform.position+offset;
    }
}
