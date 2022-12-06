using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    public Transform m_RespawnPoint;
    [SerializeField] ParticleSystem checkpointParticles;
    private bool alreadyUnlocked = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!alreadyUnlocked)
            {
                checkpointParticles.Play();
                alreadyUnlocked = true;
            }
        }
    }
}
