using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
	GameObject player = null;

	private void Start()
	{
		player = GameObject.FindWithTag("Player");
	}

	private void Update()
	{
		if (player != null)
		{
			transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
		}
	}
}
