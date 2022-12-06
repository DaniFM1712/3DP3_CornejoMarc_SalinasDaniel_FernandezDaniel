using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class HealthController : MonoBehaviour, IRestartGameElement
{
    [SerializeField] Image healthFill;
    [SerializeField] private GameObject reviveScreen;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private UnityEvent disableFeatureByDeath;
    [SerializeField] Text livesCounter;

    private int maxLives = 3;
    float currentLives;
    private float health;
    // Start is called before the first frame update
    void Start()
    {
        currentLives = maxLives;
        livesCounter.text = currentLives.ToString();
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
        if(health > 1f)
        {
            health = 1f;
        }
        UpdateHealthBar();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Cursor.lockState = CursorLockMode.None;
        disableFeatureByDeath.Invoke();
        StartCoroutine(waitTillEndOfAnimation());
    }
    IEnumerator waitTillEndOfAnimation()
    {
        yield return new WaitForSeconds(2f);

        if (currentLives <= 1)
        {
            livesCounter.text = currentLives.ToString();
            deathScreen.SetActive(true);
        }

        else
        {
            livesCounter.text = currentLives.ToString();
            reviveScreen.SetActive(true);
        }

        Time.timeScale = 0f;


    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        livesCounter.text = currentLives.ToString();
        health = 1;
        UpdateHealthBar();
    }

    public float getFillAmount()
    {
        return health;
    }

    public void restartLives()
    {
        currentLives = maxLives;
    }
    public void substractLive()
    {
        currentLives--;
    }
}
