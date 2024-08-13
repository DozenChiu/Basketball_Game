using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public BasketballThrower basketballThrower;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // �� E �����x�y
        {
            basketballThrower.PickUpBasketball();
        }
    }
}
