using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyBulletScript : MonoBehaviour
{
    Rigidbody rb;
    //public static AllyBulletScript S;
    float bulletVel = 50.0f;

    // Start is called before the first frame update
    void Awake()
    {
       rb = this.gameObject.GetComponent<Rigidbody>();
    }

    public void shootBullet()
    {

        rb.velocity = Vector3.up * bulletVel;
        
    }
   
}
