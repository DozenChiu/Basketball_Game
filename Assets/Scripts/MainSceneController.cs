using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainSceneController : MonoBehaviour
{
    public Button backToHomeButton;
    public TextMeshProUGUI timerTextTMP;
    private float timeRemaining = 120f; // 兩分鐘倒計時

    void Start()
    {
        // 設置返回按鈕的點擊事件
        backToHomeButton = GameObject.Find("BackToHomeButton").GetComponent<Button>();
        backToHomeButton.onClick.AddListener(ReturnToHome);

        // 初始化計時器
        UpdateTimerText();
    }

    void Update()
    {
        // 倒計時邏輯
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerText();
        }
        else
        {
            ReturnToHome();
        }
    }

    void UpdateTimerText()
    {
        // 更新計時器顯示
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerTextTMP.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void ReturnToHome()
    {
        // 返回 HomeScene
        SceneManager.LoadScene("Home");
    }
}
