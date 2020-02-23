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
	bool dipable = false;

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

		dipable = true;
		Event_ContinueDip();

	}

	public void Event_ContinueDip()
	{
		if (dipable)
		{
			canGenerate = true;
			if (anim != null)
			{
				anim.SetBool("Dip", true);
			}
		}
	}

	public void Event_PauseDip()
	{
		canGenerate = false;
		if (anim != null)
		{
			anim.SetBool("Dip", false);
		}
	}

	public void EndWait()
	{
		Event_PauseDip();
		dipable = false;

		if (preWaitRot != null)
		{
			transform.rotation = (Quaternion)preWaitRot;
		}

		preWaitRot = null;
	}
}
