using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum eWeaponType
{
    none,
    blaster,
    lazer,
    spread,
    phaser,
    missile,
    shield
}
[System.Serializable]
public class WeaponDefinition
{
    public eWeaponType type = eWeaponType.none;
    public string letter;
    public Color powerUpColor = Color.white;
    public GameObject weaponModelPrefab;
    public GameObject projectilePrefab;
    public Color projectileColor = Color.white;
    public float damageOnHit = 0;
    public float damagePerSec = 0;
    public float delayBetWeenShots = 0;
    public float velocity = 50;
}
public class WeaponScript : MonoBehaviour
{
    static public Transform bulletAnchor;

    [Header("Dynamic")]
    [SerializeField] private eWeaponType _type = eWeaponType.none;
    public WeaponDefinition def;
    public float nextShotTime;
    private GameObject weaponModel;
    private Transform shotPointTrans;

    private void Start()
    {
        if (bulletAnchor == null)
        {
            GameObject go = new GameObject("_BulletAnchor");
            bulletAnchor = go.transform;
        }
        shotPointTrans = transform.GetChild(0);
        SetType(_type);

        ShipScript shipScr = GetComponentInParent<ShipScript>();
        if (shipScr != null) shipScr.fireEvent += Fire;
    }

    public eWeaponType type
    {
        get { return _type; }
        set { SetType(value); }
    }
    public void SetType(eWeaponType wt)
    {
        _type = wt;
        if(type == eWeaponType.none)
        {
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            this.gameObject.SetActive(true);
        }
        def = MainScript.GET_WEAPON_DEFINITION(_type);
        if (weaponModel != null) Destroy(weaponModel);
        weaponModel = Instantiate<GameObject>(def.weaponModelPrefab, transform);
        weaponModel.transform.localPosition = Vector3.zero;
        weaponModel.transform.localScale = Vector3.one;

        nextShotTime = 0;
    }

    private void Fire()
    {
        if (!gameObject.activeInHierarchy) return;
        if (Time.time < nextShotTime) return;

        BulletScript bullet ;
        Vector3 vel = Vector3.up * def.velocity;

        switch (type)
        {
            case eWeaponType.blaster:
                bullet = MakeProjectile();
                bullet.velocity = vel;
                break;
            case eWeaponType.spread:
                bullet = MakeProjectile();
                bullet.velocity = vel;
                bullet = MakeProjectile();
                bullet.transform.rotation = Quaternion.AngleAxis(10, Vector3.back);
                bullet.velocity = bullet.transform.rotation * vel;
                bullet = MakeProjectile();
                bullet.transform.rotation = Quaternion.AngleAxis(-10, Vector3.back);
                bullet.velocity = bullet.transform.rotation * vel;
                break;
        }
    }
    private BulletScript MakeProjectile()
    {
        GameObject go;
        go = Instantiate<GameObject>(def.projectilePrefab, bulletAnchor);
        BulletScript b = go.GetComponent<BulletScript>();
        Vector3 pos = shotPointTrans.position;
        pos.z = 0;
        b.transform.position = pos;
        b.type = type;
        nextShotTime = Time.time + def.delayBetWeenShots;

        return b;
    }
}