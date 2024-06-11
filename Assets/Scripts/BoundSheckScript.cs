using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundSheckScript : MonoBehaviour
{
    [System.Flags]
    public enum eScreenLocs
    {
        onScreen = 0,
        offRight = 1,
        offLeft =2,
        offUp = 4,
        offDown = 8
    }
    
    public enum eType {center , inset,outset };

    [Header("Inscribed")]
    public eType boundType = eType.center;
    public float radius = 1f;
    public bool keepOnScreen = true;

    [Header("Dynamic")]
    public eScreenLocs screenLocs = eScreenLocs.onScreen;
    //public bool isOnScreen = true;
    public float camWidth;
    public float camHeight;

    private void Awake()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * 0.75f;
        //camWidth = camHeight * Camera.main.aspect;
        //Debug.Log("Camera aspect " + Camera.main.aspect);
        //Debug.Log("Camera Height " + camHeight);
        //Debug.Log("Camera Width " + camWidth);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float checkRadius = 0;
        screenLocs = eScreenLocs.onScreen;
        //isOnScreen = true;
        if(boundType == eType.inset)
        {
            checkRadius = -radius;
        }
        if(boundType == eType.outset)
        {
            checkRadius = radius;
        }
        Vector3 pos = transform.position;

        // Check if On Screen
        if(pos.x > camWidth + checkRadius)
        {
            pos.x = camWidth + checkRadius;
            screenLocs |= eScreenLocs.offRight;
            //isOnScreen = false;
        }
        if (pos.x < -camWidth - checkRadius)
        {
            pos.x = -camWidth - checkRadius;
            screenLocs |= eScreenLocs.offLeft;
            //isOnScreen = false;
        }
        if (pos.y > camHeight +checkRadius)
        {
            pos.y = camHeight + checkRadius;
            screenLocs |= eScreenLocs.offUp;
            //isOnScreen = false;
        }
        if (pos.y < -camHeight - checkRadius)
        {
            pos.y = -camHeight - checkRadius;
            screenLocs |= eScreenLocs.offDown;
            //isOnScreen = false;
        }
        //

        if (keepOnScreen && !isOnScreen)
        {
            transform.position = pos;
            screenLocs = eScreenLocs.onScreen;
           // isOnScreen = true;
        }
        
    }
    public bool isOnScreen
    {
        get { return (screenLocs == eScreenLocs.onScreen); }
    }  
    public bool LocIs(eScreenLocs checkLoc)
    {
        if (checkLoc == eScreenLocs.onScreen) return isOnScreen;
        return ((screenLocs & checkLoc) == checkLoc);
    }
}
