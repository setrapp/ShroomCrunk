using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMover), typeof(GroundTracker))]
public class PlayerJump : MonoBehaviour, IPreventable
{
	[SerializeField] bool normalizeForMass = true;
	[SerializeField] float initialJump = 10f;
	[SerializeField] float extraJump = 1f;
	[SerializeField] float extraDuration = 1f;
	[SerializeField] bool requireJumpRelease = false;
	[SerializeField] UnityEvent onJump = null;
	[SerializeField] UnityEvent onJumpReleased = null;

	float jumpRemaining = 0f;
	bool readyToJump = true;

	PlayerMover mover = null;
	GroundTracker groundTracker = null;
    CharacterAudio charaAudioJump;
	bool preventingControl = false;

	private void Start()
	{
        charaAudioJump = GetComponentInChildren<CharacterAudio>();
        mover = GetComponent<PlayerMover>();
		groundTracker = GetComponent<GroundTracker>();
	}

	private void FixedUpdate()
	{
		var jumpStrength = 0f;
		var jump = Input.GetAxis($"Jump");

		if (Mathf.Abs(jump) > Helper.Epsilon & !preventingControl)
		{
            bool exclusiveForce = false;
   			if (groundTracker.Grounded)
			{
				if (readyToJump)
				{
                    charaAudioJump?.JumpAudio();

                    jumpRemaining = extraDuration;
					jumpStrength = initialJump;
					exclusiveForce = true;
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

				// TODO This should be able to work at other orientations (not necessarily based in place)
				var jumpForce = new Vector3(0, jumpStrength, 0);
				mover.ApplyExternalForce(jumpForce, PlayerMover.PlaneComponent.All, exclusiveForce);
				jumpRemaining -= Time.deltaTime;
			}
		}
		else
		{
			readyToJump = true;
			jumpRemaining = 0;
			onJumpReleased.Invoke();
		}
	}

	void IPreventable.StartPrevent()
	{
		preventingControl = true;
	}

	void IPreventable.StopPrevent()
	{
		preventingControl = false;
	}
}
