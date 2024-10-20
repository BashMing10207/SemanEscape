using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

public class Hook : MonoBehaviour
{
    [SerializeField]
    LineRenderer _visual;
    //Vector3 _HookedPos;
    [SerializeField]
    Rigidbody2D _Rigidbody;
    Vector3[] _vArr;
    [SerializeField]
    float _maxDis, _height = 7;
    [SerializeField]
    AnimationCurve _curve,_hookedCurv,_lerpCurve;
    [SerializeField]
    float _speed=5f,_accelation,_maxSpeed=25f;
    [SerializeField]
    LayerMask whatisObstacle;

    [SerializeField]
    int _resolution;
    float _time=0,_hookedWeight=0;
    [HideInInspector]
    public bool isRopeEnabled=false,isHookHited=false;

    [SerializeField]
    Vector2 _direction,_hookPos;
    public Vector2 hookedPos;
    bool _isFireable = true;
    void Init()
    {
        _vArr = new Vector3[_resolution];
        _isFireable=true;
    }

    private void Start()
    {
        Init();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && _isFireable)
        {
            //_HookedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _hookPos = transform.position;
            isHookHited = false;
            _direction = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition)- (Vector2)(transform.position)).normalized;
            _time = 0;
            _isFireable = false;
            Invoke(nameof(FireAble), 0.4f);
        }
        if (Input.GetKey(KeyCode.Mouse1) && isHookHited && isRopeEnabled)
        {
            if (_maxDis > 0.5f)
            {
            Vector3 movedir = (Vector2)(hookedPos - (Vector2)transform.position).normalized * _accelation;
            float speed = Mathf.Lerp(1, 0, (Vector3.Project(movedir, (Vector3)_Rigidbody.velocity) + (Vector3)_Rigidbody.velocity).magnitude / _maxSpeed);
            _Rigidbody.AddForce(movedir.normalized*speed*_accelation,ForceMode2D.Impulse);

                _maxDis = Vector3.Distance(hookedPos, transform.position);
            }
            
        }

        _visual.enabled = Input.GetKey(KeyCode.Mouse0);
    }

    void FireAble()
    {
    _isFireable = true; 
    }
    private void FixedUpdate()
    {
        isRopeEnabled = Input.GetKey(KeyCode.Mouse0);
        if (isRopeEnabled)
        {
            if(isHookHited)
            {
                RopePhysics();
            }
            else
            {
                ProjectileCast();
            }

            SetVertices();
        }
    }

    private void ProjectileCast()
    {
        RaycastHit2D hitinfo = Physics2D.Raycast(_hookPos, _direction, _speed * Time.fixedDeltaTime, whatisObstacle);
        if (hitinfo)
        {
            OnProjectileHit(hitinfo.point);
        }
        else
        {
            _hookPos += _direction * _speed * Time.fixedDeltaTime;
        }
        _direction += Vector2.down *Time.fixedDeltaTime / 15;
        
    }

    void OnProjectileHit(Vector2 pos)
    {
        _hookPos = pos;
        hookedPos = pos;
        _maxDis = Vector3.Distance(hookedPos, transform.position);
        isHookHited = true;
        _time -= 0.2f;
    }

    void SetVertices()
    {
        //if (_isHookHited)
            //_time = Mathf.Lerp(_time, 1, 10*Time.fixedDeltaTime);

        if (_time <= 1)
        {
            _time += Time.fixedDeltaTime * 6;
        }
        else
            _time = 1;


        float heightPerTime = _lerpCurve.Evaluate(_time);
        for (int i = 0; i < _resolution; i++)
        {
            float height = _curve.Evaluate(((float)i) / _resolution);

            float rad = Mathf.Deg2Rad * (Vector2.SignedAngle(transform.right, _hookPos - (Vector2)transform.position) + 90);
            _vArr[i] = Vector2.Lerp(transform.position, _hookPos, ((float)i) / _resolution) + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * (1 - height) *heightPerTime *_height;
        }

        _visual.SetPositions(_vArr);
    }

    void RopePhysics()
    {
        Vector2 hookdir = hookedPos - (Vector2)(transform.position);
        //hookdir = _HookedPos - (transform.position) + hookdir;
        float distance = Vector2.Distance(hookedPos, transform.position + Vector3.Project(_Rigidbody.velocity, hookdir));

        if (distance > _maxDis)
        {
            _Rigidbody.velocity += (Vector2)(hookedPos - (Vector2)(transform.position)).normalized * (distance - _maxDis + 1);
        }
    }
}
