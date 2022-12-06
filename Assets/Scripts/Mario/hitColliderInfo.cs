using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class hitColliderInfo : MonoBehaviour
{
    float hitCounter = 0;
    [SerializeField] private UnityEvent<int> hitPunch;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "hitArea1" || other.tag == "hitArea2" || other.tag == "hitArea3")
        {
            switch (other.tag)
            {
                case "hitArea1":
                    Debug.Log("Other Tag: " + other.tag);
                    hitPunch.Invoke(1);
                    break;
                case "hitArea2":
                    hitPunch.Invoke(2);
                    break;
                case "hitArea3":
                    hitPunch.Invoke(3);
                    break;
            }

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
