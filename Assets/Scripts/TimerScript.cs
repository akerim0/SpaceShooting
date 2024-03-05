using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
    [Header("Inscribed")]
    public TMP_Text timerText;
    
    [SerializeField]private float _LevelDuration = 30;
    bool stoptime = false;

    System.TimeSpan timeSpan;
    void Awake()
    {
        timerText.color = Color.green;
        timerText.text = _LevelDuration.ToString(@"mm\:ss");
        timeSpan = System.TimeSpan.FromSeconds(_LevelDuration);
        timerText.text = timeSpan.ToString(@"mm\:ss");
    }

    // Update is called once per frame
    void Update()
    {
        if (stoptime == true) return;

        _LevelDuration -= Time.deltaTime;
        if(_LevelDuration <= 10.0f)
        {
            timerText.color = Color.red;
        }

        if(_LevelDuration < 0)
        {
            _LevelDuration = 0;
        }
        timeSpan = System.TimeSpan.FromSeconds(_LevelDuration);
        timerText.text = timeSpan.ToString(@"mm\:ss");

    }
    public float levelTimer { set => _LevelDuration = value; get => _LevelDuration; } 
    public void resetTimer()
    {
        stoptime = false;
        levelTimer = 30;
        timerText.color = Color.green;
    }
    public void stopTimer()
    {
        stoptime = true;
    }
}
