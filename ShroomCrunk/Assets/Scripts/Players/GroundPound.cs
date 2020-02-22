using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GroundTracker), typeof(PlayerMover))]
public class GroundPound : MonoBehaviour
{
	[SerializeField] string poundAnimParam = null;
	[SerializeField] float poundStrength = 100f;
	PlayerMover mover;
	GroundTracker groundTracker;
	bool poundReady = false;
	bool wasGrounded = false;

	private void Start()
	{
		mover = GetComponent<PlayerMover>();
		groundTracker = GetComponent<GroundTracker>();
	}

	private void Update()
	{
		if (Input.GetAxis("Jump") > Helper.Epsilon)
		{
			if (!groundTracker.Grounded && poundReady)
			{
				mover.Body.velocity = Vector3.zero;
				bool waitForAnim = false;
				if (!string.IsNullOrEmpty(poundAnimParam))
				{
					var anim = GetComponent<Animator>();
					if (anim != null)
					{
						anim.SetTrigger(poundAnimParam);
						waitForAnim = true;
					}
				}
				if (!waitForAnim)
				{
					FinishPound();
				}

				poundReady = false;
			}
		}
		else if ((groundTracker.Grounded && !wasGrounded) || mover.Body.velocity.y > 0)
		{
			poundReady = true;
		}

		wasGrounded = groundTracker.Grounded;
	}

	public void FinishPound()
	{
		mover.ApplyExternalForce(Vector3.down * poundStrength, true);

	}

	public void Event_TrackJump(bool jumping)
	{
		poundReady = poundReady && !jumping;
	}
}
