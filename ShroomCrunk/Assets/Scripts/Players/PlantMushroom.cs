using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GroundTracker))]
public class PlantMushroom : MonoBehaviour
{

	[SerializeField] mushroom_script mushroomToPlant;
	GroundTracker groundTracker = null;
	[SerializeField, Range(0, 90)] float maxIncline = 90f;
	private int mask;

	public int maxSporeCapacity = 10;
	public int sporesRemaining = 0;

	public Transform decorContainer = null;

	private void Start()
	{
		mask = LayerMask.NameToLayer("GrowSurface") | LayerMask.NameToLayer("Ground");
		groundTracker = GetComponent<GroundTracker>();
		if (decorContainer == null)
		{
			decorContainer = GameObject.FindWithTag("DecorContainer")?.transform;
		}

		sporesRemaining = maxSporeCapacity;
	}

	public void Event_AttemptPlant()
	{
		if (mushroomToPlant != null && sporesRemaining > 0)
		{
			if (groundTracker.GroundLayer != 0 && (groundTracker.GroundLayer & mask) == groundTracker.GroundLayer)
			{
				var minDot = Mathf.Cos(maxIncline * Mathf.Deg2Rad);
				var contactNormal = groundTracker.GroundNormal;

				if (Vector3.Dot(contactNormal, Vector3.up) >= minDot)
				{
					sporesRemaining--;
					var planted = Instantiate(mushroomToPlant.gameObject, groundTracker.GroundPoint, Quaternion.identity, decorContainer).GetComponent<mushroom_script>();
					planted.growUp();
				}
			}
		}
	}

	public void Event_AccumulateSpores(int addSpores)
	{
		sporesRemaining += addSpores;
	}
}
