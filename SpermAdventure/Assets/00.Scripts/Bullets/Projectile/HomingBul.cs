using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBul : CurveBullet
{
    protected override void DirectionChange()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _dir += (mousePos - (Vector2)transform.position) * _curvePower * Time.fixedDeltaTime;
    }

}
