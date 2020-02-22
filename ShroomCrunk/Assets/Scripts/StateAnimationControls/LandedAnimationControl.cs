using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandedAnimationControl : StateAnimationControl
{
	[SerializeField] protected string landedParam = "Landed";
	[SerializeField] protected GroundTracker groundTracker = null;

	public virtual void Event_Landed()
	{
		if (anim != null && groundTracker != null)
		{
			if (groundTracker.Grounded)
			{
				if (!string.IsNullOrEmpty(landedParam))
				{
					anim.SetTrigger(landedParam);
				}
			}
		}
	}
}
