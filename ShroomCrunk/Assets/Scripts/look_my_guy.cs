using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class look_my_guy : MonoBehaviour
{
    public GameObject target;
    private Quaternion myRotation;
    public float distance;
    public float xSpeed;
    public float ySpeed;
    public int yMinLimit;
    public int yMaxLimit;
    private float x;
    private float y;
    private Vector3 angles;


    // Use this for initialization
    void Start()
    {

        distance = 10.0f;

        xSpeed = 250.0f;
        ySpeed = 120.0f;

        yMinLimit = -20;
        yMaxLimit = 80;

        x = 0.0f;
        y = 0.0f;
        angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;


    }

    // Update is called once per frame
    void Update()
    {

        xSpeed = 25f;
        ySpeed = 6.0f;

        if (target)
        {
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            y = ClampAngle(y, yMinLimit, yMaxLimit);
            myRotation = Quaternion.Euler(y, x, 0);
            transform.rotation = myRotation;
        }

    }
    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
        {
            angle += 360;
        }
        if (angle > 360)
        {
            angle -= 360;
        }

        return Mathf.Clamp(angle, min, max);
    }
}
