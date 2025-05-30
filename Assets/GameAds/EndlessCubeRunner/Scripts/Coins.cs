using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
   public float turnSpeed = 90f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(turnSpeed * Time.deltaTime, 0, 0);      
    }

}
