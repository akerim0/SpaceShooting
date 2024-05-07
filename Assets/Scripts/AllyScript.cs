using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AllyPos { none,left, right}

public class AllyScript : MonoBehaviour
{
    public static AllyScript AllyScriptInstance;
    [Header("Inscriped")]
    //public float speed = 0.5f;
    public float lerpSpeed = 0.35f;

    [Header("Dynamic")]
    Transform pointA;
    Transform pointB;
    EnemyScript enemySc;
    Vector3 currentPoint;
    Vector3 nextPoint;
    private float lerpTime = 0.0f;
    public AllyPos allyPos;
    public float timeDestroyed;


    public delegate void AllyScDelegate();
    public AllyScDelegate AllyScDestroyedEvent;
    
    //public AllyScript(AllyPos po)
    //{
    //    this.allyPos = po;
    //}
    public void Awake()
    {
        AllyScriptInstance = this;
        if (allyPos == AllyPos.left)
        {
            pointA = GameObject.Find("point A").transform;
            pointB = GameObject.Find("point B").transform;
        }
        else if (allyPos == AllyPos.right)
        {
            pointA = GameObject.Find("point C").transform;
            pointB = GameObject.Find("point D").transform;
        }
    }
    public void yo(AllyPos po)
    {
        allyPos = po;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentPoint = pointA.position;
        nextPoint = pointB.position;
    }

    // Update is called once per frame
    void Update()
    {
        lerpTime += Time.deltaTime * lerpSpeed;

        if(lerpTime >= 1)
        {
            lerpTime = 0;   
            if(currentPoint == pointA.position)
            {
                currentPoint = pointB.position;
                nextPoint = pointA.position;
            }
            else
            {
                currentPoint = pointA.position;
                nextPoint = pointB.position;
            }
        }
        transform.position = Vector3.Lerp(currentPoint, nextPoint,lerpTime);
        
    }
    // void OnTriggerEnter(Collider other)
    //{
    //    //enemySc = other.gameObject.GetComponent<EnemyScript>();
    //    //if (enemySc == null) return;
    //    Debug.Log("Ally Collision ! ");
    //    Destroy(this.gameObject);
    //}
    private void OnCollisionEnter(Collision collision)
    {
        AllyScDestroyedEvent?.Invoke();
        Debug.Log("Ally Collision ! ");
        Destroy(this.gameObject);
    }

}
