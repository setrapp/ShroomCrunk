using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyCameraMoveDelegate : MonoBehaviour
{
	[SerializeField] ShrineScript shrine = null;

	public void Ready()
	{
		if (shrine != null)
		{
			shrine.ReadyForCameraMove();
		}
	}
}
