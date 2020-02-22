using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAnimationControl : MonoBehaviour
{
	[SerializeField] protected Animator anim = null;

	protected virtual void Start()
	{
		if (anim == null)
		{
			anim = GetComponentInParent<Animator>();
		}
	}
}
