using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoundSheckScript))]
public class EnemyScript : MonoBehaviour
{
    [Header("Inscribed")]
    public float speed = 10f;
    public float health = 10f;
    public float fireRate = 0.3f;
    public int score = 100;
    public float powerUpDropChance = 1f;
    protected bool calledShipDestroyed = false;
    protected BoundSheckScript bndCheck;
    public Vector3 pos
    {
        get { return this.transform.position; }
        set { this.transform.position = value; }
    }

    void Awake()
    {
        bndCheck = GetComponent<BoundSheckScript>();
    }
    // Update is called once per frame
    void Update()
    {
        Move();
        if (bndCheck.LocIs(BoundSheckScript.eScreenLocs.offDown))
        {
            Destroy(gameObject);
        }
        //if (!bndCheck.isOnScreen)
        //{
        //    if(pos.y <bndCheck.camHeight - bndCheck.radius)
        //    {
        //        Destroy(gameObject);
        //    }
        //}
    }
    void OnCollisionEnter(Collision collision)
    {
        GameObject otherGO = collision.gameObject;
        BulletScript b = otherGO.GetComponent<BulletScript>();
        if(b != null)
        {
            if (bndCheck.isOnScreen)
            {
                health -= MainScript.GET_WEAPON_DEFINITION(b.type).damageOnHit;
                if(health <= 0)
                {
                    if (!calledShipDestroyed)
                    {
                        calledShipDestroyed = true;
                        MainScript.SHIP_DESTROYED(this);
                    }    
                    Destroy(this.gameObject);
                }
            }
            Destroy(otherGO);
        }
        else
        {
            Debug.Log("Enemy hit by non Bullet Obj: " + otherGO.gameObject.name);
        }
    }
    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }
}
