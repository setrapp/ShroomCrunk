using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMover : MonoBehaviour, IPreventable
{
	enum MovementPlane
	{
		XY,
		XZ
	}

	enum MovementType
	{
		ScreenWalk = 0,
		CharacterSteer = 1,
		CharacterSteer_NoReverse = 2
	}

	public enum PlaneComponent
	{
		All,
		Plane,
		Normal
	}

	Rigidbody body = null;
	public Rigidbody Body => body;

	[SerializeField]
	MovementPlane defaultMovePlane = MovementPlane.XY;
	[SerializeField]
	MovementType moveType = MovementType.ScreenWalk;
	Vector3 horizontalAxis
	{
		get
		{
			return Vector3.Cross(Up, verticalAxis);
		}
	}
	Vector3 verticalAxis
	{
		get
		{
			return Vector3.Cross(Camera.main.transform.right, Up);
		}
	}
	Vector3 rotationAxis;
	[SerializeField]
	GroundTracker groundTracker = null;
	[SerializeField]
	MoveStats defaultStats = null;
	MoveStats stats = null;
	bool moving = false;
	bool preventingControl = false;

	Vector3 externalForce = Vector3.zero;

	[SerializeField] UnityEvent OnMoveBegin = null;
	[SerializeField] UnityEvent OnMoveEnd = null;

	public Vector3 Up
	{
		get
		{
			var up = Vector3.up;
			if (groundTracker == null || !groundTracker.Grounded)
			{
				switch (defaultMovePlane)
				{
					case MovementPlane.XY:
						up = Vector3.forward;
						break;
					case MovementPlane.XZ:
						up = Vector3.up;
						break;
				}
			}
			else
			{
				if (groundTracker.RecentCollision?.contacts.Length > 0)
				{
					up = groundTracker.RecentCollision.contacts[0].normal; 
				}
				else
				{
					switch (defaultMovePlane)
					{
						case MovementPlane.XY:
							up = Vector3.forward;
							break;
						case MovementPlane.XZ:
							up = Vector3.up;
							break;
					}
				}
			}

			return up;
		}
	}


	private void Start()
	{
		body = GetComponent<Rigidbody>();

		switch (defaultMovePlane)
		{
			case MovementPlane.XY:
				rotationAxis = Vector3.forward;
				break;
			case MovementPlane.XZ:
				rotationAxis = Vector3.up;
				break;
		}

		stats = defaultStats;
	}

	private void FixedUpdate()
	{
		var horizontal = Input.GetAxis($"Horizontal");
		var vertical = Input.GetAxis($"Vertical");

		if (Mathf.Abs(horizontal) > Helper.Epsilon || Mathf.Abs(vertical) > Helper.Epsilon)
		{
			if (moving == false)
			{
				moving = true;
				OnMoveBegin.Invoke();
			}
		}
		else
		{
			if (moving == true)
			{
				moving = false;
				OnMoveEnd.Invoke();
			}
		}

		body.AddForce(externalForce, ForceMode.Impulse);

		var internalForce = Vector3.zero;
		switch (moveType)
		{
			case MovementType.ScreenWalk:
				internalForce = (((horizontalAxis * horizontal) + (verticalAxis * vertical))).normalized * stats.acceleration;
				break;
			case MovementType.CharacterSteer:
			case MovementType.CharacterSteer_NoReverse:
				if (moveType == MovementType.CharacterSteer_NoReverse && vertical < 0)
				{
					vertical = 0;
				}

				switch (defaultMovePlane)
				{
					case MovementPlane.XY:
						internalForce = (((transform.right * horizontal) + (transform.up * vertical))).normalized * stats.acceleration;
						break;
					case MovementPlane.XZ:
						internalForce = (((transform.right * horizontal) + (transform.forward * vertical))).normalized * stats.acceleration;
						break;
				}
				break;
		}

		internalForce = OnPlane(internalForce);

		if (internalForce.sqrMagnitude > 0)
		{
			var lookAt = internalForce.normalized;
			var angle = Mathf.Acos(Vector3.Dot(Vector3.forward, lookAt)) * Mathf.Rad2Deg;
			if (Vector3.Dot(Vector3.right, lookAt) > 0)
			{
				angle *= -1;
			}

			// If attempting to turn fully around, randomly choose a direction to turn.
			if (Mathf.Abs(Vector3.Dot(transform.forward, lookAt) + 1) < Helper.Epsilon && Random.Range(0f, 1f) < 0.5f)
			{
				angle = -180;
			}

			if (!preventingControl)
			{
				transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(-rotationAxis * angle), stats.turnSpeed);
			}
		}


		Vector3 attemptedVelocityAdd = (internalForce * Time.fixedDeltaTime) / body.mass;
		var bodyVelocityOnPlane = OnPlane(body.velocity);
		float addPortion = 0;
		if (bodyVelocityOnPlane.sqrMagnitude < stats.maxSpeed * stats.maxSpeed)
		{
			addPortion = 1 - Mathf.Clamp(attemptedVelocityAdd.magnitude / (stats.maxSpeed - bodyVelocityOnPlane.magnitude), 0, 1);
		}

		if (AttempingMoveForward(horizontal, vertical))
		{
			body.AddForce(internalForce * addPortion);
		}

		if (!AttempingMoveForward(horizontal, vertical) && externalForce.sqrMagnitude < Helper.Epsilon)
		{
			var noDragVelocity = body.velocity;
			body.velocity *= stats.dragFactor;

			switch (defaultMovePlane)
			{
				case MovementPlane.XY:
					body.velocity = new Vector3(body.velocity.x, body.velocity.y, noDragVelocity.z);
					break;
				case MovementPlane.XZ:
					body.velocity = new Vector3(body.velocity.x, noDragVelocity.y, body.velocity.z);
					break;
			}
		}

		// Don't fall too fast.
		switch (defaultMovePlane)
		{
			case MovementPlane.XY:
				if (body.velocity.z < -stats.fallTerminalVelocity)
				{
					body.velocity = new Vector3(body.velocity.x, body.velocity.y, -stats.fallTerminalVelocity);
				}
				break;
			case MovementPlane.XZ:
				if (body.velocity.y < -stats.fallTerminalVelocity)
				{
					body.velocity = new Vector3(body.velocity.x, -stats.fallTerminalVelocity, body.velocity.z);
				}
				break;
		}

		externalForce = Vector3.zero;
	}

	private bool AttempingMoveForward(float horizontal, float vertical)
	{
		if (preventingControl)
		{
			return false;
		}

		if (stats.forceForward)
		{
			return true;
		}

		switch (moveType)
		{
			case MovementType.ScreenWalk:
				return Mathf.Abs(horizontal) > Helper.Epsilon || Mathf.Abs(vertical) > Helper.Epsilon;
			case MovementType.CharacterSteer:
			case MovementType.CharacterSteer_NoReverse:
				return vertical > Helper.Epsilon;
		}
		return false;
	}

	public void ApplyExternalForce(Vector3 force, bool freshParallel)
	{
		ApplyExternalForce(force, PlaneComponent.All, freshParallel);
	}

	public void ApplyExternalForce(Vector3 force, PlaneComponent planeComponent = PlaneComponent.All, bool freshParallel = false)
	{
		Vector3 relevantForce = force; 
		switch (planeComponent)
		{
			case PlaneComponent.Plane:
				relevantForce = OnPlane(force);
				break;
			case PlaneComponent.Normal:
				relevantForce = OnPlaneNormal(force);
				break;
		}

		// When using fresh parallel, remove all other motion in the force direction.
		if (freshParallel)
		{
			var forceDirection = force.normalized;
			externalForce -= Vector3.Project(externalForce, forceDirection);
			body.velocity -= Vector3.Project(body.velocity, forceDirection);
		}

		externalForce += relevantForce;
	}

	public void SetMoveStats(MoveStats newStats)
	{
		stats = newStats;
	}

	public void ResetMoveStats()
	{
		stats = defaultStats;
	}
    
	void IPreventable.StartPrevent()
	{
		preventingControl = true;
	}

	void IPreventable.StopPrevent()
	{
		preventingControl = false;
	}

	Vector3 OnPlane(Vector3 toProject)
	{
		return toProject - OnPlaneNormal(toProject);
	}

	Vector3 OnPlaneNormal(Vector3 toProject)
	{
		return Vector3.Project(toProject, Up);
	}
}

[System.Serializable]
public class MoveStats
{
	[SerializeField]
	public float maxSpeed = 10f;
	[SerializeField]
	public float fallTerminalVelocity = 50f;
	[SerializeField]
	public float acceleration = 100f;
	[SerializeField, Tooltip("Degrees Per Frame")]
	public float turnSpeed = 15f;
	[SerializeField]
	public float dragFactor = 0.6f;
	[SerializeField]
	public bool forceForward = false;
}
