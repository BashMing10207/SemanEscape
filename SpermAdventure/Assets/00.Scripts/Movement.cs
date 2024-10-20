using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] float _speed,_jumpPower,_radius,_groundDis,_maxSpeed;
    [SerializeField] LayerMask _whatIsGround;
    Collider2D[] _collider = new Collider2D[1];
    Vector3 _movedir;
    RaycastHit2D _ground;
    public Hook hook;
    private void FixedUpdate()
    {

        _movedir = Vector3.right * Input.GetAxisRaw("Horizontal") * _speed;

        if(hook.isHookHited && hook.isRopeEnabled)
        {
            _movedir = Vector3.ProjectOnPlane(_movedir, (Vector2)(hook.hookedPos - (Vector2)transform.position));//.normalized*_speed; 
        }

        if (_ground.collider != null)
        {
            _movedir = Vector3.ProjectOnPlane(_movedir, _ground.normal);
            _rb.velocity /= 1.5f;
        }


        float speed = Mathf.Lerp(1, 0, (Vector3.Project(_movedir, (Vector3)_rb.velocity) + (Vector3)_rb.velocity).magnitude / _maxSpeed);

        _rb.AddForce(_movedir.normalized * speed * _speed,ForceMode2D.Impulse);
        //_rb.velocity += (Vector2)(_movedir.normalized * speed * _speed);

    }
    private void Update()
    {
        if(_ground = Physics2D.CircleCast(transform.position,_radius,Vector2.down,_groundDis,_whatIsGround)) 
            //if(Physics2D.OverlapCircleNonAlloc(transform.position,_radius,_collider)>0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _rb.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
            }
        }
        else
        {
           // _rb.AddForce(Vector3.down*Time.deltaTime,ForceMode2D.Impulse);
        }
    }


}
