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


	List<Collision> groundHits = new List<Collision>();

	public bool Grounded { get { return groundHits.Count > 0; } }

	public Vector3 GroundNormal
	{
		get
		{
			Vector3 normal = (zNormal ? Vector3.forward : Vector3.up);
			if (Grounded)
			{
				normal = groundHits[groundHits.Count - 1].contacts[0].normal;
			}

			normal *= (invertNormal ? -1 : 1);
			return normal;
		}
	}

	public Vector3 GroundPoint
	{
		get
		{
			Vector3 point = Vector3.zero;
			if (Grounded)
			{
				point = groundHits[groundHits.Count - 1].contacts[0].point;
			}

			return point;
		}
	}

	public int GroundLayer
	{
		get
		{
			var layer = 0;
			if (Grounded)
			{
				layer = groundHits[groundHits.Count - 1].collider.gameObject.layer;
			}

			return layer;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		bool wasGrounded = Grounded;

		if (collision.collider.gameObject.layer.Equals(LayerMask.NameToLayer(groundLayer)) && !trackingCollider(collision.collider))
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
				groundHits.Add(collision);
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
			if (trackingCollider(collision.collider))
			{
				for(int i = 0; i < groundHits.Count; i++)
				{
					var trackedCollision = groundHits[i];
					if (trackedCollision.collider == collision.collider)
					{
						groundHits.RemoveAt(i);
						i--;
					}
				}
			}
		}

		if (!Grounded && wasGrounded)
		{
			onUnlanded.Invoke();
		}
	}

	bool trackingCollider(Collider collider)
	{
		foreach (var collision in groundHits)
		{
			if (collision.collider == collider)
			{
				return true;
			}
		}
		return false;
	}
}
