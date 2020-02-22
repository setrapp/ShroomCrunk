using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
	[SerializeField] private Animator anim = null;

	private void Start()
	{
		if (anim == null)
		{
			anim = GetComponent<Animator>();
		}
	}

	public void StartAnimation(string tag)
	{
		anim.SetBool(tag, true);
	}

	public void StopAnimation(string tag)
	{
		anim.SetBool(tag, false);
	}

	public void TriggerAnimation(string tag)
	{
		anim.SetTrigger(tag);
	}
}
