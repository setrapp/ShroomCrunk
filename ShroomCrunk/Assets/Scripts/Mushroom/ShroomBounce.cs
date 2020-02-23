using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomBounce : MonoBehaviour
{
	[SerializeField] float bounciness = 10f;
	[SerializeField] float groundBoundFactor = 2f;

    private ShroomBounceEffect effect;
    private void Awake()
    {
        effect = GetComponent<ShroomBounceEffect>();
    }

    private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			var mover = other.GetComponentInParent<PlayerMover>();
			var groundPound = other.GetComponentInParent<GroundPound>();

			var pounding = groundPound.Pounding ? groundBoundFactor : 1;

			mover.ApplyExternalForce(Vector3.up * bounciness * pounding, true);

			var anim = GetComponentInParent<Animator>();
			if (anim != null)
			{
				anim.SetTrigger("Bounce");
			}

            if(effect != null)
            {
                effect.triggerEffect(pounding);
            }
		}
	}
}
