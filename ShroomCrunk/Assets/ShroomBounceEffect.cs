using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomBounceEffect : MonoBehaviour
{

    public MainAudio audioSpeedUp;

    public Transform effectCenter;
    public GameObject thingThatYouAreSpawning;
    public float initSpawnRadius;
    public float finalSpawnRadius;
    public float spawnTime;
    public int spawnAmount;
    public ParticleSystem particles;
    private int mask;
    public ShroomSoundScript shroomSounds;

	public Transform decorContainer = null;

	public bool triggerEffectOnFrame = false;

	public float cooldown;
    private bool cooling;
    private void Start()
    {
        audioSpeedUp = GameObject.Find("Audio")?.GetComponent<MainAudio>();
        cooling = false;
        mask = LayerMask.GetMask("GrowSurface", "Ground");
		//StartCoroutine(spawny_spawn());
		if (decorContainer == null)
		{
			decorContainer = GameObject.FindWithTag("DecorContainer")?.transform;
		}
	}

	private void Update()
	{
		if (triggerEffectOnFrame)
		{
			triggerEffect();
			triggerEffectOnFrame = false;
		}
	}

	public IEnumerator spawny_spawn(float multiplier = 1)
    {
        cooling = true;
        //Debug.Log("Starting coroutine");
        float timePassed = 0f;
        while(timePassed < spawnTime)
        {
			var superFinalSpawnRadius = finalSpawnRadius * multiplier;

            Vector3 vec = -effectCenter.transform.up * superFinalSpawnRadius + Mathf.Lerp(initSpawnRadius, superFinalSpawnRadius, timePassed / spawnTime) * effectCenter.transform.forward;
			var finalSpawnAmount = spawnAmount * multiplier;
            for (int i = 0; i < finalSpawnAmount; i++)
            {
                RaycastHit hit;
                bool hitSumthin = Physics.Raycast(effectCenter.position, vec, out hit, 1000f, mask);
                if (hitSumthin)
                {
                    spawnGrass(hit);
                }
                vec = Quaternion.AngleAxis(((float)i / (float)finalSpawnAmount + Random.Range(-.1f, .1f)) * 360, effectCenter.transform.up) * vec;
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
    public void triggerEffect(float multiplier = 1)
    {
		if (particles != null)
		{
			particles?.Play();
		}
        audioSpeedUp?.AudioSpeed();
        shroomSounds?.bounceSound();
        if (!cooling)
        {
            StartCoroutine(spawny_spawn(multiplier));
        }
        
    }

    private bool spawnGrass(RaycastHit hit)
    {
        Quaternion rot = Quaternion.AngleAxis(Random.Range(0f, 360f), hit.normal);
        GameObject instance = Instantiate(thingThatYouAreSpawning, hit.point, rot, decorContainer);
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
