using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckedMoveAnimationControl : MoveAnimationControl
{
	[SerializeField] protected string airborneMoveParam = null;
	[SerializeField] protected GroundTracker groundTracker = null;

	protected bool isMoving = false;

	public override void Event_MoveBegin()
	{
		if (groundTracker == null)
		{
			// If ground is not being tracked, fallback to default behavior.
			base.Event_MoveBegin();
		}
		else
		{
			isMoving = true;
			if (anim != null)
			{
				if (groundTracker.Grounded)
				{
					if (!string.IsNullOrEmpty(moveParam))
					{
						anim.SetBool(moveParam, true);
					}
				}
				else
				{
					if (!string.IsNullOrEmpty(airborneMoveParam))
					{
						anim.SetBool(airborneMoveParam, true);
					}
				}
			}
		}
	}

	public override void Event_MoveEnd()
	{
		isMoving = false;
		if (anim != null)
		{
			if (!string.IsNullOrEmpty(moveParam))
			{
				anim.SetBool(moveParam, false);
			}
			if (!string.IsNullOrEmpty(airborneMoveParam))
			{
				anim.SetBool(airborneMoveParam, false);
			}
		}
	}

	public virtual void Event_Landed()
	{
		if (anim != null && !string.IsNullOrEmpty(moveParam) && isMoving)
		{
			anim.SetBool(moveParam, true);
		}
	}

	public virtual void Event_Unlanded()
	{
		if (anim != null && !string.IsNullOrEmpty(airborneMoveParam) && isMoving)
		{
			anim.SetBool(airborneMoveParam, true);
		}
	}
}