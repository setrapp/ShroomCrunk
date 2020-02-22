using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YourLordAndSavior : MonoBehaviour
{
	public float minY = 0;
	private void Update()
	{
		if (transform.position.y < minY)
		{
			transform.position = new Vector3(transform.position.x, minY, transform.position.z);
		}
	}
}
