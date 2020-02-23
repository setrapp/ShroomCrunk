using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[RequireComponent(typeof(GroundTracker), typeof(PlayerMover))]
public class GroundPound : MonoBehaviour
{
	[SerializeField] string poundAnimParam = null;
	[SerializeField] float poundStrength = 100f;

	PlayerMover mover;
	GroundTracker groundTracker;
	bool poundReady = false;
	bool pounding = false;
	bool wasGrounded = false;

	[SerializeField]
	UnityEvent onPoundedGround = null;

	private void Start()
	{
		mover = GetComponent<PlayerMover>();
		groundTracker = GetComponent<GroundTracker>();
	}

	private void Update()
	{
		var poundInput = Input.GetAxis("Jump");

		if (poundInput > Helper.Epsilon)
		{
			if (!groundTracker.Grounded && poundReady && !pounding)
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

				pounding = true;
				poundReady = false;
			}
		}

		if ((groundTracker.Grounded && !wasGrounded) || mover.Body.velocity.y > 0)
		{
			pounding = false;

			if (poundInput <= Helper.Epsilon)
			{
				poundReady = true;
			}
		}

		wasGrounded = groundTracker.Grounded;
	}

	public void FinishPound()
	{
		pounding = true;
		mover.ApplyExternalForce(Vector3.down * poundStrength, true);
	}

	public void Event_TrackJump(bool jumping)
	{
		poundReady = poundReady && !jumping;
	}

	public void OnCollisionEnter(Collision collision)
	{
		if (pounding)
		{
			onPoundedGround.Invoke();
		}
	}
}
