using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float moveSpeed = 5f;         // 移動速度
    public float rotationSpeed = 200f;  // 旋轉速度
    public float mouseSensitivity = 1f; // 滑鼠靈敏度
    private float yaw = 0f;             // 水平旋轉角度
    private float pitch = 0f;           // 垂直旋轉角度

    private float initialHeight;        // 初始高度

    void Start()
    {
        // 儲存相機的初始高度
        initialHeight = transform.position.y;
    }

    void Update()
    {
        // 移動控制
        float horizontalInput = Input.GetAxis("Horizontal"); // A, D 或 左右鍵
        float verticalInput = Input.GetAxis("Vertical");   // W, S 或 上下鍵

        // 取得相機的局部前、右方向向量
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        // 計算移動方向（僅在 X 和 Z 軸上移動）
        Vector3 moveDirection = forward * verticalInput;
        moveDirection += right * horizontalInput;

        // 設置移動方向並移動相機（保持高度不變）
        moveDirection = moveDirection.normalized * moveSpeed * Time.deltaTime;
        Vector3 newPosition = transform.position + moveDirection;

        // 更新相機的位置，保持原始高度
        transform.position = new Vector3(newPosition.x, initialHeight, newPosition.z);

        // 旋轉控制
        if (Input.GetMouseButton(0)) // 滑鼠左鍵按下
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            // 更新角度
            yaw += mouseX * rotationSpeed * Time.deltaTime * 10;
            pitch -= mouseY * rotationSpeed * Time.deltaTime * 10;

            // 限制垂直旋轉角度
            pitch = Mathf.Clamp(pitch, -89f, 89f);

            // 應用旋轉
            transform.eulerAngles = new Vector3(pitch, yaw, 0);
        }
    }
}
