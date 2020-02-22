using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAnimationControl : StateAnimationControl
{
	[SerializeField] protected string jumpParam = "Jump";
	[SerializeField] protected GroundTracker groundTracker = null;

	public virtual void Event_Jump()
	{
		if (anim != null && groundTracker != null)
		{
			if (groundTracker.Grounded)
			{
				if (!string.IsNullOrEmpty(jumpParam))
				{
					anim.SetTrigger(jumpParam);
				}
			}
		}
	}
}
