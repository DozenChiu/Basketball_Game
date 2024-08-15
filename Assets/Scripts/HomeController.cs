using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeController : MonoBehaviour
{
    public TextMeshProUGUI gameInstructionsTMP;
    private bool isInstructionsVisible = false;

    void Start()
    {
        // 隱藏遊戲說明文字
        gameInstructionsTMP.enabled = false;

        // 找到按鈕並為它們添加點擊事件
        GameObject gameInstructionsButton = GameObject.Find("GameInstructionsButton");
        GameObject startGameButton = GameObject.Find("StartGameButton");

        gameInstructionsButton.GetComponent<Button>().onClick.AddListener(ToggleInstructions);
        startGameButton.GetComponent<Button>().onClick.AddListener(StartGame);
    }

    void ToggleInstructions()
    {
        // 切換遊戲說明文字的顯示狀態
        isInstructionsVisible = !isInstructionsVisible;
        gameInstructionsTMP.enabled = isInstructionsVisible;
    }

    void StartGame()
    {
        // 切換到主遊戲場景
        SceneManager.LoadScene("MainScene");
    }
}
