using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    static public Material[] GetAllMaterials (GameObject go)
    {
        Renderer[] rends = go.GetComponentsInChildren<Renderer>();
        Material[] mats = new Material[rends.Length];
        for (int i = 0; i < rends.Length; i++)
        {
            mats[i] = rends[i].material;
        }
        return mats;
    }
}
