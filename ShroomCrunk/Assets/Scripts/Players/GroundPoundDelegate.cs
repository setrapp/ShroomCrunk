using UnityEngine;
using System.Collections;

public class GroundPoundDelegate : MonoBehaviour
{
	public void PoundIt()
	{
		GetComponentInParent<GroundPound>().FinishPound();
	}
}
