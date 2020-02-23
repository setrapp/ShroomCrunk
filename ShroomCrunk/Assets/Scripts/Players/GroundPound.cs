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
	public bool Pounding { get; private set; } = false;
	bool wasGrounded = false;

	[SerializeField]
	UnityEvent onPoundedGround = null;

	public Vector3? PoundStart { get; private set; } = null;

	[SerializeField] Animator anim = null;

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
			if (!groundTracker.Grounded && poundReady && !Pounding)
			{
				mover.Body.velocity = Vector3.zero;
				bool waitForAnim = false;
				if (!string.IsNullOrEmpty(poundAnimParam))
				{
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

				PoundStart = transform.position;
				Pounding = true;
				poundReady = false;
			}
		}

		if ((groundTracker.Grounded && !wasGrounded) || mover.Body.velocity.y > 0)
		{
			Pounding = false;

			if (poundInput <= Helper.Epsilon)
			{
				poundReady = true;
			}
		}

		wasGrounded = groundTracker.Grounded;
	}

	public void FinishPound()
	{
		Pounding = true;
		mover.ApplyExternalForce(Vector3.down * poundStrength, true);
	}

	public void Event_TrackJump(bool jumping)
	{
		poundReady = poundReady && !jumping;
	}

	public void OnCollisionEnter(Collision collision)
	{
		if (Pounding)
		{
			onPoundedGround.Invoke();
		}
	}
}
