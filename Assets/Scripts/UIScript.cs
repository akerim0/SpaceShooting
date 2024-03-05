using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIScript : MonoBehaviour
{
    [Header("Inscribed")]
    public TMP_Text levelText;
    public TMP_Text killsText;
    public TMP_Text shieldLevText;
    public TMP_Text LevelUpGO;
    public GameObject GameOverGO;

    
    // Start is called before the first frame update
    void Awake()
    {
        LevelUpGO.gameObject.SetActive(false);
        GameOverGO.SetActive(false);
        Color temp = LevelUpGO.color;
        temp.a = 0;
        LevelUpGO.color = temp;
    }

    public void UpdateKills(int kills)
    {
        killsText.text = kills.ToString();
    }
    public void UpdateShieldLevel(float shield)
    {
        shieldLevText.text = shield.ToString();
    }
   public void UpdateLevel(int level)
    {
        levelText.text = level.ToString();
    }
    public void DisplayGameOver()
    {
        GameOverGO.SetActive(true);
    }
    public void HideGameOver()
    {
        GameOverGO.SetActive(false);
    }
    public void LevelUpfade()
    {
        Color disp = LevelUpGO.color ; 
        LevelUpGO.gameObject.SetActive(true);

        float t = disp.a;
        Debug.Log("t1 = " + t);
     
        if (disp.a >= 0.70f)
        {
            t = 0f;
            LevelUpGO.gameObject.SetActive(false);
            MainScript.fade = false;
        }
        else
        {
            t += 0.3f * Time.deltaTime;
        }

        disp = new Color(disp.r, disp.g, disp.b, t);
        LevelUpGO.color  = disp;
        Debug.Log("t = " + t);
    }
}
