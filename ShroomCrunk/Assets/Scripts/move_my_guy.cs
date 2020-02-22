using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_my_guy : MonoBehaviour
{
    public float speed = .2f;
    private CharacterController cc;
    private float yPos;
    public bool Movin
    {
         get;  set;
    }
    void Start()
    {
        cc = this.GetComponent<CharacterController>();
        yPos = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {        
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        float forwardInput = Input.GetAxis("Vertical");
        float sideInput = Input.GetAxis("Horizontal");
        if(Mathf.Abs(sideInput) > Mathf.Epsilon || Mathf.Abs(forwardInput) > Mathf.Epsilon)
        {
            Movin = true;
        }
        else
        {
            Movin = false;
        }
        Vector3 move_vec = transform.forward * forwardInput * speed + transform.right * sideInput * speed;
        move_vec.y = 0;
        cc.Move(move_vec);
        //transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }
}
