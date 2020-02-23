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

			var bounceForce = Vector3.up * bounciness * pounding;
			if (groundPound.PoundStart != null)
			{
				Vector3 toPoundStart = Vector3.Project(groundPound.PoundStart.Value - mover.transform.position, Vector3.up);
				var forceToPound = toPoundStart * mover.Body.mass;
				if (forceToPound.sqrMagnitude > bounceForce.sqrMagnitude)
				{
					bounceForce = forceToPound;
				}
			}

			mover.ApplyExternalForce(bounceForce, true);

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
