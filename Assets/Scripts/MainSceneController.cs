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
    private float timeRemaining = 120f; // ������˭p��

    void Start()
    {
        // �]�m��^���s���I���ƥ�
        backToHomeButton = GameObject.Find("BackToHomeButton").GetComponent<Button>();
        backToHomeButton.onClick.AddListener(ReturnToHome);

        // ��l�ƭp�ɾ�
        UpdateTimerText();
    }

    void Update()
    {
        // �˭p���޿�
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
        // ��s�p�ɾ����
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerTextTMP.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void ReturnToHome()
    {
        // ��^ HomeScene
        SceneManager.LoadScene("Home");
    }
}
