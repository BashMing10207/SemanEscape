using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float _maxHp = 5, _hp = 5;
    protected virtual void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        _hp = _maxHp;
    }

    public virtual void ApplyDamage(float damage)
    {
        _hp -= damage;
        if (_hp <= 0 )
        {
            Dead();
        }

    }
    protected virtual void Dead()
    {
        _hp = 0;
    }
}
