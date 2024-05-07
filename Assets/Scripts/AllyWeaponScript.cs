using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyWeaponScript : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject projectile; 
    private float bulletVel = 50.0f;
    public float shotTime ;

    [Header("Dynamic")]
    Transform shotPoint;
    GameObject bulletAnchor;
    float nextShotTime = 0;
    BulletScript allyBScript;
    // Start is called before the first frame update
    void Start()
    {
        shotPoint = transform.GetChild(0);
        bulletAnchor = GameObject.Find("_BulletAnchor");
        if(bulletAnchor == null)
        {
            bulletAnchor = new GameObject("_BulletAnchor");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextShotTime)
        {
            MakeProjectile();
        }
    }
    
    void MakeProjectile()
    {
        GameObject go;
        go = Instantiate<GameObject>(projectile,bulletAnchor.transform);
        go.transform.position = shotPoint.position;
        allyBScript = go.GetComponent<BulletScript>();
        //allyBScript.shootBullet();
        if (allyBScript != null)
            allyBScript.velocity = Vector3.up * bulletVel;

        shotTime = Random.Range(1.5f, 2.5f);
        nextShotTime = Time.time + shotTime;

    }
}
