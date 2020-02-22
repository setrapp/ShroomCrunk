using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHolder : MonoBehaviour
{
    [SerializeField]
    List<GameObject> objects = new List<GameObject>(); //would be more efficient as a dictionary fix later or not (╯▔皿▔)╯
    Vector4[] positions = new Vector4[100];
          

    // Update is called once per frame
    void Update()
    {
        List<GameObject> buffer = new List<GameObject>();
        foreach(GameObject obj in objects)
        {
            if(obj != null)
            {
                buffer.Add(obj);
            }
        }

        for (int i = 0; i < buffer.Count; i++)
        {
            if (i < positions.Length)
            {
                positions[i] = buffer[i].transform.position;
            }
        }
        Shader.SetGlobalFloat("_PositionArray", Mathf.Min(buffer.Count, 100));
        Shader.SetGlobalVectorArray("_Positions", positions);

    }

    public void addGrassToThing(GameObject grass_instance)
    {
        objects.Add(grass_instance);
    }

    public void removeGrassFromThing(GameObject grass_instance)
    {
        objects.Remove(grass_instance);
    }

}