using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class UIScript : MonoBehaviour
{
    [Header("Inscribed")]
    public TMP_Text levelText;
    public TMP_Text killsText;
    public TMP_Text shieldLevText;
    public TMP_Text LevelUpGO;
    public GameObject GameOverGO;
    public GameObject PauseMenu;

    public bool GameIspaused = false;
    //public GameObject[] Enemies;
    //public GameObject[] Heroes;

    // Start is called before the first frame update
    void Awake()
    {
        LevelUpGO.gameObject.SetActive(false);
        GameOverGO.SetActive(false);
        PauseMenu.SetActive(false);
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
        //Debug.Log("t1 = " + t);
     
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
        //Debug.Log("t = " + t);
    }

    //

    void Restart()
    {
        SceneManager.LoadScene("__Scene_0");
    }
    public void ContinueGame()
    {
        if (!GameIspaused) return;
        //Enemies = GameObject.FindGameObjectsWithTag("enemy");
        //Heroes = GameObject.FindGameObjectsWithTag("hero");
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIspaused = false;
        
    }
    public void PauseGame()
    {
        if (GameIspaused) return;
        //Enemies = GameObject.FindGameObjectsWithTag("enemy");
        //Heroes = GameObject.FindGameObjectsWithTag("hero");
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameIspaused = true;
      
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}
