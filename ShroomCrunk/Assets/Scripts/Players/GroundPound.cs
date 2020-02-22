using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GroundTracker), typeof(PlayerMover))]
public class GroundPound : MonoBehaviour
{
	[SerializeField] string poundAnimParam = null;
	[SerializeField] float poundStrength = 100f;
	[SerializeField] mushroom_script mushroomToPlant;
	PlayerMover mover;
	GroundTracker groundTracker;
	bool poundReady = false;
	bool pounding = false;
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

				pounding = true;
				poundReady = false;
			}
		}
		else if ((groundTracker.Grounded && !wasGrounded) || mover.Body.velocity.y > 0)
		{
			pounding = false;
			poundReady = true;
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
			if (mushroomToPlant != null)
			{
				var planted = Instantiate(mushroomToPlant.gameObject, collision.contacts[0].point, Quaternion.identity).GetComponent<mushroom_script>();
				planted.growUp();
			}
		}
	}
}
