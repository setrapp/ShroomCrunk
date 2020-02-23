using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene("title screen");
    }
}
