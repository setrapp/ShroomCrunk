using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMover), typeof(GroundTracker))]
public class PlayerJump : MonoBehaviour
{
	[SerializeField] bool normalizeForMass = true;
	[SerializeField] float initialJump = 10f;
	[SerializeField] float extraJump = 1f;
	[SerializeField] float extraDuration = 1f;
	[SerializeField] bool requireJumpRelease = false;
	[SerializeField] UnityEvent onJump = null;

	float jumpRemaining = 0f;
	bool readyToJump = true;

	PlayerMover mover = null;
	GroundTracker groundTracker = null;

	private void Start()
	{
		mover = GetComponent<PlayerMover>();
		groundTracker = GetComponent<GroundTracker>();
	}

	private void FixedUpdate()
	{
		var jumpStrength = 0f;
		var jump = Input.GetAxis($"Jump");

		if (Mathf.Abs(jump) > Helper.Epsilon)
		{
			if (groundTracker.Grounded)
			{
				if (readyToJump)
				{
					jumpRemaining = extraDuration;
					jumpStrength = initialJump;
					onJump.Invoke();
				}
				else
				{
					jumpRemaining = 0;
				}
			}
			else
			{
				jumpStrength = extraJump;
				if (requireJumpRelease)
				{
					readyToJump = false;
				}
			}

			if (jumpRemaining > 0)
			{
				if (normalizeForMass)
				{
					jumpStrength *= mover.Body.mass;
				}

				var jumpForce = new Vector3(jumpStrength, jumpStrength, jumpStrength);
				mover.ApplyExternalForce(jumpForce, PlayerMover.PlaneComponent.Normal);
				jumpRemaining -= Time.deltaTime;
			}
		}
		else
		{
			readyToJump = true;
			jumpRemaining = 0;
		}
	}
}
