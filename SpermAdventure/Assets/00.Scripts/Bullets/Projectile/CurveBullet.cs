using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveBullet : Projectile
{
    [SerializeField]
    protected float _curvePower = 1;
    protected override void Move()
    {
        DirectionChange();
        base.Move();
    }

    protected virtual void DirectionChange()
    {
        _dir += Vector2.down * _curvePower;
    }
}
