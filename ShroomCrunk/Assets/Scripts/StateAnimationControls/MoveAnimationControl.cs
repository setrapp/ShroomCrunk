using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAnimationControl : StateAnimationControl
{
	[SerializeField] protected string moveParam = "Move";

	public virtual void Event_MoveBegin()
	{
		if (anim != null)
		{
			if (!string.IsNullOrEmpty(moveParam))
			{
				anim.SetBool(moveParam, true);
			}
		}
	}

	public virtual void Event_MoveEnd()
	{
		if (anim != null)
		{
			if (!string.IsNullOrEmpty(moveParam))
			{
				anim.SetBool(moveParam, false);
			}
		}
	}
}
