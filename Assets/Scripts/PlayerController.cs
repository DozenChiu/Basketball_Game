using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public BasketballThrower basketballThrower;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // «ö E Áä§ì¨úÄx²y
        {
            basketballThrower.PickUpBasketball();
        }
    }
}
