using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineTrigger : MonoBehaviour
{
    private ShrineScript shrine;

    private void Awake()
    {
        shrine = GetComponentInParent<ShrineScript>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player")){
            shrine.getTriggered();
        }
    }

}
