﻿using System.Collections;
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
    private move_my_guy mmg;
    public float grassDeleteTime;
    public float mushroomDeleteTime;
    
    public Transform raycasting_point;

    // Start is called before the first frame update
    void Start()
    {
        mmg = GetComponent<move_my_guy>();
        mask = LayerMask.GetMask("GrowSurface");
    }

    // Update is called once per frame
    void Update()
    {
        if (mmg.Movin)
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
}
