using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mushroom_spawnito_me_amigo : MonoBehaviour
{
    public GameObject mushroom;
    public GameObject grass;
    public float spawnRadius;
    public float shroomGrowChance;
    public float grassGrowChance;
    private int mask;
    public float grassDeleteTime;
    public float mushroomDeleteTime;

    //TODO remove
    private move_my_guy mmg;
    
    public Transform raycasting_point;
    bool spawning = false;

    // Start is called before the first frame update
    void Start()
    {
        mask = LayerMask.GetMask("GrowSurface", "Ground");
    }

    // Update is called once per frame
    void Update()
    {
		// TODO: remove
		if (mmg != null)
		{
			spawning = mmg.Movin;
		}

		if (spawning)
        {
			RaycastHit hit;
            bool hit_some = Physics.Raycast(raycasting_point.position, randomDirection(), out hit, spawnRadius, mask);
            if (hit_some)
            {
                //Debug.Log(hit.point);
                float roll = Random.Range(0f, 1f);
                if (roll < shroomGrowChance)
                {
                    if (Vector3.SqrMagnitude(hit.point - transform.position) > 36)
                    {
                        spawnMushroom(hit);
                    }
                }
                else if (roll < grassGrowChance)
                {
                    spawnGrass(hit);
                }
            }
        }
    }

    private bool spawnGrass(RaycastHit hit)
    {
        Quaternion rot = Quaternion.AngleAxis(Random.Range(0f, 360f), hit.normal);
        GameObject instance = Instantiate(grass, hit.point, rot);
        grass_script grasScript= instance.GetComponent<grass_script>();
        if (grasScript != null)
        {
            grasScript.growUp();
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool spawnMushroom(RaycastHit hit)
    {
        Quaternion rot = Quaternion.AngleAxis(Random.Range(0f, 360f), hit.normal);
        GameObject instance = Instantiate(mushroom, hit.point, rot);
        mushroom_script mushScript = instance.GetComponent<mushroom_script>();
        if(mushScript != null)
        {
            mushScript.growUp();
            return true;
        } else
        {
            return false;
        }
    }

    private Vector3 randomDirection()
    {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

	public void Event_SpawningBegin()
	{
		spawning = true;
	}

	public void Event_SpawningEnd()
	{
		spawning = false;
	}
}
