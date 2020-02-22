using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GroundTracker : MonoBehaviour
{
	[SerializeField] string groundLayer = "Ground";
	[SerializeField, Range(0, 90)] float maxIncline = 45f;

	List<Collider> groundHits = new List<Collider>();

	public bool Grounded { get { return groundHits.Count > 0; } }

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.gameObject.layer.Equals(LayerMask.NameToLayer(groundLayer)) && !groundHits.Contains(collision.collider))
		{
			var minDot = Mathf.Cos((90 - maxIncline) * Mathf.Deg2Rad);
			bool hasVerticalContact = false;

			foreach(var contact in collision.contacts)
			{
				if (-Vector3.Dot(contact.normal, collision.collider.transform.forward) >= minDot)
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
