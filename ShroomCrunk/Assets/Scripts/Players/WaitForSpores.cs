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
	[SerializeField] Animator anim = null;

	Quaternion? preWaitRot = Quaternion.identity;

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

	public void Event_DipAndWait(Transform lookAt)
	{
		preWaitRot = transform.rotation;
		if (lookAt != null)
		{
			//transform.LookAt(lookAt, Vector3.up);
		}

		canGenerate = true;
		if (anim != null)
		{
			anim.SetBool("Dip", true);
		}
	}

	public void EndWait()
	{
		canGenerate = false;
		if (anim != null)
		{
			anim.SetBool("Dip", false);
		}

		if (preWaitRot != null)
		{
			transform.rotation = (Quaternion)preWaitRot;
		}

		preWaitRot = null;
	}
}
