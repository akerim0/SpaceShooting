using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetScript : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    private float speed = 10f;
    BoundSheckScript bndCheck;
    public Vector3 pos
    {
        get { return this.transform.position; } set { this.transform.position = value; }
    } 
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
    }
    private void Move()
    {
        rb.velocity = Vector3.down * speed ;
    }
}
