using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BasketballThrower : MonoBehaviour
{
    public GameObject basketballPrefab; // �x�y�w�m��
    public Transform shootingPoint; // ���Y�I
    public Transform[] hoops; // ?�y�[����m (�i�]�t�h���x��)
    public float minThrowForce = 10f; // �̤p���Y�O��
    public float maxThrowForce = 20f; // �̤j���Y�O��
    public float minThrowAngle = 40f; // �̤p���Y����
    public float maxThrowAngle = 80f; // �̤j���Y����

    public Slider forceSlider; // �O�׶i�ױ�
    public Slider angleSlider; // ���׶i�ױ�
    public TextMeshProUGUI forceText; // �O�פ�r��
    public TextMeshProUGUI angleText; // ���פ�r��
    public TextMeshProUGUI scoreText; // ������ܤ�r��

    private bool isHoldingBall = false; // �O�_����x�y
    private float currentThrowForce; // ��e���Y�O��
    private float currentThrowAngle; // ��e���Y����
    private Vector3 throwDirection; // ���Y��V
    private int score = 0; // ����

    void Start()
    {
        currentThrowForce = minThrowForce;
        currentThrowAngle = minThrowAngle;
        throwDirection = shootingPoint.forward;

        // ��l�� UI
        forceSlider.minValue = minThrowForce;
        forceSlider.maxValue = maxThrowForce;
        angleSlider.minValue = minThrowAngle;
        angleSlider.maxValue = maxThrowAngle;

        UpdateUI();
    }

    void Update()
    {
        // �վ���Y��V�B�O�שM����
        AdjustThrowDirection();
        AdjustThrowForceAndAngle();

        if (Input.GetMouseButtonDown(0) && isHoldingBall) // ���U�ƹ�������x
        {
            ThrowBasketball();
        }

        // �ϥήg�u�˴��i�y
        CheckBasketballInHoops();

        UpdateUI(); // ��s UI
    }

    void AdjustThrowDirection()
    {
        // ����W�U��V
        float verticalInput = Input.GetAxis("Vertical");
        throwDirection = Quaternion.Euler(verticalInput * 10, 0, 0) * shootingPoint.forward;

        // ����k��V
        float horizontalInput = Input.GetAxis("Horizontal");
        throwDirection = Quaternion.Euler(0, horizontalInput * 10, 0) * throwDirection;
    }

    void AdjustThrowForceAndAngle()
    {
        // �ϥ� Z �M X ��վ���Y����
        if (Input.GetKey(KeyCode.X))
        {
            currentThrowAngle += Time.deltaTime * 10f; // ���� X ��W�[����
        }
        if (Input.GetKey(KeyCode.Z))
        {
            currentThrowAngle -= Time.deltaTime * 10f; // ���� Z ���֨���
        }
        currentThrowAngle = Mathf.Clamp(currentThrowAngle, minThrowAngle, maxThrowAngle); // ����׽d��

        // �ϥ� C �M V ��վ���Y�O��
        if (Input.GetKey(KeyCode.V))
        {
            currentThrowForce += Time.deltaTime * 10f; // ���� V ��W�[�O��
        }
        if (Input.GetKey(KeyCode.C))
        {
            currentThrowForce -= Time.deltaTime * 10f; // ���� C ���֤O��
        }
        currentThrowForce = Mathf.Clamp(currentThrowForce, minThrowForce, maxThrowForce); // ����O�׽d��
    }

    void ThrowBasketball()
    {
        GameObject ball = Instantiate(basketballPrefab, shootingPoint.position, Quaternion.identity);
        Rigidbody rb = ball.GetComponent<Rigidbody>();

        // �p��ߪ��u�����Y��V
        Vector3 launchDirection = CalculateLaunchDirection(throwDirection, currentThrowAngle);
        rb.AddForce(launchDirection * currentThrowForce, ForceMode.Impulse);

        isHoldingBall = false;
        // �b 10 ���۰ʾP�����o�����x�y
        Destroy(ball, 10f);
    }

    Vector3 CalculateLaunchDirection(Vector3 forwardDirection, float angle)
    {
        // �p��ߪ��u����V
        float radians = angle * Mathf.Deg2Rad;
        Vector3 direction = forwardDirection;
        direction.y = Mathf.Tan(radians); // �]�m�W�ߨ���
        direction.Normalize(); // �T�O��V�V�q�����׬� 1

        return direction;
    }

    void CheckBasketballInHoops()
    {
        foreach (Transform hoop in hoops)
        {
            RaycastHit hit;
            if (Physics.Raycast(hoop.position, hoop.forward, out hit, 1f))
            {
                // �ˬd�g�u�O�_�I���x�y
                if (hit.collider.CompareTag("Basketball"))
                {
                    // �p���x�y�M�x�ت��Z��
                    float distanceFromHoop = Vector3.Distance(hit.collider.transform.position, hoop.position);
                    Debug.Log("Distance from hoop: " + distanceFromHoop);

                    // �P�_�o��
                    if (distanceFromHoop > 0.25f)
                    {
                        score += 3; // �T��
                        Debug.Log("Scored 3 Points!");
                    }
                    else
                    {
                        score += 2; // ���
                        Debug.Log("Scored 2 Points!");
                    }

                    // ��s���� UI
                    UpdateScoreUI();

                    // �P���x�y
                    Destroy(hit.collider.gameObject);

                    // �h�X�`���A�קK�b����x�ؤ����ƭp��
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
        // ��s�i�ױ��M��r��
        forceSlider.value = currentThrowForce;
        angleSlider.value = currentThrowAngle;

        forceText.text = $"Force: {currentThrowForce:F1}";
        angleText.text = $"Angle: {currentThrowAngle:F1}";
    }

    void UpdateScoreUI()
    {
        // �T�O UI ����w�g�s��
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
            Debug.Log("Updated Score: " + score); // ���L��s������
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
                Gizmos.DrawLine(hoop.position, hoop.position + hoop.forward * 1f); // 1f ���u����
            }
        }
    }
}
