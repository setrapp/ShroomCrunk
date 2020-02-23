using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeController : MonoBehaviour
{
	public List<GameObject> spores = null;
	List<GameObject> onSpores = null;
	List<GameObject> offSpores = null;

	private void Awake()
	{
		onSpores = new List<GameObject>();
		offSpores = new List<GameObject>();

		foreach(var spore in spores)
		{
			offSpores.Add(spore);
		}
	}

	public void Event_LoseSpores(int count)
	{
		while (onSpores.Count > 0 && count > 0)
		{
			var spore = onSpores[Random.Range(0, onSpores.Count)];
			onSpores.Remove(spore);
			offSpores.Add(spore);
			spore.SetActive(false);
			count--;
		}
	}

	public void Event_GainSpores(int count)
	{
		while (offSpores.Count > 0 && count > 0)
		{
			var spore = offSpores[Random.Range(0, offSpores.Count)];
			offSpores.Remove(spore);
			onSpores.Add(spore);
			spore.SetActive(true);
			count--;
		}
	}
}
