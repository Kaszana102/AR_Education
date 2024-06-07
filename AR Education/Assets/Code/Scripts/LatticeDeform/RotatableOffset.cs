using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatableOffset : PositionOffset
{
    public float minYoffsetY = 0;
    public float maxYoffsetY = 0;

    protected override void Update()
    {
        base.Update();
        obj.rotation = Quaternion.Euler(0f, 0f, 0f);
        float angle = this.transform.rotation.eulerAngles.y;
        if (angle < 180)
        {
            float t = angle / 180;
            obj.position += new Vector3(0, Mathf.Lerp(minYoffsetY, maxYoffsetY, t),0); 
        }
        else
        {
            angle -= 180;
            float t = angle / 180;
            obj.position += new Vector3(0, Mathf.Lerp(maxYoffsetY, minYoffsetY, t), 0);
        }
    }
}
