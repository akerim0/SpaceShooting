using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    Renderer rend;
    [SerializeField] private float speed = 0.15f;
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float y = Time.deltaTime * speed;
        rend.material.mainTextureOffset += new Vector2(0,y);
    }
}
