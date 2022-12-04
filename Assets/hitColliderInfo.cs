using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitColliderInfo : MonoBehaviour
{
    float hitCounter = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "hitArea")
        {
            if (hitCounter < 2)
            {
                StartCoroutine(KnockBack());
                hitCounter++;
            }
            else
            {
                GetComponent<GoombaScript>().Kill();
            }
        }
    }

    IEnumerator KnockBack()
    {
        Transform mario = FindObjectOfType<MarioController>().transform;
        for (int i = 0; i < 30; i++)
        {
            gameObject.GetComponent<CharacterController>().Move(new Vector3(mario.forward.x,0.0f, mario.forward.z) * 0.05f);
            yield return new WaitForEndOfFrame();
        }
    }
}
