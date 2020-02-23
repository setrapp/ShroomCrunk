using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaitForSpores : MonoBehaviour
{
	public bool canGenerate = true;
	[SerializeField] float generateCooldown = 1f;
	Coroutine waitingToGenerate = null;
	
	[SerializeField] UnityEvent onGenerate = null;

	private void Update()
	{
		if (canGenerate)
		{
			if (waitingToGenerate == null)
			{
				waitingToGenerate = StartCoroutine(waitAndGenerate());
			}
		}
		else
		{
			if (waitingToGenerate != null)
			{
				StopCoroutine(waitAndGenerate());
				waitingToGenerate = null;
			}
		}
	}

	IEnumerator waitAndGenerate()
	{
		yield return new WaitForSeconds(generateCooldown);
		onGenerate.Invoke();
		waitingToGenerate = null;
	}
}
