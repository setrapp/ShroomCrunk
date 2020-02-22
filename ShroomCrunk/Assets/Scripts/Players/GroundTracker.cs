using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class GroundTracker : MonoBehaviour
{
	[SerializeField] string groundLayer = "Ground";
	[SerializeField] bool zNormal = false;
	[SerializeField] bool invertNormal = false;
	[SerializeField, Range(0, 90)] float maxIncline = 45f;
	[SerializeField] UnityEvent onLanded = null;

	List<Collider> groundHits = new List<Collider>();

	public bool Grounded { get { return groundHits.Count > 0; } }

	private void OnCollisionEnter(Collision collision)
	{
		bool wasGrounded = Grounded;

		if (collision.collider.gameObject.layer.Equals(LayerMask.NameToLayer(groundLayer)) && !groundHits.Contains(collision.collider))
		{
			var minDot = Mathf.Cos((90 - maxIncline) * Mathf.Deg2Rad);
			bool hasVerticalContact = false;

			foreach(var contact in collision.contacts)
			{
				var contactNormal = invertNormal ? -contact.normal : contact.normal;
				var colliderNormal = zNormal ? collision.collider.transform.forward : collision.collider.transform.up;
				if (Vector3.Dot(contactNormal, colliderNormal) >= minDot)
				{
					hasVerticalContact = true;
					break;
				}
			}

			if (hasVerticalContact)
			{
				groundHits.Add(collision.collider);
			}
		}

		if (Grounded && !wasGrounded)
		{
			onLanded.Invoke();
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (collision.collider.gameObject.layer.Equals(LayerMask.NameToLayer(groundLayer)))
		{
			if (groundHits.Contains(collision.collider))
			{
				groundHits.Remove(collision.collider);
			}
		}
	}
}
