using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BasketballThrower : MonoBehaviour
{
    public GameObject basketballPrefab; // 籃球預置體
    public Transform shootingPoint; // 投擲點
    public Transform[] hoops; // ?球架的位置 (可包含多個籃框)
    public float minThrowForce = 10f; // 最小投擲力度
    public float maxThrowForce = 20f; // 最大投擲力度
    public float minThrowAngle = 40f; // 最小投擲角度
    public float maxThrowAngle = 80f; // 最大投擲角度

    public Slider forceSlider; // 力度進度條
    public Slider angleSlider; // 角度進度條
    public TextMeshProUGUI forceText; // 力度文字框
    public TextMeshProUGUI angleText; // 角度文字框
    public TextMeshProUGUI scoreText; // 分數顯示文字框

    private bool isHoldingBall = false; // 是否抓住籃球
    private float currentThrowForce; // 當前投擲力度
    private float currentThrowAngle; // 當前投擲角度
    private Vector3 throwDirection; // 投擲方向
    private int score = 0; // 分數

    void Start()
    {
        currentThrowForce = minThrowForce;
        currentThrowAngle = minThrowAngle;
        throwDirection = shootingPoint.forward;

        // 初始化 UI
        forceSlider.minValue = minThrowForce;
        forceSlider.maxValue = maxThrowForce;
        angleSlider.minValue = minThrowAngle;
        angleSlider.maxValue = maxThrowAngle;

        UpdateUI();
    }

    void Update()
    {
        // 調整投擲方向、力度和角度
        AdjustThrowDirection();
        AdjustThrowForceAndAngle();

        if (Input.GetMouseButtonDown(0) && isHoldingBall) // 按下滑鼠左鍵投籃
        {
            ThrowBasketball();
        }

        // 使用射線檢測進球
        CheckBasketballInHoops();

        UpdateUI(); // 更新 UI
    }

    void AdjustThrowDirection()
    {
        // 控制上下方向
        float verticalInput = Input.GetAxis("Vertical");
        throwDirection = Quaternion.Euler(verticalInput * 10, 0, 0) * shootingPoint.forward;

        // 控制左右方向
        float horizontalInput = Input.GetAxis("Horizontal");
        throwDirection = Quaternion.Euler(0, horizontalInput * 10, 0) * throwDirection;
    }

    void AdjustThrowForceAndAngle()
    {
        // 使用 Z 和 X 鍵調整投擲角度
        if (Input.GetKey(KeyCode.X))
        {
            currentThrowAngle += Time.deltaTime * 10f; // 按住 X 鍵增加角度
        }
        if (Input.GetKey(KeyCode.Z))
        {
            currentThrowAngle -= Time.deltaTime * 10f; // 按住 Z 鍵減少角度
        }
        currentThrowAngle = Mathf.Clamp(currentThrowAngle, minThrowAngle, maxThrowAngle); // 限制角度範圍

        // 使用 C 和 V 鍵調整投擲力度
        if (Input.GetKey(KeyCode.V))
        {
            currentThrowForce += Time.deltaTime * 10f; // 按住 V 鍵增加力度
        }
        if (Input.GetKey(KeyCode.C))
        {
            currentThrowForce -= Time.deltaTime * 10f; // 按住 C 鍵減少力度
        }
        currentThrowForce = Mathf.Clamp(currentThrowForce, minThrowForce, maxThrowForce); // 限制力度範圍
    }

    void ThrowBasketball()
    {
        GameObject ball = Instantiate(basketballPrefab, shootingPoint.position, Quaternion.identity);
        Rigidbody rb = ball.GetComponent<Rigidbody>();

        // 計算拋物線的投擲方向
        Vector3 launchDirection = CalculateLaunchDirection(throwDirection, currentThrowAngle);
        rb.AddForce(launchDirection * currentThrowForce, ForceMode.Impulse);

        isHoldingBall = false;
        // 在 10 秒後自動銷毀未得分的籃球
        Destroy(ball, 10f);
    }

    Vector3 CalculateLaunchDirection(Vector3 forwardDirection, float angle)
    {
        // 計算拋物線的方向
        float radians = angle * Mathf.Deg2Rad;
        Vector3 direction = forwardDirection;
        direction.y = Mathf.Tan(radians); // 設置上拋角度
        direction.Normalize(); // 確保方向向量的長度為 1

        return direction;
    }

    void CheckBasketballInHoops()
    {
        foreach (Transform hoop in hoops)
        {
            RaycastHit hit;
            if (Physics.Raycast(hoop.position, hoop.forward, out hit, 1f))
            {
                // 檢查射線是否碰到籃球
                if (hit.collider.CompareTag("Basketball"))
                {
                    // 計算籃球和籃框的距離
                    float distanceFromHoop = Vector3.Distance(hit.collider.transform.position, hoop.position);
                    Debug.Log("Distance from hoop: " + distanceFromHoop);

                    // 判斷得分
                    if (distanceFromHoop > 0.25f)
                    {
                        score += 3; // 三分
                        Debug.Log("Scored 3 Points!");
                    }
                    else
                    {
                        score += 2; // 兩分
                        Debug.Log("Scored 2 Points!");
                    }

                    // 更新分數 UI
                    UpdateScoreUI();

                    // 銷毀籃球
                    Destroy(hit.collider.gameObject);

                    // 退出循環，避免在兩個籃框中重複計算
                    break;
                }
            }
        }
    }

    public void PickUpBasketball()
    {
        isHoldingBall = true;
    }

    void UpdateUI()
    {
        // 更新進度條和文字框
        forceSlider.value = currentThrowForce;
        angleSlider.value = currentThrowAngle;

        forceText.text = $"Force: {currentThrowForce:F1}";
        angleText.text = $"Angle: {currentThrowAngle:F1}";
    }

    void UpdateScoreUI()
    {
        // 確保 UI 元件已經連接
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
            Debug.Log("Updated Score: " + score); // 打印更新的分數
        }
        else
        {
            Debug.LogError("ScoreText is not assigned!");
        }
    }

    void OnDrawGizmos()
    {
        if (hoops.Length > 0)
        {
            Gizmos.color = Color.red;
            foreach (Transform hoop in hoops)
            {
                Gizmos.DrawLine(hoop.position, hoop.position + hoop.forward * 1f); // 1f 為線長度
            }
        }
    }
}
