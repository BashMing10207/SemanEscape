using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    protected float _speed;
    [SerializeField]
    protected LayerMask _layerMask;
    protected Vector2 _dir;
    protected void OnEnable()
    {
        Init();
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        RaycastHit2D hitomi = Scan();
        if (hitomi.collider)
        {
            Hit(hitomi);
        }
        Move();
    }
    
    protected virtual void Init()
    {
        _dir = transform.up * _speed;
    }

    protected virtual void Move()
    {
        transform.position += (Vector3)(_dir * Time.fixedDeltaTime);
    }

    protected virtual RaycastHit2D Scan()
    {
        return Physics2D.Raycast(transform.position, _dir, _dir.magnitude * Time.fixedDeltaTime, _layerMask);
    }
    
    protected virtual void Hit(RaycastHit2D hitomi)
    {
        if (hitomi.transform.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            rb.AddForce(_dir * 2, ForceMode2D.Impulse);

        transform.position = hitomi.point;
        Destroy(gameObject);
    }

}
