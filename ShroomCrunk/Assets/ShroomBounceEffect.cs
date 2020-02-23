using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomBounceEffect : MonoBehaviour
{
    public Transform effectCenter;
    public GameObject thingThatYouAreSpawning;
    public float initSpawnRadius;
    public float finalSpawnRadius;
    public float spawnTime;
    public int spawnAmount;
    public ParticleSystem particles;
    private int mask;

    public float cooldown;
    private bool cooling;
    private void Start()
    {
        cooling = false;
        mask = LayerMask.GetMask("GrowSurface", "Ground");
        //StartCoroutine(spawny_spawn());
    }

    public IEnumerator spawny_spawn()
    {
        cooling = true;
        Debug.Log("Starting coroutine");
        float timePassed = 0f;
        while(timePassed < spawnTime)
        {
            Vector3 vec = -effectCenter.transform.up * finalSpawnRadius + Mathf.Lerp(initSpawnRadius, finalSpawnRadius, timePassed / spawnTime) * effectCenter.transform.forward;
            for (int i = 0; i < spawnAmount; i++)
            {
                RaycastHit hit;
                bool hitSumthin = Physics.Raycast(effectCenter.position, vec, out hit, 1000f, mask);
                if (hitSumthin)
                {
                    spawnGrass(hit);
                }
                vec = Quaternion.AngleAxis(((float)i / (float)spawnAmount + Random.Range(-.1f, .1f)) * 360, effectCenter.transform.up) * vec;
            }
            timePassed += Time.deltaTime;
            yield return null;
        }
        yield return coolDownPeriod();
    }

    private IEnumerator coolDownPeriod()
    {
        float timePassed = 0f;
        while (timePassed < cooldown)
        {
            timePassed += Time.deltaTime;
            yield return null;
        }
        cooling = false;
    }
    public void triggerEffect()
    {
        particles.Play();
        if (!cooling)
        {
            StartCoroutine(spawny_spawn());
        }
        
    }

    private bool spawnGrass(RaycastHit hit)
    {
        Quaternion rot = Quaternion.AngleAxis(Random.Range(0f, 360f), hit.normal);
        GameObject instance = Instantiate(thingThatYouAreSpawning, hit.point, rot);
        grass_script grasScript = instance.GetComponent<grass_script>();
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

}
