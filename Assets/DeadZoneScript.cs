using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZoneScript : MonoBehaviour
{



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<MarioController>().GetDamaged(-8/8f);
        }
    }
}
