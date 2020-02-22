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
	MovementPlane plane = MovementPlane.XY;
	[SerializeField]
	MovementType moveType = MovementType.ScreenWalk;
	Vector3 horizontalAxis = Vector3.right;
	Vector3 verticalAxis = Vector3.up;
	Vector3 rotationAxis = Vector3.forward;
	[SerializeField]
	MoveStats defaultStats = null;
	MoveStats stats = null;
	bool moving = false;
	bool preventingControl = false;

	Vector3 externalForce = Vector3.zero;

	[SerializeField] UnityEvent OnMoveBegin = null;
	[SerializeField] UnityEvent OnMoveEnd = null;


	private void Start()
	{
		body = GetComponent<Rigidbody>();

		stats = defaultStats;

		switch (plane)
		{
			case MovementPlane.XY:
				horizontalAxis = Vector3.right;
				verticalAxis = Vector3.up;
				break;
			case MovementPlane.XZ:
				horizontalAxis = Vector3.right;
				verticalAxis = Vector3.forward;
				break;
		}
		rotationAxis = Vector3.Cross(horizontalAxis, verticalAxis);
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

				switch (plane)
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
				transform.rotation = Quaternion.RotateTowards(body.rotation, Quaternion.Euler(rotationAxis * angle), stats.turnSpeed);
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
			//body.velocity *= stats.dragFactor;

			switch (plane)
			{
				case MovementPlane.XY:
					body.velocity = new Vector3(body.velocity.x, body.velocity.y, noDragVelocity.z);
					break;
				case MovementPlane.XZ:
					body.velocity = new Vector3(body.velocity.x, noDragVelocity.y, body.velocity.z);
					break;
			}
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

	public void ApplyExternalForce(Vector3 force, PlaneComponent planeComponent = PlaneComponent.All)
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
		Vector3 planeNormal = Vector3.zero;
		switch (plane)
		{
			case MovementPlane.XY:
				planeNormal = Vector3.forward;
				break;
			case MovementPlane.XZ:
				planeNormal = Vector3.up;
				break;
		}

		return Vector3.Project(toProject, planeNormal);
	}
}

[System.Serializable]
public class MoveStats
{
	[SerializeField]
	public float maxSpeed = 10f;
	[SerializeField]
	public float acceleration = 100f;
	[SerializeField, Tooltip("Degrees Per Frame")]
	public float turnSpeed = 15f;
	[SerializeField]
	public float dragFactor = 0.6f;
	[SerializeField]
	public bool forceForward = false;
}
