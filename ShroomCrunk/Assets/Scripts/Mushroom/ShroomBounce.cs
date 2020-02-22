using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomBounce : MonoBehaviour
{
	[SerializeField] float bounciness = 10f;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			var mover = other.GetComponentInParent<PlayerMover>();
			mover.ApplyExternalForce(Vector3.up * bounciness, true);

			var anim = GetComponentInParent<Animator>();
			if (anim != null)
			{
				anim.SetTrigger("Bounce");
			}
		}
	}
}
