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
	[SerializeField] UnityEvent onUnlanded = null;


	List<Collider> groundHits = new List<Collider>();

	public bool Grounded { get { return groundHits.Count > 0; } }

	public Vector3 GroundNormal
	{
		get
		{
			Vector3 normal = (zNormal ? Vector3.forward : Vector3.up);
			if (Grounded)
			{
				normal = zNormal ? groundHits[groundHits.Count - 1].transform.forward : groundHits[groundHits.Count - 1].transform.up;
			}

			normal *= (invertNormal ? -1 : 1);
			return normal;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		bool wasGrounded = Grounded;

		if (collision.collider.gameObject.layer.Equals(LayerMask.NameToLayer(groundLayer)) && !groundHits.Contains(collision.collider))
		{
			var minDot = Mathf.Cos(maxIncline * Mathf.Deg2Rad);
			bool hasVerticalContact = false;

			foreach(var contact in collision.contacts)
			{
				// TODO This is not enough to prevent the player from running on ground at too sharp and incline.
				var contactNormal = invertNormal ? -contact.normal : contact.normal;

				// TODO This should work on planes other XZ
				if (Vector3.Dot(contactNormal, Vector3.up) >= minDot)
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
		bool wasGrounded = Grounded;
		if (collision.collider.gameObject.layer.Equals(LayerMask.NameToLayer(groundLayer)))
		{
			if (groundHits.Contains(collision.collider))
			{
				groundHits.Remove(collision.collider);
			}
		}

		if (!Grounded && wasGrounded)
		{
			onUnlanded.Invoke();
		}
	}
}
