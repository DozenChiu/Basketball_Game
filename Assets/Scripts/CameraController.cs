using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float moveSpeed = 5f;         // ���ʳt��
    public float rotationSpeed = 200f;  // ����t��
    public float mouseSensitivity = 1f; // �ƹ��F�ӫ�
    private float yaw = 0f;             // �������ਤ��
    private float pitch = 0f;           // �������ਤ��

    private float initialHeight;        // ��l����

    void Start()
    {
        // �x�s�۾�����l����
        initialHeight = transform.position.y;
    }

    void Update()
    {
        // ���ʱ���
        float horizontalInput = Input.GetAxis("Horizontal"); // A, D �� ���k��
        float verticalInput = Input.GetAxis("Vertical");   // W, S �� �W�U��

        // ���o�۾��������e�B�k��V�V�q
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        // �p�Ⲿ�ʤ�V�]�Ȧb X �M Z �b�W���ʡ^
        Vector3 moveDirection = forward * verticalInput;
        moveDirection += right * horizontalInput;

        // �]�m���ʤ�V�ò��ʬ۾��]�O�����פ��ܡ^
        moveDirection = moveDirection.normalized * moveSpeed * Time.deltaTime;
        Vector3 newPosition = transform.position + moveDirection;

        // ��s�۾�����m�A�O����l����
        transform.position = new Vector3(newPosition.x, initialHeight, newPosition.z);

        // ���౱��
        if (Input.GetMouseButton(0)) // �ƹ�������U
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            // ��s����
            yaw += mouseX * rotationSpeed * Time.deltaTime * 10;
            pitch -= mouseY * rotationSpeed * Time.deltaTime * 10;

            // ��������ਤ��
            pitch = Mathf.Clamp(pitch, -89f, 89f);

            // ���α���
            transform.eulerAngles = new Vector3(pitch, yaw, 0);
        }
    }
}
