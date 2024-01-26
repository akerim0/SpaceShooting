using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipScript : MonoBehaviour
{
    static public ShipScript S { get; private set; }

    [Header("inscribed")]
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    public GameObject bulletPref;
    public float bulletSpeed = 40f;
    public WeaponScript[] weapons;

    [Header("Dynamic")]
    [Range(0, 4)]
    [SerializeField] private float _shieldLevel = 1;
    [Tooltip("This fiels holds a reference to the last triggering GameObject")]
    private GameObject lasttriggerGo = null;

    public delegate void WeaponFireDelegate();
    public event WeaponFireDelegate fireEvent;



    private void Awake()
    {
        if (S == null)
        {
            S = this;
        }
        ClearWeapons();
        weapons[0].SetType(eWeaponType.blaster);
        //fireEvent += TempFire;
    }

    // Update is called once per frame
    void Update()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += hAxis * speed * Time.deltaTime;
        pos.y += vAxis * speed * Time.deltaTime;
        transform.position = pos;

        transform.rotation = Quaternion.Euler(vAxis * pitchMult, hAxis * rollMult, 0);
        if (Input.GetKeyDown(KeyCode.Space) && fireEvent != null) {
            fireEvent();
        } 
    }
    private void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        //Debug.Log("Shield trigger hit by : " + go.gameObject.name);
        if (go == lasttriggerGo) return;
        lasttriggerGo = go;
        EnemyScript enemyScript = go.GetComponent<EnemyScript>();
        PowerUpScript pUp = go.GetComponent<PowerUpScript>();

        if (enemyScript != null) {
            shieldLevel--;
            Destroy(go);

        }
        else if (pUp != null)
        {
            AbsorbPowerUp(pUp);
        }
        else
        {
            Debug.LogWarning("Shield trigger hit by non Enemy: " + go.name);
        }
      

    }
    public void AbsorbPowerUp(PowerUpScript pUp)
    {
        Debug.Log("Absorbed power Up " + pUp.type);
        switch (pUp.type)
        {
            case eWeaponType.shield:
                shieldLevel++;
                break;
            default:
                if(pUp.type == weapons[0].type)
                {
                    WeaponScript weap = GetEmptyWeaponSlot();
                    if(weap != null)
                    {
                        weap.SetType(pUp.type);
                    }
                   
                }
                else
                {
                    ClearWeapons();
                    weapons[0].SetType(pUp.type);
                }
                break;
        }
        pUp.AbsorbedBy(this.gameObject);
    }

    //void TempFire()
    //{
    //    GameObject projGO = Instantiate<GameObject>(bulletPref);
    //    projGO.transform.position = transform.position;
    //    Rigidbody rb = projGO.GetComponent<Rigidbody>();
    //    //rb.velocity = Vector3.up * bulletSpeed;
    //    BulletScript bulletSc = projGO.GetComponent<BulletScript>();
    //    bulletSc.type = eWeaponType.blaster;
    //    float tSpeed = MainScript.GET_WEAPON_DEFINITION(bulletSc.type).velocity;
    //    rb.velocity = Vector3.up * tSpeed;

    //}
    public float shieldLevel {
        get { return _shieldLevel; }
        set { _shieldLevel = Mathf.Min(value, 4);
            if(value < 0)
            {
                Destroy(this.gameObject);
                MainScript.HeroDied();
            }
        }
    }
    WeaponScript GetEmptyWeaponSlot()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].type == eWeaponType.none)
            {
                return weapons[i];
            }
        }
        return null;
    }
    void ClearWeapons()
    {
        foreach(WeaponScript w in weapons)
        {
            w.SetType(eWeaponType.none);
        }
    }
}
