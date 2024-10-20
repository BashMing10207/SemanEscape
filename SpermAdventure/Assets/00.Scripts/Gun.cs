using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject pref;
    [SerializeField]
    Rigidbody2D _rb;
    public ProjectileSO projectileSO;
    [SerializeField]
    KeyCode _keyCode;

    void Update()
    {
        
        transform.up =Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        if(Input.GetKeyDown(_keyCode))
        {
            Instantiate(projectileSO.pref,transform.position,transform.rotation);
            _rb.AddForce(transform.up * -projectileSO.recoil);
        }
    }
}
