using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            quitDisBitch();
        }
    }
    public void quitDisBitch()
    {
        Application.Quit();
    }
}
