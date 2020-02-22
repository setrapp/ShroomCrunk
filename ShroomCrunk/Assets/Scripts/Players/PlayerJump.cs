using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMover), typeof(GroundTracker))]
public class PlayerJump : MonoBehaviour
{
	[SerializeField] bool normalizeForMass = true;
	[SerializeField] float initialJump = 10f;
	[SerializeField] float extraJump = 1f;
	[SerializeField] float extraDuration = 1f;

	float jumpRemaining = 0f;

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
				jumpRemaining = extraDuration;
				jumpStrength = initialJump;
			}
			else
			{
				jumpStrength = extraJump;
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
			jumpRemaining = 0;
		}
	}
}
