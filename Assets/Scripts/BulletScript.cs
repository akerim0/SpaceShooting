using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoundSheckScript))]
public class BulletScript : MonoBehaviour
{
    private BoundSheckScript bndCheck;
    private Renderer rend;

    [Header("Dynamic")]
    public Rigidbody rigid;
    [SerializeField] private eWeaponType _type;

    public eWeaponType type
    {
        get { return (_type); }
        set { SetType(value); }
    }

    // Start is called before the first frame update
    void Awake()
    {
        bndCheck = GetComponent<BoundSheckScript>();
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bndCheck.LocIs(BoundSheckScript.eScreenLocs.offUp))
        {
            Destroy(gameObject);
        }
    }
    public void SetType(eWeaponType eType)
    {
        _type = eType;
        WeaponDefinition def = MainScript.GET_WEAPON_DEFINITION(_type);
        rend.material.color = def.projectileColor;
            
    }
    public Vector3 velocity
    {
        get { return rigid.velocity; }
        set { rigid.velocity = value; }
    }
}
