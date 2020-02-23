using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHolder : MonoBehaviour
{
    [SerializeField]
    Vector4[] positions = new Vector4[1];


    // Update is called once per frame
    void Update()
    {
        positions[0] = this.transform.position;
        Shader.SetGlobalFloat("_PositionArray", 1);
        Shader.SetGlobalVectorArray("_Positions", positions);

    }
}