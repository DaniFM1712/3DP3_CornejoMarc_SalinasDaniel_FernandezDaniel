using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class HealthController : MonoBehaviour, IRestartGameElement
{
    [SerializeField] Image healthFill;
    [SerializeField] private GameObject reviveScreen;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private UnityEvent disableInput;
    private int livesAmount = 3;
    private float health;
    // Start is called before the first frame update
    void Start()
    {
        GameControllerScript.GetGameController().AddRestartGameElement(this);
        deathScreen.SetActive(false);
        reviveScreen.SetActive(false);
        health = 1.0f;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthFill.fillAmount = health;
    }

    public void UpdateHealth(float amountOfHealthRemoved)
    {
        health += amountOfHealthRemoved;
        UpdateHealthBar();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Cursor.lockState = CursorLockMode.None;
        disableInput.Invoke();
        StartCoroutine(waitTillEndOfAnimation());
    }
    IEnumerator waitTillEndOfAnimation()
    {
        yield return new WaitForSeconds(3.1f);

        if (livesAmount <= 0)
        {
            deathScreen.SetActive(true);
        }

        else
        {
            reviveScreen.SetActive(true);
        }

    }

    public void RestartGame()
    {
        health = 1;
        UpdateHealthBar();
    }

    public float getFillAmount()
    {
        return health;
    }
}
