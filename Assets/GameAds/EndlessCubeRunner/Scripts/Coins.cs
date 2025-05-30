using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    [SerializeField]
    private float xAngle = 90f;
    [SerializeField]
    private float yAngle = 0;
    [SerializeField]
    private float zAngle = 0;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(xAngle * Time.deltaTime, yAngle * Time.deltaTime, zAngle * Time.deltaTime);      
    }

}
