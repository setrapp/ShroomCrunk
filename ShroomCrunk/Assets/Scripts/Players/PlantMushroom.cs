using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GroundTracker))]
public class PlantMushroom : MonoBehaviour
{
	[System.Serializable]
	public class IntEvent : UnityEvent<int> { }

	[SerializeField] mushroom_script mushroomToPlant;
	GroundTracker groundTracker = null;
	[SerializeField, Range(0, 90)] float maxIncline = 90f;
	private int mask;

	public int maxSporeCapacity = 10;
	public int sporesRemaining = 0;

	public Transform decorContainer = null;

	[SerializeField] IntEvent onAccumulate = null;
	[SerializeField] IntEvent onPlant = null;

	private void Start()
	{
		mask = LayerMask.NameToLayer("GrowSurface") | LayerMask.NameToLayer("Ground");
		groundTracker = GetComponent<GroundTracker>();
		if (decorContainer == null)
		{
			decorContainer = GameObject.FindWithTag("DecorContainer")?.transform;
		}

		Event_AccumulateSpores(maxSporeCapacity);
	}

	public void Event_AttemptPlant()
	{
		if (mushroomToPlant != null && sporesRemaining > 0)
		{
			var groundLayer = groundTracker?.RecentCollision?.collider.gameObject.layer ?? 0;
			if (groundLayer != 0 && (groundLayer & mask) == groundLayer)
			{
				var minDot = Mathf.Cos(maxIncline * Mathf.Deg2Rad);
				var contactNormal = groundTracker.RecentCollision.contacts[0].normal;

				if (Vector3.Dot(contactNormal, Vector3.up) >= minDot)
				{
					sporesRemaining--;
					var planted = Instantiate(mushroomToPlant.gameObject, groundTracker.RecentCollision.contacts[0].point, Quaternion.identity, decorContainer).GetComponent<mushroom_script>();
					planted.growUp();
					onPlant.Invoke(1);
				}
			}
		}
	}

	public void Event_AccumulateSpores(int addSpores)
	{
		float oldSpores = sporesRemaining;
		sporesRemaining = Mathf.Min(sporesRemaining + addSpores, maxSporeCapacity);

		if (sporesRemaining > oldSpores)
		{
			onAccumulate.Invoke(addSpores);
		}
	}
}
