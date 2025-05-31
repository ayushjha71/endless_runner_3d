using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField]
    private float xAngle = 90f;
    [SerializeField]
    private float yAngle = 0;
    [SerializeField]
    private float zAngle = 0;
    [SerializeField]
    private float speed = 2f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(xAngle * Time.deltaTime * speed, yAngle * Time.deltaTime * speed, zAngle * Time.deltaTime * speed);      
    }

}
