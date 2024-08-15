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
        // ���ùC��������r
        gameInstructionsTMP.enabled = false;

        // �����s�ì����̲K�[�I���ƥ�
        GameObject gameInstructionsButton = GameObject.Find("GameInstructionsButton");
        GameObject startGameButton = GameObject.Find("StartGameButton");

        gameInstructionsButton.GetComponent<Button>().onClick.AddListener(ToggleInstructions);
        startGameButton.GetComponent<Button>().onClick.AddListener(StartGame);
    }

    void ToggleInstructions()
    {
        // �����C��������r����ܪ��A
        isInstructionsVisible = !isInstructionsVisible;
        gameInstructionsTMP.enabled = isInstructionsVisible;
    }

    void StartGame()
    {
        // ������D�C������
        SceneManager.LoadScene("MainScene");
    }
}
